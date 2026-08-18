# Tests — Bridge

*Claude Opus 5 (claude-opus-5) — Created on 2026-08-19*

For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev

96 xUnit tests in one project, `tests/Bridge.Tests`. All of them are **unit tests**: no
process boundary, no filesystem, no network. The vendor SDKs the examples end at are
simulated in memory, so what would be integration surface elsewhere is ordinary object
state here — and a test can assert what was actually sent, drawn or kept.

## What the tests are for

They are not regression cover for a library. They are the instrument the teaching numbers
are read off. Every figure that appears on a slide or in `README.md` is asserted by one of
these, and asserted **by counting the code** rather than by restating the number.

That distinction is the whole design of the suite. Writing

```csharp
var kinds = 3; var channels = 3;
Assert.Equal(9, kinds * channels);
```

proves something about integers. It goes on passing the day a fourth channel is added and
the slide still says nine. So the suite counts instead:

```csharp
var kinds    = TypeCensus.ConcreteImplementationsOf(ClassicNs, typeof(Notification));
var channels = TypeCensus.ConcreteImplementationsOf(ClassicNs, typeof(INotificationChannel));
Assert.Equal(9, kinds * channels);
```

Add a fourth channel and this fails, which is what a number on a slide is entitled to.

`TypeCensus` (in the test root) does three jobs: enumerate the types declared in a
namespace, count the concrete implementations of a root type within one, and read a
declared field back by name — the bridge reference, in every example.

Two tests go further and count the **source text**, because the fault they describe is a
line that is missing rather than a type that exists:

- `ProblemTests.BranchesMultiply` — nine `case` leaves in `SwitchingNotifier`, three of
  them SMS branches, and `Transports.SmsLimit` named in two of the three.
- `FileProblemTests.TheRulesLeak` — six `case` leaves in `SwitchingFileManager`, and a
  retention loop in five of them.

They locate the sources through `TypeCensus.SourceRoot`, anchored with `CallerFilePath`
at compile time, so the tests do not depend on the working directory.

## Layout

| File | Tests | What it holds |
|---|---|---|
| `Violation/ViolationTests.cs` | 6 | The silent LSP breach, caught by capturing `Console.Out`. |
| `Basic/BasicBridgeTests.cs` | 5 | m + n against m x n with the domain removed. |
| `Shape/ShapeBridgeTests.cs` | 7 | One circle, two devices, 1 call against 16. No primitive names a shape. |
| `Gof/WindowProblemTests.cs` | 5 | The platform as a base class, and the X drawing code written six times over. |
| `Gof/WindowSolutionTests.cs` | 6 | The same pictures, a run-time swap, and a third platform written inside the test. |
| `Gof/DesignComparisonTests.cs` | 3 | Both designs draw byte-identical output; nine types against six. |
| `Notifications/ProblemTests.cs` | 9 | The three naive designs, and the forgotten SMS rule. |
| `Notifications/ClassicBridgeTests.cs` | 10 | 3 x 3 combinations, the retry written once, the channel's limits asked for. |
| `Notifications/DesignComparisonTests.cs` | 2 | All designs agree on the wire; a fourth channel is one class. |
| `Notifications/VariationsTests.cs` | 6 | GoF implementation issues 2 and 3: a factory chooses, and one implementor is shared. |
| `Files/FileProblemTests.cs` | 7 | The retention breach nothing throws on. |
| `Files/FileBridgeTests.cs` | 7 | One rule over three stores, and a live manager moved between them. |
| `Retrofit/RetrofitTests.cs` | 6 | The legacy caller undisturbed, and the standard satisfied over it. |
| `Hw/StatementRunTests.cs` | 6 | The screen reader, and why no primitive is about paper. |
| `Hw/PaymentDeskTests.cs` | 6 | Cash collapsing the two phases, and no capability booleans. |
| `Hw/RoutePlannerTests.cs` | 5 | Two maps, different answers, and no vendor type in the abstraction. |

## Two things the port had to handle

**Parallelism is off, deliberately.** `AssemblyInfo.cs` carries
`[assembly: CollectionBehavior(DisableTestParallelization = true)]`. The examples print —
the shape drawers and the window implementors both write every call to the console — and
`ViolationTests` has to capture `Console.Out` to prove that `ASubType` prints *nothing*,
which is the only way that violation is visible at all. Under xUnit's default those two
facts collide: a capture opened by one class swallows another class's output. The suite
runs in about 45 ms, so there is nothing to win by running it at once.

**`Type.IsAbstract` is true for interfaces in .NET.** Anywhere the suite counts "the
abstract kinds" it has to write `t.IsAbstract && !t.IsInterface`, or `IShape` inflates the
count by one. The Java original has no equivalent trap.

## Run it with

```bash
cd "~/Development/NET/Design Patterns/Design Patterns with CSharp/Bridge"

# all 96
~/.dotnet/dotnet test tests/Bridge.Tests

# one file's worth
~/.dotnet/dotnet test tests/Bridge.Tests --filter "FullyQualifiedName~RetrofitTests"

# one test
~/.dotnet/dotnet test tests/Bridge.Tests --filter "FullyQualifiedName~TheForgottenRule"

# with each test's name printed
~/.dotnet/dotnet test tests/Bridge.Tests -v normal
```

`dotnet` on `PATH` is 9.0.x and cannot build `net10.0`; `~/.dotnet/dotnet` is 10.0.102.
