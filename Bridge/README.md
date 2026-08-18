# Bridge — Design Patterns with C#

*Claude Opus 5 (claude-opus-5) — Created on 2026-08-19*

For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev

The C# port of the Bridge material from **Design Patterns with Java**
(`kaldiroglu/DesignPatternsWithJava`, package root
`dev.kaldiroglu.dp.structural.bridge`). Every example the Java repository carries is
here, in the same shape and with the same numbers, under the root namespace
`dev.kaldiroglu.Bridge`.

## What Bridge is for

Two things vary for reasons that have nothing to do with each other. A notification is
urgent or a digest; it goes out by email, SMS or push. Nobody decides the second by
deciding the first.

Bind them with inheritance and the class count is their product: three kinds and three
channels is nine classes, and a fourth channel is three more. Put one behind a reference
the other holds, and the count is their sum: six classes, and a fourth channel is one.

The reference is the whole pattern. Because it is a field rather than a base class, the
implementation can also be chosen — and changed — while the program runs, which is what
a preference stored in a database requires and what a base class can never give.

## The examples

| Namespace | What it shows |
|---|---|
| `Bridge.Violation` | Why not inheritance. `ASubType` overrides a method that promised to print, and stores instead. Callers holding `AType` get silence and no exception. |
| `Bridge.Basic` | The pattern with the domain removed. `Problem` needs six classes for four combinations; `Pattern` needs four. |
| `Bridge.Notifications` | An order system. Three naive designs, then the bridge, then GoF's implementation issues 2 and 3 as working variations. |
| `Bridge.Shape` | Shapes over two window systems. MacOS draws an arc in one call; XWindows has no arc and builds it from sixteen lines. |
| `Bridge.Gof` | GoF's own `Window`/`WindowImp` (Design Patterns, pp. 151–157), drawn onto a character grid so both designs can be compared on identical output. |
| `Bridge.Files` | A client's document store: two departments with opposite retention rules, over Evernote, SharePoint and FileNet. |
| `Bridge.Retrofit` | A regulation arrives over a system that already works. The required interface sits on top; the engine stays behind a reference and is never recompiled. |
| `Bridge.Hw` | The three homework exercises: a statement run, a payment desk and a map switch. |

Each of the first six is split into a `Problem` (or naive) side and a `Solution` (or
`Pattern`) side. Both sides work. The difference is what the next change costs, and the
tests are written to measure exactly that.

### Three things worth stopping on

**The implementor carries primitives, not the abstraction's operations.** `IShapeDrawer`
has `DrawLine`, `DrawArc` and `Clear` — never `DrawTriangle`. `IWindowImp` has
`DeviceRect` and `DeviceText` — never `DrawIcon`. If the two interfaces mirror each
other, they are one interface written twice and there is no bridge left.

**The device may not be able to do what it was asked.** `PMWindowImp` has no rectangle
call, so it builds one from a polyline: seven journal entries against X's three, for the
same picture. `XWindowsDrawer` has no arc, so a circle becomes sixteen line segments.
The abstraction never finds out, and could not act on it if it did.

**The naive designs fail quietly.** `SwitchingNotifier` states the 160-character SMS rule
in two of its three SMS branches; the digest branch throws in production.
`SwitchingFileManager` applies a retention rule in five of its six branches; insurance
keeps eight versions on FileNet where it is allowed two, and nothing throws at all.

## The diagrams

20 PlantUML diagrams live in a `uml/` folder beside the code each one describes —
`src/Bridge/Retrofit/uml/`, `src/Bridge/Notifications/Solution/uml/`, and so on. That is
the layout the Java repository uses. Each is checked in as `.puml`, `.png` and `.svg`.

Seventeen are class diagrams: one per example side, plus GoF's pattern structure from
p. 153. The notifications example carries three more — an object diagram showing the two
axes meeting in one reference, a sequence diagram of a single urgent SMS, and a class
diagram of the two variations. The window example carries an object diagram too, showing
one `IconWindow` class producing two different pictures.

The problem-side diagrams are worth as much as the solution-side ones, because the point
of the material is what the naive designs cost rather than that they fail. The Java
repository has no diagram for the naive document store; this port adds one, since
`Bridge.Files.Problem` is a worked example here.

Names are checked, not remembered: every class, interface and member named in a diagram
was verified against the sources. The numbers in the notes are the ones the tests assert.

Nothing in the build depends on them — the `.csproj` globs `*.cs` only, so `.puml`,
`.png` and `.svg` are inert as far as MSBuild is concerned.

## Architecture

- **One class library, `Bridge`**, holding all eight example groups as nested namespaces.
  Sources mirror namespaces: `src/Bridge/Notifications/Solution/Classic/…`.
- **A console runner, `Bridge.Demo`**, that runs each example on its own, so none of them
  needs a line commented out to be seen alone.
- **An xUnit project, `Bridge.Tests`**, described in `Test.md`.
- `net10.0`, nullable reference types on, implicit usings on — set once in
  `Directory.Build.props` and inherited by all three projects.
- The vendor SDKs every example ends at (`Transports`, `VendorStores`,
  `InMemoryProvider`) are simulated in memory, so the examples run anywhere and a test
  can assert what was actually sent or kept rather than describing it.

## Differences from the Java original

The port is faithful in behavior and in every number. Four things had to change:

- **Interfaces take the `I` prefix**, as the rest of this solution does: `IShapeDrawer`,
  `IWindowImp`, `INotificationChannel`, `IFileProvider`, `IVendorClient`.
- **`Notifications.Domain.Console` became `Scenario`.** `Console` is taken in .NET, and
  `Demo` is taken by this repository's own `dev.kaldiroglu.Bridge.Demo` namespace.
- **`UrgentNotification::new` became `c => new UrgentNotification(c)`.** C# has no
  constructor method group, so `NotificationService.Send` takes a
  `Func<INotificationChannel, Notification>`.
- **`virtual` is explicit.** In `Bridge.Violation` and `Bridge.Basic.Problem` the base
  methods must opt in to being overridden. That is one more deliberate step than Java
  asks for, and no protection at all — which is the point the violation makes.

## Run it with

The machine's `dotnet` on `PATH` is 9.0.x and cannot build `net10.0`. Use the .NET 10 SDK
directly:

```bash
cd "~/Development/NET/Design Patterns/Design Patterns with CSharp/Bridge"

# build everything
~/.dotnet/dotnet build

# the tests — 96 of them
~/.dotnet/dotnet test tests/Bridge.Tests

# every example, in the order the course presents them
~/.dotnet/dotnet run --project src/Bridge.Demo

# one example on its own
~/.dotnet/dotnet run --project src/Bridge.Demo -- violation

# re-render every UML diagram to PNG and SVG (needs `brew install plantuml`)
./render-uml.sh
```

The runner accepts: `violation`, `basic`, `notifications-problem`, `notifications`,
`notifications-factory`, `notifications-shared`, `shapes`, `window`, `files-problem`,
`files`, `retrofit`.

From the repository root, `~/.dotnet/dotnet build "Design Patterns with CSharp.sln"`
builds Bridge along with every other pattern.
