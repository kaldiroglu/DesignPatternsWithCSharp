# Mediator — Design Patterns with C#

*Claude Opus 5.5 (claude-opus-5-5) — Created on 2026-10-09*

For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev

The C# port of the Mediator material from **Design Patterns with Java**
(`kaldiroglu/DesignPatternsWithJava`, package root `dev.kaldiroglu.dp.behavioral.mediator`).
Every class that repository carries is here, in the same shape, under the root namespace
`dev.kaldiroglu.Mediator`: 41 Java files and 41 C# files in the library, one for one, and
the runner `Mediator.Demo`.

## What Mediator is for

A group of objects talk to each other through one object, the mediator, and never directly.
Each object knows the mediator only. The rules about who talks to whom, and what follows
from a change, are in the mediator, in one place. A new kind of object only has to talk to
the mediator; the other objects do not change.

The cost is that the mediator holds every rule, so it can grow large. It is also the one
object that every other object shares, so when they run on several threads, the threads
meet in the mediator.

## The examples

| Namespace | What it shows |
|---|---|
| `Mediator.Chat` | The main worked example: a team chat with messages to everyone and private messages. The promise: a private message is seen only by the person it is sent to. `Problem.Direct` is stage one: every member holds every other member (4 members, 12 references). `Problem.Directory` is stage two: one shared `Directory`, but every sender applies the rules itself. `Problem.Bus` is stage three: a `MessageBus` delivers every message to every client, and each client decides what to show. `Solution` has the mediator `ChatRoom`, the colleague `IParticipant`, the concrete colleagues `Member` and `Guest`, and one runnable class per slide: `ChatRoomMain` runs the pattern on its own, `ReferencesMain` counts the references of stage one, and `TwoWaysMain` sends the same private message through the bus and through the room. Each stage also has a `Main` of its own. |
| `Mediator.Gof` | GoF's own example (Design Patterns, pp. 273–282): a font dialog. `Problem.FontDialog` has widgets that call each other. `Solution` has the mediator `DialogDirector`, the concrete mediator `FontDialogDirector`, the colleague `Widget`, and the general widgets `ListBox`, `EntryField` and `Button`. `Problem.Main` and `Solution.Main` run the same steps, one design each. |
| `Mediator.Hw` | The three homework exercises: a bank branch where a `QueueManager` pairs customers with tellers (`BankQueue`), an airport where a `ControlTower` gives the runway to one aircraft at a time (`AirTraffic`), and a meeting-room `BookingForm` that is the mediator of its own fields (`BookingForm`). |
| `Mediator.Traffic` | Cars at a junction ask a traffic police officer for permission to pass. Each car runs on its own thread, asks once, and blocks until the officer tells it to proceed. `Main` creates five cars. |

### Things worth stopping on

**Where stage three fails.** Elif sends Mert a private message. The bus delivers it to
Burak, Mert and Can. The team's own clients hide it unless it is for them, but the guest
client, written later by another team, shows it: Can reads Elif's private message to Mert.
In the chat room, the same message is delivered to Mert only. The guest checks nothing, and
still shows nothing it should not, because it receives only what the room sends.

**The widgets are general.** In `Gof.Solution`, `ListBox`, `EntryField` and `Button` know
no other widget. The whole behavior of the dialog is in `FontDialogDirector.WidgetChanged`.

**A shared mediator is where threads meet.** `Hw.AirTraffic.ControlTower` locks every
public method, so the check "is the runway free?" and the step "give it to this aircraft"
happen together. `Traffic.TrafficPolice` does the same for the junction: it decides inside one lock, keeps the
waiting cars in a queue, and calls the cars outside the lock.

## Architecture

- **One class library, `Mediator`**, holding every example as nested namespaces — `Chat`,
  `Gof`, `Hw` and `Traffic`. Sources mirror namespaces: `src/Mediator/Chat/Solution/…`.
