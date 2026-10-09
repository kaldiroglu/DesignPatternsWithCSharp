# Visitor — Design Patterns with C#

*Claude Opus 5.5 (claude-opus-5-5) — Created on 2026-10-09*

For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev

The C# port of the Visitor material from **Design Patterns with Java**
(`kaldiroglu/DesignPatternsWithJava`, package root
`dev.kaldiroglu.dp.behavioral.visitor`). Every class that repository carries is here, in
the same shape, under the root namespace `dev.kaldiroglu.Visitor`: 118 Java files, 118 C#
files, one for one.

## What Visitor is for

An operation that runs over a set of objects of different classes is written as its own
class, the visitor. The visitor has one method for each class it visits. Each object has one
method, `Accept`, which calls the visitor's method for its own class. A new operation is a
new visitor, and the classes it visits do not change.

That is what lets a checkout add tax, shipping and receipt lines to books, food, electronics
and gift cards without one line of checkout code in the item classes. It also means that
when a new kind of item is added, every visitor that does not handle it stops compiling.

## The examples

| Namespace | What it shows |
|---|---|
| `Visitor.Checkout` | The main worked example: a shop that taxes books at 5%, food at 1% and electronics at 20%, and later sells gift cards, which carry no tax and are not shipped. `Problem.Methods` is stage one: each item computes its own tax, shipping cost and receipt line. `Problem.OverloadedTax` is stage two: one class with one `TaxOf` per kind of item. `Problem.TypeTestTax` and `TypeTestShipping` are stage three: one class that tests the type of each item. `Solution` has the element `IItem`, the four record elements, the visitor `IItemVisitor<R>`, the visitors `TaxVisitor`, `ShippingVisitor` and `ReceiptLineVisitor`, and the object structure `Checkout`. `Modern` is the same items as records with one switch for the tax. `Solution.Main` runs one cart through all of them. |
| `Visitor.Gof` | GoF's own example (Design Patterns, pp. 331–344): a compiler's syntax tree. `Problem` has every job — `TypeCheck`, `GenerateCode`, `PrettyPrint` — on every node. `Solution` has `INodeVisitor` with `VisitAssignment`, `VisitVariableRef`, `VisitConstant` and `VisitAdd`, and three visitors that walk the tree themselves. |
| `Visitor.Hw` | The three homework exercises: printers for bank accounts that write to an output they are given (`AccountPrint`), a folder tree that walks itself (`FileTree`), and an arithmetic expression that gains a `Neg` node and a `DepthCounter` operation (`Expression`). |
| `Visitor.File` | Text files and XML files that must be checked before they are read. `Domain` has the files only; `Problem1` tests types in the client; `Problem2` moves the type tests into `FileOperator`; `Pattern1` has `File.Accept(IVisitor)` and `FileVisitor`. |
| `Visitor.Factory` | A company's employees and its boss get a health check. `Boss` is not an `Employee`, so a visitor can visit classes that have no common parent. `HR` creates random employees. |
| `Visitor.Interpreter` | Interpreter, taught as a section of the Visitor deck (Design Patterns, pp. 243–255): a rule language for a shop. `IRule` is the abstract expression; `CategoryIs` and `PriceBelow` are terminal expressions; `And`, `Or` and `Not` are nonterminal ones. Each interprets itself against a `Product`. The Java `Rule` is a `sealed` interface; C# cannot close an interface, so `IRule` is open and each record is sealed. |
| `Visitor.Animal` | A dog and a cat, and a feeder. `Problem` tests the type in `Feed`; `Pattern1` lets the animal's `Accept` choose the right `Feed` overload; `Pattern2` has one feeder per animal, and an animal refuses the wrong feeder. |
| `Visitor.Pattern` | An earlier outline of GoF's nodes. Its methods are empty, as in the Java, so there is nothing to run. |

### Things worth stopping on

