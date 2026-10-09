*Claude Opus 5.5 (claude-opus-5-5) — Created on 2026-10-09*

# Tests — State

For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev

31 xUnit tests in one project, `tests/State.Tests`. All of them are **unit tests**: no
process boundary, no network. Two tests read C# source files of the library, found from
the test file's own path (`CallerFilePath`), so they do not depend on the working directory.

The tests are a port of the Java JUnit tests in `dev.kaldiroglu.dp.behavioral.state`. Each
Java test class has one C# test class, and each Java test method has a `[Fact]` with the same
name in PascalCase, the same assertions and the same expected values. The numbers the State
deck quotes about the order, the TCP connection and the door are asserted here as they are
in Java.

## What is tested

| File | Java class | Java tests | C# tests | What it checks |
|---|---|---|---|---|
| `Order/ProblemTests.cs` | `ProblemTest` | 8 | 8 | Stage one's four flags (sixteen combinations, five statuses). Stage two's four switches with no `default`. Stage three's second shipment counts attempts 4, 5 and 6 and never goes back to the warehouse. |
| `Order/SolutionTests.cs` | `SolutionTest` | 9 | 9 | The second shipment counts 1, 2 and 3 and goes back. A new `Shipped` starts at zero. Refusals by default. The end states override nothing. The five states. The context only forwards. `Main`'s five lines. |
| `Gof/TCPConnectionTests.cs` | `TCPConnectionTest` | 7 | 7 | Both versions give the same log and state for all 125 sequences of three requests. Five switches over three states. Default "ignored" behavior and an `internal` `ChangeState`. Each concrete state is one shared object. |
| `Door/DoorTests.cs` | `DoorTest` | 5 | 5 | `Pattern1` and `Pattern2` print the same story. The states decide in `Pattern1`; the manager decides in `Pattern2`. Two state objects per `Pattern1` door. The version with a boolean. |
| `KnownUsesTests.cs` | `KnownUsesTest` | 3 | 2 | The .NET rows of the known-uses table: `TaskStatus` and `WebSocketState` are enums with the names the deck shows. |
| **Total** | | **32** | **31** | |

`Printed.cs` holds the helpers: `By` captures `Console.Out` (restored in `finally`) and
returns the printed lines; `CodeOf` reads a source file and strips its comments, because
well-commented code names what it leaves out; `CountOf` counts a substring.

The examples print, so `AssemblyInfo.cs` turns off xUnit's parallel run of test classes.
Otherwise one class's capture of `Console.Out` would catch another class's output.

## Java tests not ported, or changed

- **`KnownUsesTest.threadState`, `futureState` and `futureTask` are not ported.** They check
  JDK types (`Thread.State`, `Future.State`, `FutureTask`'s private `volatile int state`).
  .NET has none of these. In their place `KnownUsesTests` has two tests for the .NET rows of
  the same slide table: `TaskStatus` (its names include `Created` and `Running`, and
  `Created` is first) and `WebSocketState` (its names include `Connecting` and `Open`).
- **`SolutionTest.fiveSealedStates` is changed.** Java checks that `OrderState` is a sealed
  interface that permits exactly five records. C# has no sealed interface. The C# test
  checks the form the port uses: exactly five types in the library implement `IOrderState`
  (`Placed`, `Paid`, `Shipped`, `Delivered`, `Cancelled`), each is a sealed record, only
  `Shipped` has positional parameters (`TrackingNumber`, `FailedAttempts`), and the other
  four have none. It cannot check that the compiler forbids a sixth implementation, because
  in C# it does not.
- **`ProblemTest.theDataLivesInTheOrder` is changed.** `OrderStatus` is a class with five
  shared instances in C#, not an enum. The test counts the five `static readonly` instances.
  Java checks that the enum declares no instance fields; the C# class declares one, the field
  behind its `Name` property, which Java keeps in `java.lang.Enum`. So the test checks that
  this is the only instance field, and that each constant's class declares none. The order's
  data is two `internal` properties (`TrackingNumber`, `FailedAttempts`) rather than two
  fields, so the test reads properties.
- **`ProblemTest.switchesHaveNoDefault` counts `status switch`** rather than
  `switch (status)`, because the C# port uses switch expressions.
- **`SolutionTest.theContextOnlyForwards`** checks for `bool` instead of `boolean`, and checks
  both `failedAttempts` and `FailedAttempts`, because C# names a property in PascalCase.
- **`TCPConnectionTest.defaultsAndChangeState`** checks that `ChangeState` is `internal`
  (Java: package-private), the closest C# access level.
- **`IllegalStateException` is `InvalidOperationException`** throughout, as in the port.

No test found a difference in behavior between the C# port and the Java.

## Run it with

```bash
cd ~/"Development/NET/Design Patterns/Design Patterns with CSharp/State"

# all 31
~/.dotnet/dotnet test State.sln

# one class
~/.dotnet/dotnet test tests/State.Tests --filter "FullyQualifiedName~TCPConnectionTests"

# with each test's name printed
~/.dotnet/dotnet test tests/State.Tests -v normal
```

`dotnet` on `PATH` cannot build `net10.0`; use `~/.dotnet/dotnet`.