- **A console runner, `Mediator.Demo`**, that runs the Java original's `main` methods — the
  chat's three stages and the room, GoF's font dialog before and after the pattern, the
  three homework exercises and the traffic junction — each on its own.
- `net10.0`, nullable reference types on, implicit usings on — set once in
  `Directory.Build.props` and inherited by both projects.
- **An xUnit project, `Mediator.Tests`**, in `tests/Mediator.Tests`. It is described in `Test.md`.

## Differences from the Java original

The port is faithful in behavior. The output of `chat-problem-direct`,
`chat-problem-directory`, `chat-problem-bus`, `chat-room`, `chat-references`, `chat-two-ways`,
`gof-problem`, `gof-solution`, `hw-bankqueue`, `hw-airtraffic` and `hw-bookingform` is byte
for byte the output of the Java `main` methods. These outputs
are the same under a Turkish and a Swedish locale.
`traffic` runs on five threads, so the order of its lines changes from run to run in both
languages; its first 3 lines (the junction, the officer and a blank line) are the same as
the Java's, and each car asks once and passes once in both languages, which `MainOutput` checks. What had to change:

- **Interfaces take the `I` prefix**: `IParticipant`, `IClient`, `IVehicle` and
  `ITrafficMediator`.
- **The Java record is a `sealed record`** with the Java accessor names as properties:
  `Message(string From, string? To, string Text)`. Java's method `isPrivate()` is the property
  `IsPrivate`. No example prints a record, so its `ToString()` is left as C# writes it.
- **Accessors became properties**: `Name`, `Inbox`, `Shown`, `Deliveries`, `References`,
  `Members`, `Log`, `Number`, `CallSign`, `Intent`, `BookEnabled`, `Warning`, `Booked`,
  `Text`, `Selection` and `Enabled`. A setter the Java keeps package-private is `internal set`
  (`Customer.Number`, and `Enabled` on the button in `Gof.Problem.FontDialog`). Methods that
  do more than set a value stay methods: `SetText`, `Type`, `SetAttendees`, `ChooseRoom`.
  `Junction.IsBusy()` and `SetBusy()` stay methods, as in the Java.
- **Public fields became get-only properties.** `FontDialog` and `FontDialogDirector` have
  public `final` fields in the Java (`log`, `fontList`, `fontName`, `ok`, `cancel`). Here
  they are `Log`, `FontList`, `FontName`, `Ok` and `Cancel`. `Log` is the list itself, as
  in the Java, not a copy.
- **Lists are returned as `IReadOnlyList<T>`**, each a copy, as Java's `List.copyOf` gives.
  The examples print lists as `[a, b, c]`, as Java's `List.toString()` does, and booleans as
  `true` and `false`, not C#'s `True` and `False`.
- **`Optional<Member>` is `Member?`.** `Directory.Find` returns `null` when there is no
  member with that name, and `Member.Whisper` checks for it, where the Java uses
  `filter(...).ifPresent(...)`.
- **`ChatRoom` keeps participants in an `OrderedDictionary<string, IParticipant>`**, where
  the Java uses a `LinkedHashMap`. Both keep the order in which participants joined; a plain
  `Dictionary` does not promise any order. The blocks are a `Dictionary<string,
  HashSet<string>>`, filled the way Java's `computeIfAbsent` fills them.
- **`Deque` is `Queue<T>`** in `QueueManager` and `ControlTower`. They only add at the back
  and take from the front.
- **`ControlTower` locks a private object** where the Java methods are `synchronized`. A
  private object means no code outside the class can take the same lock. `Grant` is called
  only with the lock held, as in the Java. `IllegalStateException` is
  `InvalidOperationException`.
- **`BookingForm`'s capacities are a `static readonly IReadOnlyDictionary<string, int>`**,
  where the Java uses `Map.of`. `getOrDefault` is `GetValueOrDefault`.
- **A car's `CountDownLatch` is a `ManualResetEventSlim`.** `countDown()` is `Set()` and
  `await()` is `Wait()`. `Run()` is a method that `Main` passes to a `Thread`, where the Java
  `Car` is a `Runnable`.