**Where stage three fails.** A cart of a book for 40, food for 100 and electronics for 500
has tax 103. Stage two charges 128, and stage three charges 103 — until a gift card for 100
is added. Then stage three charges tax 123 and shipping 60, where the correct figures are
103 and 50. The visitors charge 103 and 50. The demo prints all of these lines.

**Overloads are chosen at compile time, in C# as in Java.** In `OverloadedTax.Total`, each
item is held as an `IItem`. C# chooses between overloads from the static type of the
argument, so `TaxOf(item)` calls `TaxOf(IItem)` for every item, and every item is taxed at
the standard rate of 20%. The port keeps it that way on purpose, so the total is 128, as in
the Java.

**Double dispatch.** Inside `Book.Accept`, `this` has the static type `Book`, so
`visitor.Visit(this)` chooses `Visit(Book)` at compile time. The first call, `item.Accept`,
chooses by the item's run-time type; the second, `visitor.Visit`, by the visitor's.

**Who walks the structure.** GoF implementation issue 2 (who is responsible for traversing
the object structure?). In `Gof.Solution` and `Hw.Expression` the visitor walks the tree by
calling `Accept` on the children. In `Hw.FileTree` the structure walks itself:
`Folder.Accept` visits the folder and then each child. In `Checkout.Solution` the object
structure, `Checkout`, walks the cart.

## A switch over records, and why C# does not check it

In Java, `checkout.modern.Item` is a `sealed` interface: it lists the four records, and no
other class may implement it. A `switch` over a sealed type must cover every permitted
class, or it does not compile. So `Tax.of` has no `default`, and a fifth item kind is a
compile error in every switch that forgets it. That is the same check the visitor's
interface gives, without `accept` and `visit`.

**C# has no sealed interface.** Any class may implement `Modern.IItem`, so the compiler
cannot know that the four records are all there are. The port is an interface, four
`sealed record`s, and a switch expression with type patterns:

```csharp
public static int Of(IItem item) => item switch
{
    Book book => book.Price * 5 / 100,
    Food food => food.Price * 1 / 100,
    Electronics electronics => electronics.Price * 20 / 100,
    GiftCard => 0,
    _ => throw new InvalidOperationException("No tax rule for " + item.GetType().Name)
};
```

