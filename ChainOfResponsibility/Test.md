# Tests — Chain of Responsibility

*Claude Opus 5.5 (claude-opus-5-5) — Created on 2026-10-09*

For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev

34 xUnit tests in one project, `tests/ChainOfResponsibility.Tests`. All of them are **unit
tests**: no process boundary, no network. Five tests read source files of the library, and
five tests capture `Console.Out`.

The tests are ported from the JUnit tests of the Java repository
(`src/test/java/dev/kaldiroglu/dp/behavioral/chainOfResponsibility`). Each Java test class
has one C# test class with the same name, each Java test method has one `[Fact]` with the
same name in PascalCase, and the expected values are the same. The Java `@DisplayName` text
is the `<summary>` of each test.

## Helpers

`Printed.cs` is the port of the Java `Printed` class:

- `By(action)` runs the action with `Console.Out` sent to a `StringWriter`, puts the old
  writer back in `finally`, and returns the printed lines as Java's `String.lines()` does.
- `CodeOf(path)` reads a source file under `src/ChainOfResponsibility` and removes every
  comment. It finds the folder from `CallerFilePath`, as `Bridge.Tests` does, so the tests do
  not depend on the working directory.
- `CountOf(text, needle)` counts how many times the needle occurs.

`AssemblyInfo.cs` turns off parallel test runs, because tests that capture `Console.Out`
would otherwise take the output of other test classes.

## Layout

| File | Tests | What it tests |
|---|---|---|
| `Expense/Problem/ProblemTest.cs` | 8 | The three Part 1 designs on six expenses. Stage three lets Burak and Cem approve their own; source checks count the names and the `new` calls. |
| `Expense/Solution/SolutionTest.cs` | 8 | The chain gives the six answers the rule asks for, the audit log sees 6, a finance check is one more link, no link names another, and `Main.Run()` output. |
| `Gof/HelpTest.cs` | 6 | GoF's context-sensitive help: a button, its dialog and the application answer in turn; the help desk before the pattern; `Main.Run()` output. |
| `CallCenter/CallCenterTest.cs` | 4 | Standard, gold and VIP customers reach the right desk; the VIP desk has no next desk. |
| `Hw/Middleware/MiddlewareTest.cs` | 3 | Homework 2: every link runs, the admin check stops a request, the list decides the order. |
| `Hw/CashDispenser/NoteSlotTest.cs` | 3 | Homework 3: each slot pays its part; 380 and 260 leave 10 that cannot be paid. |
| `Hw/Maintenance/MaintenanceTest.cs` | 2 | Homework 1: each request finds its developer; a request nobody takes goes back to the team lead. |

Java has the same 7 test classes and 34 test methods. Every one is ported.

## Changes from the Java tests

- **`HelpTest.TheHelpDeskKnowsEveryControl` counts different text for the same four names.**
  The Java `HelpDesk` is a `switch` statement, and the test counts `case "` and `, "`. The C#
  `HelpDesk` is a switch expression, so the test counts `" =>` (each name before an arrow)
  and `" or "` (two names on one arm). The expected value is still 4: print button, ok
  button, printer list and print dialog.
- **`HelpTest.NobodyHasHelp` passes `null!` to `Application`.** The C# constructor takes a
  non-nullable `string`; the `!` only tells the compiler that the null is on purpose.
- **`SolutionTest.AFinanceCheckIsOneMoreLink` uses a small private class** where the Java
  uses an anonymous subclass of `ExpenseHandler`. C# has no anonymous subclasses.

No test failed, and no behavior difference between the C# and the Java was found.

## Run it with

```bash
cd ~/"Development/NET/Design Patterns/Design Patterns with CSharp/ChainOfResponsibility"

# all 34
~/.dotnet/dotnet test ChainOfResponsibility.sln

# one class
~/.dotnet/dotnet test ChainOfResponsibility.sln --filter "FullyQualifiedName~ProblemTest"
```

`dotnet` on `PATH` cannot build `net10.0`; use `~/.dotnet/dotnet`.
