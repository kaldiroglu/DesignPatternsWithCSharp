# Memento — Design Patterns with C#

*Claude Opus 5.5 (claude-opus-5-5) — Created on 2026-10-09*

For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev

The C# port of the Memento material from **Design Patterns with Java**
(`kaldiroglu/DesignPatternsWithJava`, package root `dev.kaldiroglu.dp.behavioral.memento`).
Every class that repository carries is here, in the same shape, under the root namespace
`dev.kaldiroglu.Memento`: 39 Java files and 39 C# files in the library, one for one, and
the runner `Memento.Demo`.

## What Memento is for

An object saves its state into another object, the memento, and can take that state back
later. A third object, the caretaker, keeps the mementos and decides when to save and when
to go back. The caretaker cannot read or change what is inside a memento. So the object
keeps its fields private, and still gets undo, checkpoints and rollback.

The cost is memory: every memento is a copy of some state. When the state is large, a
memento can hold only what one change touched (an incremental memento), and then the
mementos must be restored in the reverse order they were made.

## The examples

| Namespace | What it shows |
|---|---|
| `Memento.Game` | The main worked example: a game checkpoint. A player has health, a position and an inventory. The promise: loading gives back exactly what the player had at the checkpoint. `Problem.Setters` is stage one: the game reads and writes every field through public getters and setters, so anyone can set health to 999. `Problem.History` is stage two: the player keeps its own save slots. `Problem.Copy` is stage three: the game keeps a copy of the player, but the copy shares the inventory list with the player. `Solution` has the originator `Player`, the memento `Player.ICheckpoint`, the caretaker `Game`, and `Main`, which runs stage one, stage three and the memento. Each stage also has a `Main` of its own. |
| `Memento.Gof` | GoF's own example (Design Patterns, pp. 283–291): a constraint solver keeps a line with one bend between two boxes. `Problem` undoes a move by moving the box back, and the bend does not come back. In `Solution`, `MoveCommand` (the caretaker) takes a memento from `ConstraintSolver` (the originator) before the move and gives it back on undo. `Main` runs the same steps in both designs; `Problem.Main` and `Solution.Main` run one design each. |
| `Memento.Hw` | The three homework exercises: a `TextEditor` with undo and redo kept by a `History` (`Editor`), a `Batch` of transfers that gives every `Account` its memento back when one transfer fails (`Rollback`), and a `Sheet` whose memento holds only the cells one edit changed (`Incremental`). |
| `Memento.Gui` | A window whose state is one object, `GuiComponentState`, kept by a `GuiComponentMemento`. `Test` saves, moves and resizes the window, then undoes. |
| `Memento.Pattern1` | An originator whose state changes every second on one thread, and a caretaker thread that keeps mementos in a stack, saves every two seconds and undoes every fifth time. `Memento` is a class of its own, with a public `State`. |
| `Memento.Pattern2` | The same, with the memento nested in the originator, so only the originator can read it. |

### Things worth stopping on

**Where stage three fails.** After loading, the stage-three player has health 100 and is at
the bridge, but carries `[shield, potion]` — what it had when it died, not
`[sword, shield]`. The copy constructor copies the reference to the inventory list, so the
checkpoint and the player share one list. The port keeps this on purpose.

**The memento after the move.** In GoF's example the bend goes from 50 to 20 during the move.
Without a memento it stays at 20 after undo; with one it is at 50 again.

**Rollback without reversing.** `Batch` does not know how to reverse a transfer. It takes a
memento of every account first, and when Ayse cannot pay 70 it gives every account its
memento back: Ali 100, Ayse 50, Can 0.

## How the memento's state is kept hidden in C#

In the Java, each memento is a public nested class with private fields and a private
constructor: `Player.Checkpoint`, `ConstraintSolver.Memento`, `TextEditor.Snapshot`,
`Account.Saved` and `Sheet.Change`. The caretaker can name the type and hold an instance,
but cannot create one or read it. The enclosing class can, because in Java an enclosing class
reads the private members of a class nested in it.

**C# does not allow this.** In C# a nested class can read the private members of the class
that encloses it, but an enclosing class cannot read the private members of a class nested
in it. A direct copy of the Java does not compile. The options considered:

- **`internal` members on the memento.** These compile, but then every class in the assembly
  can read the state — `Game` too. Rejected.
- **The memento reads and writes the originator itself.** A nested class may write the
  originator's private fields, so the memento could have a `RestoreInto(Player)` method. But
  that method must be visible to the player, so it must be `internal` or `public`, and then
  the caretaker can call it as well. Rejected.
- **An empty public interface and a private class (chosen).** Each originator declares a
  nested public interface with no members — `Player.ICheckpoint`,
  `ConstraintSolver.IMemento`, `TextEditor.ISnapshot`, `Account.ISaved` — and a nested
  `private sealed class` that implements it and holds the state. The originator's save
  method returns the interface; its load method takes the interface and casts it back to the
  private class. The caretaker sees only the interface: it can keep the memento and give it
  back, but it cannot name the private class, so it can neither create one nor read one,
  except through reflection. The private class's members are `public`, but a member of a
  private class is visible only where the class is: inside the originator.

