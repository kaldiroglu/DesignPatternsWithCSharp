# Tests — Mediator

*Claude Opus 5.5 (claude-opus-5-5) — Created on 2026-10-09*

For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev

25 xUnit tests in one project, `tests/Mediator.Tests`. All of them are **unit tests**: no
process boundary, no network, no threads and no sleeping. Two tests read source files of
the library, and three tests capture `Console.Out`.

The tests are ported from the JUnit tests of the Java repository
(`src/test/java/dev/kaldiroglu/dp/behavioral/mediator`). Each Java test class has one C#
test class with the same name, each Java test method has one `[Fact]` with the same name in
PascalCase, and the expected values are the same. The Java `@DisplayName` text is the
`<summary>` of each test.

## Helpers

- `Printed.cs` is the port of the Java `Printed` class: `By` captures `Console.Out` with a
  `StringWriter` and restores it in `finally`; `CodeOf` reads a source file under
  `src/Mediator` without comments, found from `CallerFilePath`; `CountOf` counts a needle.
- `Fields.cs` is the port of the Java `Fields` class. `HeldBy(type)` lists the types of the
  instance fields a class declares, and the type arguments of a generic field, such as
  `Member` in `List<Member>`. `HoldsAny(type, others)` says whether any of them is one of
  the others. The backing field of an auto-property is a real field, so it is counted, as a
  Java field is.
- `AssemblyInfo.cs` turns off parallel test runs, because tests that capture `Console.Out`
  would otherwise take the output of other test classes.

## Layout

| File | Tests | What it tests |
|---|---|---|
| `Chat/Problem/Direct/DirectTest.cs` | 3 | Stage one: four members hold 12 references; a fifth member is added to one list only. |
| `Chat/Problem/Directory/DirectoryTest.cs` | 3 | Stage two: one directory; every sender applies the block rule, written twice in `Member`. |
| `Chat/Problem/Bus/BusTest.cs` | 3 | Stage three: the bus delivers a private message to 3 clients, and the guest client shows it. |
| `Chat/Solution/SolutionTest.cs` | 4 | The chat room delivers to Mert only, keeps the blocks, is the only reference a member holds; `Main.Run()` output. |
| `Gof/FontDialogTest.cs` | 5 | GoF's font dialog with a director; who holds whom before and after; `Main.Run()` output. |
| `Traffic/TrafficPoliceTest.cs` | 1 | One vehicle in the junction at a time, with a recording vehicle instead of `Car`. |
| `Hw/BankQueue/QueueManagerTest.cs` | 2 | Homework 1: customers and tellers paired in arrival order; neither holds the other. |
| `Hw/AirTraffic/ControlTowerTest.cs` | 2 | Homework 2: one aircraft on the runway at a time; every public member takes the lock. |
| `Hw/BookingForm/BookingFormTest.cs` | 2 | Homework 3: Book needs a room, a date and people who fit; a small room warns. |

Java has 10 test classes and 26 test methods. 9 classes and 25 methods are ported.

## Java tests not ported

- **`KnownUsesTest.aButtonGroup`** (1 test). It checks Swing's `ButtonGroup` and
  `JRadioButton`, a Java library known use. .NET has no such class, so there is nothing to
  port.

## Changes from the Java tests

- **`ControlTowerTest.EveryMethodIsSynchronized` reads the source.** The Java checks the
  `synchronized` modifier on each public method of `ControlTower`. The C# tower uses a
  `lock` statement on a private object, which leaves no mark on the method that reflection
  can read. The test still counts 3 public methods by reflection (`Request`, `RunwayClear`
  and the getter of `Log`), and then checks in the source, without comments, that the body
  of each of those public members contains `lock (`.
- **`DirectoryTest.TheRulesAreInTheSender` counts `blocked.Contains(Name)`**, the C#
  spelling of the Java `blocked.contains(name)`. The expected value is still 2.
- **`TrafficPoliceTest.Recorder` implements `Stop()`**, which is the Java `stopp()`; the
  README explains the name.

The slow and thread-based `Traffic.Car` is not tested, in C# as in Java: a waiting car
sleeps for a second. No test failed, and no behavior difference between the C# and the
Java was found.

## Run it with

```bash
cd ~/"Development/NET/Design Patterns/Design Patterns with CSharp/Mediator"

# all 25
~/.dotnet/dotnet test Mediator.sln

# one class
~/.dotnet/dotnet test Mediator.sln --filter "FullyQualifiedName~FontDialogTest"
```

`dotnet` on `PATH` cannot build `net10.0`; use `~/.dotnet/dotnet`.