- **Number formats do not depend on the machine's locale.** Numbers that are printed are
  formatted with the invariant culture.
- **The `main` methods became `Run()` methods** called by `Mediator.Demo`. `traffic` runs last when every example runs.
- **Names that clash, and how they are resolved.**
  - Three classes are named `Member`, in `Chat.Problem.Direct`, `Chat.Problem.Directory` and
    `Chat.Solution`, as in the Java. Inside each namespace the plain name means that
    namespace's class. `Chat.Solution.ReferencesMain` writes stage one's member as
    `Problem.Direct.Member`: from inside `Chat.Solution`, `Problem` is found as a sibling
    namespace. The Java imports stage one's `Member` and writes the room's `Member` with its
    full package name; the port does the opposite, because `ReferencesMain` is in the room's
    namespace.
  - The namespace `Chat.Problem.Directory` holds the class `Directory`. Implicit usings bring
    in `System.IO`, which also has a `Directory`. Inside the namespace the plain name means
    the example's class, because a type in the current namespace is found before a type that a
    `using` brings in.
  - The namespace `Hw.BookingForm` holds the class `BookingForm`. Inside the namespace the
    plain name means the class, so `Hw.BookingForm.Main` writes `new BookingForm()`.
  - `Chat.Problem.Bus.Message` and the two `Button` classes (`Gof.Solution.Button`, and
    `Button` nested in `Gof.Problem.FontDialog`) do not meet any type that implicit usings
    bring in. The library uses no UI framework.
  - The root namespace is `dev.kaldiroglu.Mediator`, and no type is named `Mediator`, so the
    name always means the namespace.
  - Nine namespaces have a class named `Main`, as in the Java. `Main` is a
    static class with a `Run()` method, which C# allows. In `Mediator.Demo` the runner
    reaches them through aliases: `ChatDirectMain`, `ChatDirectoryMain`, `ChatBusMain`,
    `GofProblemMain`, `GofSolutionMain`, `BankQueueMain`, `AirTrafficMain`,
    `BookingFormMain` and `TrafficMain`; `ChatReferencesMain` and `ChatTwoWaysMain` name the
    two runnable classes of `Chat.Solution`. `Gof.Problem.Main` and `Gof.Solution.Main` have
    the same short name, so a test that uses both writes them in full.
- **The `uml/` diagrams, the `CD.png` image and the per-package `README.md` files in the Java
  packages are not ported yet.** This README carries their content.

## Tests

`tests/Mediator.Tests` holds 38 xUnit tests in 9 test classes. They are ported from the
JUnit tests of the Java repository, one C# test class for each Java test class, with the
same expected values, except where `Test.md` notes a change. `Test.md` lists what each
class tests, and the Java tests that are not ported, with the reason.

## Run it with

The `dotnet` on `PATH` cannot build this repository — a tracked `global.json` at the root
pins the SDK to 10.0.0. Use the .NET 10 SDK directly:

```bash
cd ~/"Development/NET/Design Patterns/Design Patterns with CSharp/Mediator"

# build everything
~/.dotnet/dotnet build Mediator.sln

# every example, in the order the course presents them
~/.dotnet/dotnet run --project src/Mediator.Demo

# one example on its own
~/.dotnet/dotnet run --project src/Mediator.Demo -- chat-two-ways
```

The runner accepts: `chat-problem-direct`, `chat-problem-directory`, `chat-problem-bus`,
`chat-room`, `chat-references`, `chat-two-ways`, `gof-problem`, `gof-solution`, `hw-bankqueue`,
`hw-airtraffic`, `hw-bookingform`, `traffic`.

Run the tests:

```bash
~/.dotnet/dotnet test Mediator.sln
```

From the repository root, `~/.dotnet/dotnet build "Design Patterns with CSharp.sln"`
builds Mediator along with every other pattern.