`Sheet.IChange` declares one member, `Size`, because the Java `Sheet.Change` has one public
method, `size()`. Nothing else of the change is visible.

One difference remains. In Java, the caretaker cannot give the originator a memento of the
wrong kind, because no other class can create one. In C# another class could implement the
empty interface, and the cast in the load method would then throw `InvalidCastException`.
The load methods' XML comments say so.

## Architecture

- **One class library, `Memento`**, holding every example as nested namespaces — `Game`,
  `Gof`, `Hw`, `Gui`, `Pattern1` and `Pattern2`. Sources mirror namespaces:
  `src/Memento/Game/Solution/…`.
- **A console runner, `Memento.Demo`**, that runs the Java original's `main` methods — the
  game's three stages and the memento, GoF's constraint solver before and after the pattern,
  the three homework exercises, the GUI component and the two threaded examples — each on
  its own.
- `net10.0`, nullable reference types on, implicit usings on — set once in
  `Directory.Build.props` and inherited by both projects.
- **An xUnit project, `Memento.Tests`**, in `tests/Memento.Tests`. It is described in `Test.md`.

## Differences from the Java original

The port is faithful in behavior. The output of `game-setters`, `game-history`,
`game-copy`, `game`, `gof-problem`, `gof-solution`, `gof`, `hw-editor`, `hw-rollback`,
`hw-incremental` and `gui` is byte for byte the output of the Java `main` methods. These
outputs are the same under a US English, a Turkish and a Swedish locale. `pattern1` and `pattern2` run two threads each, so the order of their
lines changes from run to run in both languages. In the runs checked, the C# and the Java
printed the same number of lines, the same number of saves and state changes, and the same
undo lines on the error stream (`state-6` and `state-16` for `pattern1`, `state-6` for
`pattern2`). What had to change:

- **The mementos are an empty public interface and a private class**, as explained above.
  The Java memento types `Player.Checkpoint`, `ConstraintSolver.Memento`,
  `TextEditor.Snapshot`, `Account.Saved` and `Sheet.Change` appear to the caretaker as
  `Player.ICheckpoint`, `ConstraintSolver.IMemento`, `TextEditor.ISnapshot`,
  `Account.ISaved` and `Sheet.IChange`. The private classes keep the Java names.
