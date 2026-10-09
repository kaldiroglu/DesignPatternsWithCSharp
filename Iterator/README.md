# Iterator — Design Patterns with C#

*Claude Opus 5.5 (claude-opus-5-5) — Created on 2026-10-08*

For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev

The C# port of the Iterator material from **Design Patterns with Java**
(`kaldiroglu/DesignPatternsWithJava`, package root
`dev.kaldiroglu.dp.behavioral.iterator`). Every class that repository carries is here, in
the same shape, under the root namespace `dev.kaldiroglu.Iterator`.

## What Iterator is for

A collection gives out a separate object that walks it. The object keeps the position of
the walk, so the collection does not have to show how it stores its elements, and any
number of walks can run at the same time.

That is what lets payroll and the phone book walk a department in two different orders
without seeing its lists, lets a report walk two org charts side by side and stop at the
first difference, and lets a loop read a paged web service as if it were one long list.

## The examples

| Namespace | What it shows |
|---|---|
| `Iterator.OrgChart` | The main worked example. A department has members and sub-departments. `Problem` is three naive stages — the department gives out its lists (`OpenDepartment`, with `PayrollRun` writing the recursion), copies everyone into a new list (`CopyingDepartment`), and walks itself with a callback (`CallbackDepartment`). Stage three cannot walk two charts side by side, so its `ChangeReport` copies both. `Solution` gives the department two iterators, `DepthFirstIterator` and `LevelOrderIterator`, and its `ChangeReport` moves two of them forward together. |
| `Iterator.Gof` | GoF's own example (Design Patterns, pp. 257–271). `Problem.CursorList` keeps the cursor in the list, so a loop inside a loop makes 3 pairs instead of 9. `Solution` has GoF's `IIterator<T>` (`First`, `Next`, `IsDone`, `CurrentItem`), `AbstractList<T>` with `CreateIterator()`, `List<T>` and `ChainList<T>`, their iterators, `PrintEmployees`, and the internal iterator `ListTraverser<T>` with `PrintNEmployees`. |
| `Iterator.FileSystem` | A `Directory` holds files, shortcuts, aliases and other directories, and gives out a `DirectoryIterator` that walks its own elements only. |
| `Iterator.Hw` | The three homework exercises: every part in a bill of materials with its total quantity (`Bom.PartIterator`), the business days between two dates with no list behind them (`Calendar.BusinessDays`), and a paged source read one page at a time (`Paging.PagedIterator`). |

### Things worth stopping on

**Where stage three fails.** `CallbackDepartment` copies nothing and offers two orders, but
the department runs the walk. A report that must compare two charts cannot hold two walks
at once, so `Problem.ChangeReport` copies both charts into lists first. GoF implementation
issue 1 (who controls the iteration?) makes the same point.

**An iterator over a tree remembers its path.** `DepthFirstIterator` keeps a stack of the
departments it still has to visit; `LevelOrderIterator` keeps a queue. That is the only
difference between them. GoF implementation issue 7 (iterators for composites).

**An iterator does not need a collection.** `BusinessDays` computes the next date when it
is asked. `PagedIterator` fetches the next page only when the current one is used up, so a
loop that stops at the first match never fetches the later pages.

## The C# form: `IEnumerable<T>` and `IEnumerator<T>`

Java's `Iterable<T>` and `Iterator<T>` are .NET's `IEnumerable<T>` and `IEnumerator<T>`:
`iterator()` is `GetEnumerator()`, `hasNext()` and `next()` together are `MoveNext()` and
`Current`. The port uses them wherever the Java uses `java.util`, so `Department`
implements `IEnumerable<Employee>` and `foreach` works on it.

**The iterators are written out as classes.** A C# developer would usually write
`DepthFirstIterator` and the others with `yield return`: the method reads like a loop and
the compiler builds the enumerator class. The port does not do that, because the class
with its own position is what the deck teaches. `OrgChart.Solution.DepthFirstWithYield` is
the one `yield` version, beside the hand-written one, so the two can be compared. It is
the same pattern: the compiler writes the ConcreteIterator instead of the programmer.

