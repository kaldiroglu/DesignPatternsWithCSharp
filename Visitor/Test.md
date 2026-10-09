*Claude Opus 5.5 (claude-opus-5-5) — Created on 2026-10-09*

# Tests — Visitor

For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev

44 xUnit tests in one project, `tests/Visitor.Tests`. All of them are **unit tests**: no
process boundary, no network. Some tests read the library's own C# source files, and some
capture `Console.Out` to read what an example prints.

The tests are a port of the Java JUnit tests in
`src/test/java/dev/kaldiroglu/dp/behavioral/visitor/` of **Design Patterns with Java**. Each
Java test class has one C# test class, and each Java test method has a C# `[Fact]` with the
same name in PascalCase, the same assertions and the same expected values, except where
listed below.

## What is tested

| File | Java | C# | What it checks |
|---|---|---|---|
| `Checkout/ProblemTests.cs` | 7 | 7 | The three naive stages: each item taxes itself (103), overloads tax every item at 20% (128), type tests give 103 until a gift card makes it 123 and shipping 60. |
| `Checkout/SolutionTests.cs` | 10 | 10 | The visitors give 103 and 50 with a gift card; one visit method per item; no type tests in the visitors; the records and the switch in `Modern`; the output of `Checkout.Solution.Main.Run()`. |
| `Gof/CompilerTests.cs` | 7 | 7 | GoF's compiler before and after the pattern: the same errors, code and text. |
| `File/FileTests.cs` | 5 | 5 | `Accept` returns the visitor's answer; the type tests and casts in `FileOperator`; the 0.80 thresholds. |
| `Factory/HealthVisitorTests.cs` | 4 | 4 | Who gets a health check, and that `Boss` shares no parent with `Employee`. |
| `Interpreter/RuleTests.cs` | 5 | 5 | The shop's rule language: four products, the limits, `Describe()`, the five rule records, the output of `Main.Run()`. |
| `Hw/Expression/ExpressionTests.cs` | 4 | 4 | `(2 + 3) * -4` is -20, has depth 3, and prints as `((2 + 3) * -4)`. |
| `KnownUsesTests.cs` | 4 | 2 | .NET's `ExpressionVisitor`, in place of the JDK types. |
| **Total** | **46** | **44** | |

`Printed.cs` holds the helpers: `By` captures `Console.Out` and restores it in `finally`;
`CodeOf` reads a source file under `src/Visitor` and removes its comments, so a check that
a file contains no X does not match a comment that names X. The source root is found with
`CallerFilePath`, captured in that file, so the tests do not depend on the working
directory. `AssemblyInfo.cs` turns off parallel test runs, because a capture opened by one
test class would also collect another class's output.

## Java tests that were changed or not ported

**C# has no sealed interface.** Four Java checks rely on it, and each has the closest C#
check instead:

- `ProblemTests.TheItemIsNotSealed` — Java asserts `Item` is not sealed. C# asserts
  `IItem` is an interface (an interface can never be sealed). The Java counts three
  `instanceof`; C# counts three `is Book`-style type tests (after removing strings), and
  `else {` is matched with a line break allowed before the brace.
- `SolutionTests.TheSealedItem` — Java asserts `Modern.Item` is sealed and permits exactly
  four records. C# asserts that `Modern.IItem` is an open interface, that the library has
  exactly four implementations of it (`Book`, `Food`, `Electronics`, `GiftCard`), and that
  each is a sealed record.
- `SolutionTests.TheSwitch` — Java counts four `case` labels and no `default`. The C#
  switch expression has no `case` labels: the test counts four type arms and one `_` arm
  that throws, and asserts no `default`. It adds one check that the Java does not need:
  a fifth item kind, written in the test, compiles and makes `Tax.Of` throw
  `InvalidOperationException` at run time.
- `RuleTests.FiveRuleClasses` — Java asserts `Rule` is sealed and permits five records.
  C# asserts that the library has exactly five implementations of `IRule` (`CategoryIs`,
  `PriceBelow`, `And`, `Or`, `Not`), each a sealed record.

**Type tests are written differently.** Java looks for `instanceof`; C# looks for `is`:

- `SolutionTests.NoTypeTests` — no `is`, no `default` and no `_ =>` in `TaxVisitor`,
  `ShippingVisitor`, `ReceiptLineVisitor` and `Checkout`.
- `FileTests.TypeTestsAndCasts` — `FileOperator` contains `aFile is XMLFile` and
  `(TextFile)aFile`; `FileVisitor` contains no `is` once its strings are removed (it prints
  sentences such as "It is a valid XML file").

**Reflection reads property getters as methods.** Where Java counts declared methods, the
C# tests leave out property accessors (`IsSpecialName`), so `Name` and `Price` are not
counted as operations.

**`KnownUsesTest` — four Java tests, not ported.** They check JDK types:
`java.nio.file.FileVisitor` and `SimpleFileVisitor`,
`javax.lang.model.element.ElementVisitor`, `com.sun.source.tree.TreeVisitor` and
`TreeScanner`, and the class-file API (`ClassElement` sealed, `ClassModel` iterable). None
of them exists in .NET. In their place, two tests check the first row of the deck's .NET
table: `ExpressionVisitor` has `VisitBinary`, `VisitConstant`, `VisitParameter` and
`VisitLambda`, and a subclass of it rewrites the constants of `x => (x + 1) * 2`. The
second row, Roslyn's `CSharpSyntaxWalker`, is not tested, because it needs the
`Microsoft.CodeAnalysis.CSharp` package. The third row, a `switch` that needs a default
case, is checked by `SolutionTests.TheSwitch`.

## Run it with

```bash
cd ~/"Development/NET/Design Patterns/Design Patterns with CSharp/Visitor"

# all 44
~/.dotnet/dotnet test Visitor.sln

# one class
~/.dotnet/dotnet test tests/Visitor.Tests --filter "FullyQualifiedName~SolutionTests"

# with each test's name printed
~/.dotnet/dotnet test tests/Visitor.Tests -v normal
```

`dotnet` on `PATH` cannot build `net10.0`; use `~/.dotnet/dotnet`.