- **Java records are C# records.** `Batch.Transfer` is `sealed record Transfer(Account From,
  Account To, int Amount)`. Stage two's private record `Saved` is a `private sealed record`.
- **Accessors became properties**: `Health`, `Position` and `Inventory` on stage one's
  player (with setters, as the Java has setters), `Name` and `X` on `Graphic`, `CheckpointCount`,
  `CellCount`, `Size`, and the getters and setters of `GuiComponent`, `GuiComponentState`
  and `GuiComponentMemento.State`. `Originator.State` in `Pattern1` is a get-only property;
  `SetState` stays a method because it also prints. `SetMemento`, `SetOriginator` and
  `Line()` stay methods, as in the Java.
- **`Pattern2`'s memento is a private class behind an empty public interface.** The Java
  memento is nested in `Originator` with a private field, which only `Originator` can read.
  A C# outer class cannot read a nested class's private members, so `Originator.IMemento` is
  an empty public interface for the caretaker, and the state is in a private nested class
  `Memento`, the same idiom as the other mementos in this port.
- **Lists are `List<string>`, returned as `IReadOnlyList<string>`**, each a copy, as Java's
  `List.copyOf` gives. The examples print lists as `[a, b]`, as Java's `List.toString()`
  does.
- **`Deque` is `Stack<T>`** in stage two's player, `Game` and `History`. They only `push`,
  `peek`, `pop` and `clear`. One difference: Java's `peek()` on an empty deque returns
  `null`, and `Stack.Peek()` throws `InvalidOperationException`. No example loads before it
  saves.
- **`Batch.Run` keeps the mementos in an `OrderedDictionary<Account, Account.ISaved>`**, where
  the Java uses a `LinkedHashMap`. Both keep the order of the accounts. `Run` takes an
  `IReadOnlyList<Account>`. `IllegalStateException` is `InvalidOperationException`.
- **`Sheet` reads the old value before it sets the new one.** Java's `HashMap.put` returns
  the old value; a C# `Dictionary` does not. `Sheet.Set` takes an
  `IReadOnlyDictionary<string, int>`. For a cell that does not exist, the Java puts the new
  cell and then throws `NullPointerException` in `Map.copyOf`; the C# throws
  `KeyNotFoundException` before it sets anything. No example uses a cell that does not
  exist. `Map.copyOf` is a new `Dictionary`, and `putAll` is a loop.
- **`Caretaker` and `OriginatorTrigger` hold a thread; they are not one.** In the Java they
  extend `Thread`. In C# `Thread` is `sealed`, so each creates a `Thread` in its constructor
  and `Start()` starts it; the thread calls `Run()`, as Java's `start()` calls `run()`. This
  is how the Mediator port's `Traffic.Car` does it.
- **The two threaded `Test.Run()` methods wait for their threads.** The Java `main` returns
  at once, and the JVM waits for the threads before it exits, so the output is the same. Here
  the runner must not start `pattern2` while `pattern1` is still printing, so `Run()` calls
  `Join()` on both threads. `Caretaker` and `OriginatorTrigger` have a `Join()` method for
  this.
- **`synchronized` methods lock a private object**, and `Thread.sleep` is `Thread.Sleep`.
  `InterruptedException` is `ThreadInterruptedException`, printed to the error stream as
  `printStackTrace()` does. One difference: an exception that ends a Java thread prints a
  stack trace and the other threads go on; in C# it ends the whole program. No example
  throws on a thread.
- **Undo messages go to `Console.Error`**, as the Java's go to `System.err`. In `Pattern1`
  and `Pattern2` the caretaker keeps its mementos in a `Stack<T>`; undo after a single save
  prints "Caretaker: Nothing to undo."
- **Number formats do not depend on the machine's locale.** Numbers that are printed are
  formatted with the invariant culture.
- **The `main` methods became `Run()` methods** called by `Memento.Demo`.
- **Names that clash, and how they are resolved.**
  - The root namespace is `dev.kaldiroglu.Memento`, and `Pattern1` has a class named
    `Memento`, as in the Java. Inside `dev.kaldiroglu.Memento.Pattern1` the plain name
    `Memento` means the class, because a type in the current namespace is found before an
    enclosing namespace. Nothing outside `Pattern1` names that class, so the clash never
    reaches other code. `ConstraintSolver.Memento` and `Pattern2.Originator.Memento` are
    private nested classes; inside their enclosing class the plain name means the nested
    class.
  - Four classes are named `Player`, in `Game.Problem.Setters`, `Game.Problem.History`,
    `Game.Problem.Copy` and `Game.Solution`, and three are named `Game`, in
    `Game.Problem.Setters`, `Game.Problem.Copy` and `Game.Solution`, as in the Java. The
    class `Game` also has the name of the namespace `Memento.Game`. Inside
    `Game.Solution` the plain name `Game` means the caretaker class, because a type in the
    current namespace is found first. `Game.Solution.Main` writes the stage classes as
    `Problem.Setters.Player` and `Problem.Copy.Game`: from inside `Game.Solution`, `Problem`
    is found as a sibling namespace. The Java writes their full package names.
  - `Gof.Main` writes `Problem.Graphic` and `Solution.Graphic` in the same way.
  - Inside `Game.Problem.Setters` and `Game.Problem.Copy`, the plain names `Player` and
    `Game` mean that stage's classes, so their `Main` classes write `new Game()` as the Java
    does.
  - Thirteen namespaces have a class named `Main` or `Test`, as in the Java. `Main` and
    `Test` are static classes with a `Run()` method, which C# allows. In `Memento.Demo` the
    runner reaches them through aliases: `GameSettersMain`, `GameHistoryMain`,
    `GameCopyMain`, `GameMain`, `GofProblemMain`, `GofSolutionMain`, `GofMain`,
    `EditorMain`, `RollbackMain`, `IncrementalMain`, `GuiTest`, `Pattern1Test` and
    `Pattern2Test`.
- **The `uml/` diagrams, the `CD1.png` and `SD1.png` images and the per-package `README.md`
  files in the Java packages are not ported yet.** This README carries their content.

## Tests

`tests/Memento.Tests` holds 27 xUnit tests in 9 test classes. They are ported from the
JUnit tests of the Java repository, one C# test class for each Java test class, with the
same expected values, except where `Test.md` notes a change. `Test.md` lists what each
class tests, and the Java tests that are not ported, with the reason.

## Run it with

The `dotnet` on `PATH` cannot build this repository — a tracked `global.json` at the root
pins the SDK to 10.0.0. Use the .NET 10 SDK directly:

```bash
cd ~/"Development/NET/Design Patterns/Design Patterns with CSharp/Memento"

# build everything
~/.dotnet/dotnet build Memento.sln

# every example, in the order the course presents them (pattern1 and pattern2 take about
# twenty seconds each)
~/.dotnet/dotnet run --project src/Memento.Demo

# one example on its own
~/.dotnet/dotnet run --project src/Memento.Demo -- game
```

The runner accepts: `game-setters`, `game-history`, `game-copy`, `game`, `gof-problem`,
`gof-solution`, `gof`, `hw-editor`, `hw-rollback`, `hw-incremental`, `gui`, `pattern1`,
`pattern2`.

Run the tests:

```bash
~/.dotnet/dotnet test Memento.sln
```

From the repository root, `~/.dotnet/dotnet build "Design Patterns with CSharp.sln"`
builds Memento along with every other pattern.