**GoF's own interface stays GoF's.** `Gof.Solution.IIterator<T>` keeps the four operations
from the book, because that shape is what the GoF slides show.

## Architecture

- **One class library, `Iterator`**, holding every example as nested namespaces —
  `OrgChart`, `Gof`, `FileSystem` and `Hw`. Sources mirror namespaces:
  `src/Iterator/OrgChart/Solution/…`.
- **A console runner, `Iterator.Demo`**, that runs the Java original's `main` methods —
  the org chart's problem and solution, GoF's lists with their problem and solution, the
  file system and the three homework exercises — and, in addition, the `yield` walk, each
  on its own.
- `net10.0`, nullable reference types on, implicit usings on — set once in
  `Directory.Build.props` and inherited by both projects.
- **A test project, `Iterator.Tests`**, with 34 xUnit tests ported from the Java JUnit tests.

## Differences from the Java original

The port is faithful in behavior. The output of every runner entry except `orgchart-yield`
is byte for byte the output of the Java `main` method it ports, and the whole runner prints the same under
a Turkish locale. What had to change:

- **`Iterable`/`Iterator` became `IEnumerable<T>`/`IEnumerator<T>`.** Each Java iterator is
  a class implementing `IEnumerator<T>` with `MoveNext`, `Current`, `Reset` and `Dispose`.
  Java's `next()` returns the element and moves on; C#'s `MoveNext()` moves on and `Current`
  returns the element. `Reset()` starts the walk again where that is cheap, and throws
  `NotSupportedException` in `PagedIterator`, because starting again would fetch the pages
  again.
- **`PartIterator` finds the next part when `MoveNext()` is called.** The Java iterator
  finds it one step ahead, in its constructor and in `next()`, so that `hasNext()` can
  answer. The parts and their order are the same.
- **An `Iterable` written as a lambda needs a class in C#.** Java's `byLevel()` and
  `partsOf()` return `() -> new …Iterator(…)`. `IEnumerable<T>` is not a delegate type, so
  both return a small internal `LambdaEnumerable<T>` that holds the same lambda. It is not
  part of the pattern.
- **`BusinessDays`' iterator is a private nested class**, because C# has no anonymous
  classes. `LocalDate` is `DateOnly`, and the holidays are a `HashSet<DateOnly>`.
- **`PageSource<T>` is a delegate.** Java declares a `@FunctionalInterface` so that callers
  pass a lambda; the C# form of that is a delegate, and callers still pass a lambda.
- **GoF's `List<T>` keeps its name.** It shares the name with .NET's
  `System.Collections.Generic.List<T>`. Inside `Gof.Solution` the plain name means GoF's
  class, because a type in the current namespace wins over one brought in by a `using`; the
  two places there that need .NET's list write `System.Collections.Generic.List<string>` in
  full, as the Java writes `java.util.List`. In `Gof.Main`, one namespace up, GoF's types
  are written `Solution.List<Employee>`, `Solution.IIterator<Employee>` and so on, and the
  plain `List<string>` is .NET's.
- **Interfaces take the `I` prefix**: GoF's `Iterator` is `IIterator<T>`, and the file
  system's `Storage` is `IStorage`.
- **The file system's type parameter keeps the name `Storage`.** In
  `DirectoryIterator<Storage>`, `Storage` is a type parameter, as in Java, and one of the
  deck's exercises asks about it. In C# the interface is `IStorage`, so the parameter no
  longer hides it, but the name still reads like a type. C# naming would call it
  `TStorage`. The unchecked cast in the constructor is kept: `(List<Storage>)(object)` in
  C#, which works only when `Storage` is `IStorage`, as every caller uses it.
- **`Directory.iterator()` is `GetEnumerator()`.** Because of that name, C#'s `foreach`
  would accept a `Directory` even though it does not implement `IEnumerable`; Java's
  for-each would not. `Test` walks it with `MoveNext()` and `Current`, as the Java walks it
  with `hasNext()` and `next()`.
