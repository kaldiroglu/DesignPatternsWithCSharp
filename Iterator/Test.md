*Claude Opus 5.5 (claude-opus-5-5) — Created on 2026-10-09*

# Tests — Iterator

For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev

37 xUnit tests in one project, `tests/Iterator.Tests`. All of them are **unit tests**: no
process boundary, no network, no real file system (the file system example is objects in
memory).

The tests are a port of the Java JUnit tests in `dev.kaldiroglu.dp.behavioral.iterator`.
Each Java test class has one C# test class (the name ends in `Tests`, as in the rest of this
repository), and each Java test method has a `[Fact]` with the same name in PascalCase, the
same display name, the same assertions and the same expected values. Every Java test is
ported; none is left out.

## What is tested

| File | Java class | Java tests | C# tests | What it checks |
|---|---|---|---|---|
| `OrgChart/Problem/ProblemTests.cs` | `orgchart.problem.ProblemTest` | 7 | 7 | Stage one: payroll works, and a caller can remove a person from the real list. Stage two: a read-only copy, a new one on every call. Stage three: two orders of walking, and the change report that copies both charts. |
| `OrgChart/Solution/SolutionTests.cs` | `orgchart.solution.SolutionTest` | 11 | 11 | `Main.Run` prints its 19 lines. The two orders differ only in rows 4 and 5. The change report: the first difference, and all three differences. Two iterators keep their own positions. No public list getter. The walk reads the live department. A change during the walk throws. Both iterators end cleanly. |
| `Gof/ListIteratorTests.cs` | `gof.ListIteratorTest` | 9 | 9 | One cursor gives 3 pairs; two iterators give 9. `Gof.Main.Run` prints six lines. One client walks an array list and a chain list. The reverse iterator. `CurrentItem` after the end throws. The internal iterator stops after 2 or walks to the end. Lists grow past 4 slots. |
| `FileSystem/DirectoryIteratorTests.cs` | `fileSystem.DirectoryIteratorTest` | 4 | 4 | `FileSystem.Test.Run` prints the four elements of Dev and the two files in Reports. A folder is one element. `Elements` is `internal` and read-only. `DirectoryIterator` is in the same namespace as `Directory`. |
| `Hw/HomeworkTests.cs` | `hw.HomeworkTest` | 6 | 6 | Two wheels of 36 spokes give one line of 72 spokes. Only parts come out. The part iterator ends. Business days skip the weekend and a holiday. A paged walk that stops early fetches 2 pages; a full walk fetches 4. |
| **Total** | | **37** | **37** | |

The helpers:

- `Printed.cs` — `By` captures `Console.Out` with a `StringWriter`, restores it in
  `finally`, and returns the printed lines split as Java's `String.lines()` splits them.
  The Java tests use `command.lender.Printed` for the same job.
- `AssemblyInfo.cs` — `[assembly: CollectionBehavior(DisableTestParallelization = true)]`.
  The org chart, GoF and file system examples print, and a `Console.SetOut` capture in one
  test class would take in another class's output if the classes ran in parallel.

## Changes from the Java tests

No expected value was changed. Where Java and C# differ, the check was moved to the closest
C# form:

- **`next()` became `MoveNext()` and `Current`.** Java's `next()` moves on and returns the
  element; C#'s `MoveNext()` moves on and `Current` returns it.
- **The end of a walk.** Java's `next()` after the last element throws
  `NoSuchElementException`. A C# enumerator answers `false` from `MoveNext()`, and `Current`
  then throws `InvalidOperationException`. The tests `BothIteratorsEndCleanly`,
  `PartIteratorEnds` and `WalkingToTheEndFetchesEveryPage` check both.
- **A change during the walk.** Java throws `ConcurrentModificationException`. The .NET
  `List<T>` enumerator throws `InvalidOperationException` when its list changes, so that is
  the exception `AddingDuringTheWalkThrows` expects.
- **Read-only lists.** Java's unmodifiable list throws `UnsupportedOperationException` on
  `add`. The C# list is read-only, and adding to it through `ICollection<T>` throws
  `NotSupportedException`.
- **`Optional<String>` became `string?`.** `Optional.of(x)` is the string `x`, and
  `Optional.empty()` is `null`.
- **No public list getter.** Java looks at public methods that return a `List`. In C# a
  getter is a property, so the test looks at public methods and public properties whose type
  is a list or a collection.
- **Package access.** Java's `elements()` is package-private. C# has no package access, and
  the port makes `Elements` `internal`. The test checks that its getter is `internal`, not
  public, not protected and not private. The test project cannot call an `internal` member,
  so it reads the list through reflection before it tries to add to it. The Java "same
  package" check is a "same namespace" check.
- **`Main.NAMES`.** Java's array is public. The C# `Gof.Main.Names` is private, so the test
  reads it through reflection rather than writing the three names again.
- **The bicycle.** Java takes the city bicycle from the Composite package
  (`ProductCatalog.cityBicycle()`, `Catalog.POWDER_COATING`). The C# port copies only the
  bill-of-materials types into `Hw.Bom.Composite`, not the catalog, so the test builds the
  same tree with the same part numbers, names and quantities. Java builds each wheel with 32
  spokes and then calls `wheel.changeQuantity(spoke, 36)`. The C# `Assembly` has no
  `ChangeQuantity`, so in `SpokesAreMultipliedDownTheTree` the wheel is built with 36 spokes
  from the start, which is the state the Java test checks. The 72 is still counted by the
  iterator, not written in the test.
- **Anonymous classes became nested classes.** Java's `ListTraverser` written inside the
  test is the nested class `SeeEveryone`.
- **`forEachRemaining` and `forEach`** became `while (MoveNext())` and `foreach` loops.
- **Exceptions.** `IllegalStateException` is `InvalidOperationException`, and
  `IndexOutOfBoundsException` is `ArgumentOutOfRangeException`.
- **`LocalDate` is `DateOnly`.**

## Run it with

```bash
cd ~/"Development/NET/Design Patterns/Design Patterns with CSharp/Iterator"

# all 37
~/.dotnet/dotnet test Iterator.sln

# one class
~/.dotnet/dotnet test Iterator.sln --filter "FullyQualifiedName~ListIteratorTests"

# one test
~/.dotnet/dotnet test Iterator.sln --filter "FullyQualifiedName~AddingDuringTheWalkThrows"
```

`dotnet` on `PATH` cannot build `net10.0`; use `~/.dotnet/dotnet`.
