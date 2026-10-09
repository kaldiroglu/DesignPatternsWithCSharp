# Tests — Memento

*Claude Opus 5.5 (claude-opus-5-5) — Created on 2026-10-09*

For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev

27 xUnit tests in one project, `tests/Memento.Tests`. All of them are **unit tests**: no
process boundary, no network, no threads and no sleeping. Three tests capture `Console.Out`.

The tests are ported from the JUnit tests of the Java repository
(`src/test/java/dev/kaldiroglu/dp/behavioral/memento`). Each Java test class has one C#
test class with the same name, each Java test method has one `[Fact]` with the same name in
PascalCase, and the expected values are the same except where noted below. The Java
`@DisplayName` text is the `<summary>` of each test.

## Helpers

- `Printed.cs` is the port of the Java `Printed` class: `By` captures `Console.Out` with a
  `StringWriter` and restores it in `finally`. It also has `CodeOf` and `CountOf`, as in the
  other patterns; no Memento test reads source text.
- `Methods.cs` is the port of the Java `Methods` class. `PublicMethodsOf(type)` lists the
  public methods a class declares, sorted. `PublicSettersOf(type)` keeps the ones that set a
  value: a property setter, named `set_Name` by the compiler, or a method named `Set...`.
- `AssemblyInfo.cs` turns off parallel test runs, because tests that capture `Console.Out`
  would otherwise take the output of other test classes.

## Layout

| File | Tests | What it tests |
|---|---|---|
| `Game/Problem/Setters/SettersTest.cs` | 2 | Stage one: loading is correct, and any code may set `Health` to 999. |
| `Game/Problem/History/HistoryTest.cs` | 2 | Stage two: loading is correct with no public getters or setters; the player keeps the save slots. |
| `Game/Problem/Copy/CopyTest.cs` | 3 | Stage three: the copy shares the inventory list, so loading gives `[shield, potion]`. |
| `Game/Solution/SolutionTest.cs` | 5 | The checkpoint keeps the promise, can be loaded twice, and is closed; public setters in each stage; `Main.Run()` output. |
| `Gof/ConstraintSolverTest.cs` | 5 | GoF's constraint solver: undo by moving back leaves the bend at 20, the memento puts it at 50; undo order; the memento is closed; `Main.Run()` output. |
| `Gui/GuiTest.cs` | 3 | The window's memento restores the saved state, and its state is public; `Test.Run()` output. |
| `Hw/Editor/HistoryTest.cs` | 3 | Homework 1: undo and redo with two stacks; a new change clears redo; the cursor is saved. |
| `Hw/Rollback/BatchTest.cs` | 2 | Homework 2: a failed batch is rolled back; a good batch is done. |
| `Hw/Incremental/SheetTest.cs` | 2 | Homework 3: two cells on a 10,000-cell sheet make a memento of 2 entries; undo in reverse order. |

Java has 10 test classes and 28 test methods. 9 classes and 27 methods are ported.

## Java tests not ported

- **`KnownUsesTest.aStateEdit`** (1 test). It checks Swing's `StateEdit` and
  `StateEditable`, a Java library known use. .NET has no such class, so there is nothing to
  port.

## Changes from the Java tests

The C# mementos are built in a different way, so the tests that check that a memento is
closed check the C# idea. In the Java the memento is a nested class with private fields and
a private constructor. A C# outer class cannot read a nested class's private members, so in
the port the class that holds the state is a **private nested class**, and the caretaker
sees only an **empty public interface** (`ConstraintSolver.IMemento`, `Player.ICheckpoint`).

- **`ConstraintSolverTest.TheMementoIsClosed`** checks that `ConstraintSolver.IMemento` is
  an interface with no members, that the class `Memento` that holds the bend is nested
  private and implements it, and that the caretaker `MoveCommand` keeps its memento as
  `IMemento`, not as the private class. The Java checks private fields, a private
  constructor and no methods.
- **`SolutionTest.TheCheckpointIsClosed`** checks that the class `Checkpoint` is nested
  private and holds `Health`, `Inventory` and `Position` (the Java field names, as
  properties), that `Player.ICheckpoint` has no members, and that the caretaker `Game` keeps
  only a `Stack<Player.ICheckpoint>`. The Java checks the three private fields, a private
  constructor and no methods.
- **`SettersTest.AnyoneMaySetHealth` expects `set_Health`, `set_Inventory` and
  `set_Position`** where the Java expects `setHealth`, `setInventory` and `setPosition`.
  The Java setters are C# property setters, and these are the names the compiler gives them.
  The test sets `player.Health = 999`, where the Java calls `setHealth(999)`.
- **`GuiTest.TheMementoIsOpen` expects `get_State` and `set_State`** where the Java expects
  `getState` and `setState`, for the same reason: they are the accessors of the property
  `State`.
- **`HistoryTest.LoadingIsCorrect` (stage two) looks for getters as `get_...` or `Get...`**,
  and **`ThePlayerKeepsTheSlots` looks for a `Stack<>` field** where the Java looks for a
  `Deque`. The port uses `Stack<T>`, as the README says.

The thread-based `Pattern1` and `Pattern2` examples are not tested, in C# as in Java: they
sleep and their output order changes from run to run. No test failed, and no behavior
difference between the C# and the Java was found.

## Run it with

```bash
cd ~/"Development/NET/Design Patterns/Design Patterns with CSharp/Memento"

# all 27
~/.dotnet/dotnet test Memento.sln

# one class
~/.dotnet/dotnet test Memento.sln --filter "FullyQualifiedName~ConstraintSolverTest"
```

`dotnet` on `PATH` cannot build `net10.0`; use `~/.dotnet/dotnet`.