- **`copy()` uses `MemberwiseClone()`.** Java's `clone()` throws here, because the classes
  do not implement `Cloneable`, so the Java `copy()` prints "Problem with copying" and
  returns `null`. C# has no such check, so the copy succeeds and is shallow. No example
  calls `Copy()`.
- **`Optional<String>` became `string?`.** Both `ChangeReport` classes return `null` when
  the charts are the same; `Main` prints `"none"` for it, as the Java prints
  `orElse("none")`.
- **Package access became `internal`.** `Department.Members`/`Units`,
  `Directory.Elements` and `ChainList<T>.Head` are visible to the iterators through `internal`, which is the whole
  assembly rather than one package. C# has nothing closer.
- **Accessors became properties**: `Name`, `Role`, `Count`, `Members`, `Units`,
  `Elements`, `IsDirectory`, `Parent`, `Lines`, `PagesFetched`. `OpenDepartment.Members` and
  `OpenDepartment.Units` still return the real mutable `List<T>`, because handing out the
  internal list is the fault that class shows. `Directory.Elements` is `internal` and
  read-only, as in the Java, where `elements()` is package-private and
  `DirectoryIterator` lives in the same package. Other
  methods that return a list return `IReadOnlyList<T>`. GoF's four operations stay methods
  (`IsDone()`, `CurrentItem()`), because they are the book's interface.
- **`Consumer<Employee>` became `Action<Employee>`** in `CallbackDepartment`.
- **`IllegalStateException` is `InvalidOperationException`**, and
  `IndexOutOfBoundsException` is `ArgumentOutOfRangeException`.
- **`ChainListIterator.Next()` after the end does nothing.** The Java version throws a
  `NullPointerException` there; the C# one uses `?.`. Before the end the two are the same.
- **The bill of materials is copied, not referenced.** `hw.bom` uses the Composite deck's
  `BomComponent`, `Assembly`, `Part`, `Service`, `BomLine` and `Money`. The Composite port
  has them, but no pattern folder in this repository references another one's project, so
  the types are copied into `Hw.Bom.Composite` and trimmed to what the homework needs. The
  roll-ups (cost, weight, part count), the caches, the parent links and the cycle check are
  left out. Each copied type says so in its doc comment.
- **GoF's `Main` prints `false` in lower case**, as Java prints a `boolean`, and prints its
  lists as `[a, b, c]`, as Java's `List.toString()` does.
- **The `main` methods became `Run()` methods** called by `Iterator.Demo`. The
  commented-out lines in the Java `Test` are not ported.
- **`hw.bom`'s `Main` builds the bicycle itself.** The Java `Main` takes it from the
  Composite package's `ProductCatalog.cityBicycle()` and then sets 36 spokes per wheel with
  `changeQuantity`. The copied types have neither, so the C# `Main` builds the same tree
  with 36 spokes from the start, as the homework tests do. The printed lines are the same.
- **The `uml/` diagrams in the Java packages are not ported yet.**

## Tests

`tests/Iterator.Tests` holds 34 xUnit tests, one test class for each Java test class, with
the same assertions and the same expected values. Every Java test is ported. `Test.md`
lists what is tested and every place where a check had to take a different C# form.

```bash
~/.dotnet/dotnet test Iterator.sln
```

## Run it with

The `dotnet` on `PATH` cannot build this repository — a tracked `global.json` at the root
pins the SDK to 10.0.0. Use the .NET 10 SDK directly:

```bash
cd ~/"Development/NET/Design Patterns/Design Patterns with CSharp/Iterator"

# build everything
~/.dotnet/dotnet build Iterator.sln

# every example, in the order the course presents them
~/.dotnet/dotnet run --project src/Iterator.Demo

# one example on its own
~/.dotnet/dotnet run --project src/Iterator.Demo -- orgchart
```

The runner accepts: `orgchart-problem`, `orgchart`, `orgchart-yield`, `gof`, `gof-problem`,
`gof-solution`, `filesystem`, `hw-bom`, `hw-calendar`, `hw-paging`.

Run the tests with `~/.dotnet/dotnet test Iterator.sln`.

From the repository root, `~/.dotnet/dotnet build "Design Patterns with CSharp.sln"`
builds Iterator along with every other pattern.
