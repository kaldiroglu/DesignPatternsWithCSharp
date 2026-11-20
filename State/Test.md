*Claude Opus 5.5 (claude-opus-5-5) — Created on 2026-10-09*

# Tests — State

For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev

65 xUnit tests in one project, `tests/State.Tests`. All of them are **unit tests**: no
process boundary, no network. Five tests read C# source files of the library, found from
the test file's own path (`CallerFilePath`), so they do not depend on the working directory.

The tests are a port of the Java JUnit tests in `dev.kaldiroglu.dp.behavioral.state`. Each
Java test class has one C# test class, and each Java test method has a `[Fact]` with the same
name in PascalCase, the same assertions and the same expected values. The numbers the State
deck quotes about the order, the TCP connection and the door are asserted here as they are
in Java.

## What is tested

| File | Java class | Java tests | C# tests | What it checks |
|---|---|---|---|---|
| `Order/ProblemTests.cs` | `ProblemTest` | 15 | 14 | Stage one's four flags (sixteen combinations, five statuses). Stage two's four switches with no `default`. Stage three's second shipment counts attempts 4, 5 and 6 and never goes back to the warehouse; a reset of the count to 0 fixes that; back at the warehouse the order still holds the old tracking number. The lines `StageOneMain`, `StageTwoMain`, `StageThreeMain` and `SecondShipmentMain` print. |
| `Order/SolutionTests.cs` | `SolutionTest` | 11 | 11 | The second shipment counts 1, 2 and 3 and goes back. A new `Shipped` starts at zero. Refusals by default. The end states override nothing. The five states. The context only forwards. The lines `SecondShipmentMain`, `SideBySideMain` and `RefusedRequestMain` print. |
| `Order/StateMachineTests.cs` | `StateMachineTest` | 9 | 9 | The order as a finite state machine: the table `TransitionTableMain` prints, six of its twenty-five cells are moves, one start and two end states, the guard on `FailDelivery`, a refused event, an action that depends on the state and the event, the switch codes the same machine, each switch method is one column and each state class is one row. |
| `Gof/TCPConnectionTests.cs` | `TCPConnectionTest` | 7 | 7 | Both versions give the same log and state for all 125 sequences of three requests. Five switches over three states. Default "ignored" behavior and an `internal` `ChangeState`. Each concrete state is one shared object. |
| `Door/DoorTests.cs` | `DoorTest` | 8 | 8 | All three versions print the same story, starting closed. The states decide in `Pattern1`, and its state interface has only the requests. The manager decides in `Pattern2` from the state and the request; a request with no line keeps the state; its states hold no fields. No state stores a `bool`. Two state objects per `Pattern1` door. The version without the pattern has one `bool` and one `if` in each method. |
| `Person/PersonTests.cs` | `PersonTest` | 3 | 3 | `Main` output. The person is not a state. A greeting does not change the state. |
| `KnownUsesTests.cs` | `KnownUsesTest` | 5 | 2 | The .NET rows of the known-uses table: `TaskStatus` and `WebSocketState` are enums with the names the deck shows. |
| `Hw/HomeworkTests.cs` | `AirConditionerTest` | 4 | 4 | The room and the target choose cooling, idle or heating; off stays off; only `AcState` declares `Running`; `Main` output. |
| `Hw/HomeworkTests.cs` | `DocumentTest` | 4 | 3 | A draft cannot be approved; the table has four transitions; `Main` output. The Java check that `Status` declares no methods has no C# test, because a C# enum cannot declare methods. |
| `Hw/HomeworkTests.cs` | `VendingMachineTest` | 4 | 4 | A sold-out machine returns the coin; one coin at a time; a refill; `Main` output. |
| **Total** | | **70** | **65** | |

`Printed.cs` holds the helpers: `By` captures `Console.Out` (restored in `finally`) and
returns the printed lines; `CodeOf` reads a source file and strips its comments, because
well-commented code names what it leaves out; `CountOf` counts a substring.

The examples print, so `AssemblyInfo.cs` turns off xUnit's parallel run of test classes.
Otherwise one class's capture of `Console.Out` would catch another class's output.

## Java tests not ported, or changed

- **`KnownUsesTest.threadState`, `futureState`, `futureTask`, `futureTaskHasSevenIntStates`
  and `futureTaskUsesCompareAndSet` are not ported.** They check JDK types (`Thread.State`,
  `Future.State`, and `FutureTask`'s seven `int` states, its private `volatile int state` and
  its static `VarHandle`).
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
- **`ProblemTest.enumConstantsHaveTheirOwnMethods` is not ported as its own test.** It
  checks that `OrderStatus` is an enum whose constants have their own methods. A C# enum
  cannot have methods, so `TheDataLivesInTheOrder` checks the C# form instead: `OrderStatus`
  is an abstract class, not an enum, with five shared instances.
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

# all 65
~/.dotnet/dotnet test State.sln

# one class
~/.dotnet/dotnet test tests/State.Tests --filter "FullyQualifiedName~TCPConnectionTests"

# with each test's name printed
~/.dotnet/dotnet test tests/State.Tests -v normal
```

`dotnet` on `PATH` cannot build `net10.0`; use `~/.dotnet/dotnet`.