The last arm, `_`, is needed. Without it the compiler gives warning CS8509 ("The switch
expression does not handle all possible values of its input type (it is not exhaustive)"), because some other class could implement
`IItem`. With it, a fifth item kind compiles without any error or warning, and `Tax.Of`
throws when it meets one at run time. **In C#, a missing case here is not a compile error.**
An abstract record with a private constructor would close the hierarchy, but the C#
compiler still does not use that to check a switch. So in C#, the visitor's interface is the
way to get the check: add `Visit(GiftCard)` to `IItemVisitor<R>`, and every visitor without
it stops compiling.

## Architecture

- **One class library, `Visitor`**, holding every example as nested namespaces —
  `Checkout`, `Gof`, `Hw`, `File`, `Factory`, `Animal` and `Pattern`. Sources mirror
  namespaces: `src/Visitor/Checkout/Solution/…`.
- **A console runner, `Visitor.Demo`**, that runs the Java original's `main` methods — the
  checkout, GoF's compiler, the three file examples, the health check and the three animal
  examples — and, in addition, the three homework exercises, each on its own.
- `net10.0`, nullable reference types on, implicit usings on — set once in
  `Directory.Build.props` and inherited by both projects.
- There is no test project, as requested.

## Differences from the Java original

The port is faithful in behavior. The output of `checkout`, `gof`, `animal-problem`,
`animal-pattern1` and `animal-pattern2` is byte for byte the output of the Java `main`
methods. The homework output is byte for byte the output of a Java driver that makes the
same calls. These outputs are the same under a Turkish and a Swedish locale. The file
examples and `factory` choose at random, so their output has the same shape as the Java's,
but not the same lines. What had to change:

- **Interfaces take the `I` prefix**: `IItem` (in `Problem`, `Problem.Methods`, `Solution`
  and `Modern`), `IItemVisitor<R>`, `INode` (in `Gof.Problem`, `Gof.Solution` and
  `Pattern.Problem`), `INodeVisitor`, `IAccount`, `IAccountVisitor`, `IEntry`,
  `IEntryVisitor`, `IExpr`, `IExprVisitor<R>`, `IVisitor` (in `File.Pattern1` and
  `Factory`), `IAnimal` (in all three animal namespaces) and `IFeeder`. GoF's method names
  are PascalCase: `VisitAssignment`, `VisitVariableRef`, `VisitConstant`, `VisitAdd`.
- **The generic method `<R> R accept(ItemVisitor<R>)` is `R Accept<R>(IItemVisitor<R>)`**,
  and the same for `IExpr`. Java's `Integer` is `int`: `TaxVisitor` and `ShippingVisitor`
  implement `IItemVisitor<int>`, and `Evaluator` and `DepthCounter` implement
  `IExprVisitor<int>`.
- **Java records are `sealed record`s** with the Java accessor names as properties:
  `Book(string Name, int Price)`, `AssignmentNode(string Variable, INode Value)`,
  `FileEntry(string Name, long Size)` and so on. No example prints a record, so their
  `ToString()` is left as C# writes it.
- **The `sealed` interface in `Checkout.Modern` is a plain interface**, and its switch has a
  `_` arm that throws. See the section above. The Java variable `sealed` in `Main` is named
  `records`, because `sealed` is a C# keyword.
- **The printers in `Hw.AccountPrint` take a `TextWriter`** where the Java takes a
  `java.io.PrintWriter`. The runner passes `Console.Out` to `TextPrinter`, and a
  `StringWriter` to `HtmlPrinter`, as the Java driver does.
- **`Folder`'s varargs constructor is `params IEntry[] children`.**
- **`Math.random()` is `Random.Shared.NextDouble()`**, in the three file examples and in
  `HR`, with the same thresholds.
- **Constants are PascalCase**: `Rates.BookTax`, `Rates.StandardShipping`,
  `Employee.BaseSalary`, `Manager.ManagementPayment`. The two `Rates` classes are `static`
  classes; Java's are `final` with a private constructor. The solution's `Rates` is
  `internal`, as the Java one is package-private.
- **Stage one keeps its operations as methods**: `Tax()`, `ShippingCost()` and
  `ReceiptLine()` on `Problem.Methods.IItem`. They are the operations the stage puts on the
  item, so they stay methods, as `PrettyPrint()` does on GoF's nodes.
- **Accessors became properties**: `Name`, `Price`, `Owner`, `Children`, `Size`, `Value`,
  `Left`, `Right`, `Operand`, `Errors`, `Code`, `Text`, `Lines`, `Total`, `Age`. In
  `Factory.Employee` the four getter and setter pairs became read-write properties `No`,
  `Name`, `Year` and `Department`. In `File.Pattern1.File`, `getName()` is the property
  `Name`, and the protected field `name` stays.
- **`virtual` is explicit.** In `Factory`, `Employee.Work`, `CalculateSalary` and
  `PrintInfo` and `Manager.Manage` are `virtual`, because the subclasses override them.
  `Pattern2.AbstractAnimal` declares `Eat` and `Accept` as `abstract`: Java's abstract class
  does not mention them, but a C# abstract class must declare every member of its interface.
- **Lists are returned as `IReadOnlyList<T>`**, copied first, as the Java returns
  `List.copyOf` or `Stream.toList()`. Parameters that take a list are `IReadOnlyList<T>`;
  the Java problem nodes' `Set<String>` and `List<String>` are `ISet<string>` and
  `IList<string>`, because the nodes add to them.
- **The runner prints lists as `[a, b, c]`**, as Java's `List.toString()` does, in GoF's
  `Main`.
- **Number formats do not depend on the machine's locale.** Numbers that are printed are
  formatted with the invariant culture. Under a Swedish locale, -20000 and -20 would
  otherwise print with the minus sign U+2212.
- **The `main` methods became `Run()` methods** called by `Visitor.Demo`. The commented-out
  lines in the Java (the animal `Test` classes and the `Feeder` overloads) are not ported.
  The homework demos are only in the runner.
- **Old behavior is kept on purpose, each with a `// NOTE:` comment that says the Java does
  the same**:
  - `Factory.Secretary` never assigns `managerServed`, so `Serve()` prints
    `Secretary Sevim serves her manager: null`. C# would print an empty string for `null`,
    so the port writes `"null"` itself. No demo calls `Serve()`.
  - `Factory.Company.SetVisitor` does not store the visitor: it applies it to every
    employee at once. The name is kept.
  - `Pattern.Problem.INode` has a method misspelled `GeneratoCode`; the name is kept.
  - `Animal.Problem.Test` keeps the comments `// Prints "Gnaws bones"`. They are wrong: the
    program prints `Woof` and `Meeoow`.
- **`HR.GetAnEmployee` ends with `return e!;`.** The Java can return `null` in theory; the
  random number is always 0 to 9, so one of the ten cases always sets `e`. The `!` tells
  the compiler so.
- **Names that clash, and how they are resolved.**
  - The root namespace ends in `Visitor`, but no type is named `Visitor`: the two Java
    interfaces named `Visitor` are `IVisitor`, so the namespace and the types never meet.
  - The namespace `File` holds four classes named `File`, and implicit usings bring in
    `System.IO.File`. Inside each `File.*` namespace the plain name means the example's
    class, because a type in the current namespace wins over an imported one. Anywhere else
    under `dev.kaldiroglu.Visitor`, `File` means the namespace; no code there uses
    `System.IO.File`.
  - The namespace `Checkout` holds the class `Checkout.Solution.Checkout`. Inside
    `Checkout.Solution` the plain name means the class.
  - Most examples have a class named `Main` or `Test`, as in the Java. `Main` is a static
    class with a `Run()` method, which C# allows. In `Visitor.Demo` the runner reaches them
    through aliases: `CheckoutMain`, `GofMain`, `FileProblem1Test`, `FactoryTest`,
    `AnimalPattern2Test` and so on.
  - The Java `Main` classes write the `problem` and `modern` types with their full package
    names. The C# `Main` classes write `Problem.Book`, `Modern.Tax` and
    `Solution.AddNode`: from inside `Checkout.Solution`, `Problem` and `Modern` are found as
    sibling namespaces.
- **The `uml/` diagrams, the `CD1.png`, `CD2.png`, `CD3.png` and `SD1.png` images, and the
  `ReadMe.txt` notes in the Java packages are not ported yet.**

## Run it with

The `dotnet` on `PATH` cannot build this repository — a tracked `global.json` at the root
pins the SDK to 10.0.0. Use the .NET 10 SDK directly:

```bash
cd ~/"Development/NET/Design Patterns/Design Patterns with CSharp/Visitor"

# build everything
~/.dotnet/dotnet build Visitor.sln

# every example, in the order the course presents them
~/.dotnet/dotnet run --project src/Visitor.Demo

# one example on its own
~/.dotnet/dotnet run --project src/Visitor.Demo -- checkout
```

The runner accepts: `checkout`, `gof`, `file-problem1`, `file-problem2`, `file-pattern1`,
`factory`, `interpreter`, `animal-problem`, `animal-pattern1`, `animal-pattern2`, `hw-accountprint`,
`hw-filetree`, `hw-expression`.

There are no tests to run.

From the repository root, `~/.dotnet/dotnet build "Design Patterns with CSharp.sln"`
builds Visitor along with every other pattern.
