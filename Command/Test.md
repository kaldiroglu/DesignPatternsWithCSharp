*Claude Opus 5.5 (claude-opus-5-5) — Created on 2026-10-09*

# Tests — Command

For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev

61 xUnit tests in one project, `tests/Command.Tests`. All of them are **unit tests**: no
process boundary, no network. Some tests read C# source files of the library. They find the
files from the test file's own path (`CallerFilePath`), so they do not depend on the working
directory.

The tests are a port of the Java JUnit tests in `dev.kaldiroglu.dp.behavioral.command`.
Each Java test class has one C# test class (the name ends in `Tests`, as in the rest of this
repository), and each Java test method has a `[Fact]` with the same name in PascalCase, the
same display name, the same assertions and the same expected values. Every Java test is
ported; none is left out. The Java helper class `lender.Printed` has no tests of its own; it
became `Printed.cs`.

## What is tested

| File | Java class | Java tests | C# tests | What it checks |
|---|---|---|---|---|
| `Account/ProblemTests.cs` | `account.ProblemTest` | 10 | 10 | Stage one undoes one step only, and two of its fields start with `last`. Stage two has 8 public operations, 3 of them banking, and two switches with 4 `case` labels. Stage three moves both switches into the teller. The reversal: undo takes back half a transfer and 300.00 lira vanish. |
| `Account/SolutionTests.cs` | `account.SolutionTest` | 9 | 9 | One undo takes back a whole transfer. Undo and redo. `CloseOut` gives back the 750.00 it took. A failed request is not recorded. A transfer is all or nothing. The teller names no operation. A new operation is one class. Standing orders. `Execute` takes no arguments. |
| `Gof/MenuTests.cs` | `gof.MenuTest` | 14 | 14 | Before the pattern, the menu item names `Application` and `Document` and branches on its label; `paste` fails on the first click. With the pattern, the toolkit classes name no receiver, `ICommand` has one method, and paste, open, macro, `SimpleCommand`, `SetCommand` and `Menu.Click` work. |
| `Ac/AirConditionerTests.cs` | `ac.AirConditionerTest` | 10 | 10 | What the switch and the four commands print. The switch holds four `ICommand` fields. `Undo` and `Redo` print nothing. |
| `Lender/Problem1/Problem1Tests.cs` | `lender.problem1.Problem1Test` | 3 | 3 | `Lend` takes the concrete `Borrower` class. `Main.Run` prints one loan. |
| `Lender/Problem2/Problem2Tests.cs` | `lender.problem2.Problem2Test` | 4 | 4 | `Lend` takes the `IBorrower` interface, and a borrower written in the test works. |
| `Lender/Pattern/PatternTests.cs` | `lender.pattern.PatternTest` | 5 | 5 | `Lend` takes an `ICommand` and names no concrete command. The amount arrives at `Execute`. The tax office ignores the amount. |
| `Lender/Lambda/LambdaTests.cs` | `lender.lambda.LambdaTest` | 3 | 3 | `Lend` takes an `Action<int>` (Java: `IntConsumer`). A lambda receives the money. Same output as the pattern version. |
| `Ac/Lambda/LambdaSwitchTests.cs` | `ac.lambda.LambdaSwitchTest` | 3 | 3 | The switch holds three `Action<Temperature>` fields and one `Action`, and no `ICommand`. Same output as `Person`. |
| **Total** | | **61** | **61** | |

The helpers:

- `Printed.cs` — `By` captures `Console.Out` with a `StringWriter`, restores it in
  `finally`, and returns the printed lines split as Java's `String.lines()` splits them (a
  line break at the very end does not add an empty line). `LendParameters` reads the
  parameter types of a class's `Lend` method.
- `SourceText.cs` — reads a file under `src/Command`, removes its comments, and counts a
  substring.
- `AssemblyInfo.cs` — `[assembly: CollectionBehavior(DisableTestParallelization = true)]`.
  The lender and air conditioner classes print, and a `Console.SetOut` capture in one test
  class would take in another class's output if the classes ran in parallel.

## Changes from the Java tests

No expected value was changed. Where Java and C# differ, the check was moved to the closest
C# form:

- **Lambdas became small classes.** Java's `Command`, `Borrower` and `Transaction` are
  functional interfaces or are implemented by anonymous classes in the tests. A C# lambda
  cannot implement an interface, so the tests use small private nested classes: `Run`
  (an `ICommand` that runs an `Action`), `Recorder`, and `MonthlyFee`.
- **`main` became `Run()`.** `Main.main(new String[0])` is `Main.Run()`.
- **"Imports" in `MenuTest`.** Java checks the `import` lines of the toolkit's
  `MenuItem`, `Menu` and `Command`. The C# files need no `using` line: they sit in
  namespaces inside `Gof`, so they see `Application` and `Document` without one. The C# test
  checks two things instead: the code (comments removed) names the type as a whole word,
  and the types in the class's fields, properties, constructors and methods include it. The
  problem `MenuItem` passes both checks for `Application`; the three solution files pass
  neither, for `Application`, `Document` or `Clipboard`.
- **Public methods in `account.ProblemTest`.** Java counts the public methods, and its
  accessors such as `balance()` and `canUndo()` are methods. In C# they are properties
  (`Balance`, `CanUndo`), so the test counts the declared public methods that are not
  property accessors together with the declared public properties. The counts stay 8 and 3.
- **Field names in `theAccountCarriesBookkeeping`.** Java reads `owner`, `balance`,
  `lastKind` and `lastAmount` in that order. In C# `balance` is the property `Balance`, kept
  in a field the compiler writes (`<Balance>k__BackingField`), and the other fields start
  with an underscore. The test reads the property's name for the backing field, drops the
  underscore, and compares the four names as a set, because reflection does not promise the
  declaration order. It still checks four fields and two that start with `last`.
- **The interface has three methods.** Java counts `execute`, `undo` and `description`. In
  C# `Description` is a property; its getter `get_Description` is the third method.
- **Type tests.** Java checks that the teller contains no `instanceof`; the C# test checks
  that it contains neither of the words `is` and `as`.
- **Names in the source.** `transfer`, `withdraw(`, `deposit(` and `record(` are
  `Transfer`, `Withdraw(`, `Deposit(` and `Record(`.
- **Exceptions.** `IllegalStateException` is `InvalidOperationException`,
  `IllegalArgumentException` is `ArgumentException`, and the `NullPointerException` from
  `setCommand(null)` is `ArgumentNullException`.
- **Output not checked.** In `onTwiceOffTwice`, `turnOff` and `eachOnlyGoesOneWay`, Java
  calls `turnOn(22)` once without capturing what it prints. The C# test captures it and ignores it, so the test run
  prints nothing.

## Run it with

```bash
cd ~/"Development/NET/Design Patterns/Design Patterns with CSharp/Command"

# all 61
~/.dotnet/dotnet test Command.sln

# one class
~/.dotnet/dotnet test Command.sln --filter "FullyQualifiedName~MenuTests"

# one test
~/.dotnet/dotnet test Command.sln --filter "FullyQualifiedName~TheReversal"
```

`dotnet` on `PATH` cannot build `net10.0`; use `~/.dotnet/dotnet`.
