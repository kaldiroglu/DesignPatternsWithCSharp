# Tests — Decorator

*Claude Opus 5 (claude-opus-5) — Created on 2026-08-19*

For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev

50 xUnit tests in one project, `tests/Decorator.Tests`. All of them are **unit tests**: no
network and no wall clock. One touches the filesystem — `InvoiceStreamTests` goes through
`InvoiceStreamDemo`, which writes to two `Path.GetTempFileName()` paths and deletes both in
a `finally`. The remote price feed is simulated in memory and time is a `ManualClock`, so
nothing is timing-dependent and every run produces the same numbers.

## What the tests are for

They are the instrument the teaching claims are read off. A decorator chain's behavior is
mostly invisible — the point of the pattern is that the component cannot tell it has been
wrapped — so the only honest way to teach it is to measure what actually happened:

- **How many times the supplier was called.** Caching forwards zero times on a hit;
  retrying forwards several; logging forwards once and writes two lines, or four, depending
  on whether it sits inside or outside the retry.
- **What the bytes came to.** Encrypted bytes do not compress, so one ordering produces a
  larger file than the other, and the test asserts both sizes rather than describing the
  effect.
- **What the picture looks like.** The visual tests compare rendered character grids
  against the strings the Java port produces, character for character.

Where a number appears in `README.md` or on a slide, a test asserts it. `2⁵ − 1 = 31` is
checked two ways — as a shift and as the sum of the binomial coefficients — so the figure
cannot drift into being a plausible-looking guess.

## Layout

| File | Tests | What it holds |
|---|---|---|
| `Toast/ToastTests.cs` | 7 | The Ayvalık toast shop: welded prices against five topping classes, the same topping twice, and a multiplying decorator whose position changes the bill. |
| `Middleware/ClassicDecoratorTests.cs` | 9 | Each decorator on its own, ordering, decoration over a `sealed` class, and GoF's third consequence. |
| `Middleware/VariationsTests.cs` | 3 | Classic, fluent and functional assembly produce identical behavior and identical numbers. |
| `Gof/Visual/DesignComparisonTests.cs` | 5 | Subclassing and decorating draw the same picture; the difference is what a third embellishment costs. |
| `Gof/Visual/SkinAndGutsTests.cs` | 5 | A style is a separate hierarchy, so a fourth style leaves the decorator untouched. |
| `Gof/Stream/StreamTests.cs` | 5 | Transformations against destinations, added instead of multiplied, and `Close` reaching all the way down. |
| `Hw/FeeEngineTests.cs` | 4 | Subtraction versus multiplication, and why one of them makes position matter. |
| `Hw/InvoicePipelineTests.cs` | 5 | Round-trips, ordering against compressibility, and a wrong chain refused rather than silently wrong. |
| `Hw/PowerUpsTests.cs` | 6 | Stacking, expiry, revocation, and a decorator that forwards zero times. |
| `Io/InvoiceStreamTests.cs` | 1 | One more decorator, a smaller file, and the same total read back. |

## Two things worth knowing before changing them

**Time is injected, never read.** Nothing calls `DateTime.Now`; the decorators that care
about time take an `IClock`, and the tests hand them a `ManualClock`. It moves only when
`SimulatedRemotePriceFeed` charges a call its latency — no test advances it by hand.

That leaves a gap worth knowing about rather than papering over: **expiry is not tested.**
The cache TTL is 60 seconds and the rate-limit window is an hour, both far longer than the
few simulated milliseconds a test elapses, so the cache never evicts and the window never
resets. What is asserted is the hit (`forwards zero times`) and the refusal (`once the
quota is spent`). A test for eviction would be one `_clock.Advance(...)` away, and the
Java port does not have one either.

**Failures are scripted, not random.** `SimulatedRemotePriceFeed.FailNext(n)` queues
outages that the next calls will hit, so "retrying forwards three times and then succeeds"
is a fact about the test rather than a hope about the network.

## Run it with

```bash
cd "~/Development/NET/Design Patterns/Design Patterns with CSharp/Decorator"

# all 50
~/.dotnet/dotnet test tests/Decorator.Tests

# one file's worth
~/.dotnet/dotnet test tests/Decorator.Tests --filter "FullyQualifiedName~ToastTests"

# one test
~/.dotnet/dotnet test tests/Decorator.Tests --filter "FullyQualifiedName~TheSameToppingTwice"

# with each test's name printed
~/.dotnet/dotnet test tests/Decorator.Tests -v normal
```

The `dotnet` on `PATH` will not do: a tracked `global.json` at the repository root pins the
SDK to 10.0.0, and the PATH SDK reports that as "The command could not be loaded".
