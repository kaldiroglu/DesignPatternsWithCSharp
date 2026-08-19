# Decorator — Design Patterns with C#

*Claude Opus 5 (claude-opus-5) — Created on 2026-08-19*

For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev

The C# port of the Decorator material from **Design Patterns with Java**
(`kaldiroglu/DesignPatternsWithJava`, package root
`dev.kaldiroglu.dp.structural.decorator`). Every example that repository carries is here,
in the same shape and with the same numbers, under the root namespace
`dev.kaldiroglu.Decorator`.

## What Decorator is for

Responsibilities that can be combined, in any order, in combinations nobody enumerated in
advance. Logging, retries, caching and a rate limit around one method. Cheese, sausage and
tomato on one piece of bread.

Write a class per combination and the count is the power set: five toppings is
2⁵ − 1 = 31 classes, and the thirty-second order the shop takes is one nobody wrote. Give
each responsibility a class that *is a* component and *has a* component, and five classes
price every combination — including cheese twice, which no class-per-combination menu can
express at all.

What makes it a Decorator rather than a Bridge: the wrapper implements the interface it
wraps, so a decorated object is still a component and can be decorated again. The
recursion is the pattern.

## The examples

| Namespace | What it shows |
|---|---|
| `Decorator.Toast` | An Ayvalık toast shop. `Problem` welds a price into each combination; `Solution` gives each topping a class and prices every one of them. |
| `Decorator.Middleware` | A price feed with logging, retries, caching and a rate limit. Three naive designs, five classic decorators, and two variations. |
| `Decorator.Gof.Visual` | GoF's own example (Design Patterns, pp. 175–181): borders and scrollbars around a `TextView`, plus the *skin versus guts* variation. |
| `Decorator.Gof.Stream` | GoF's second example (pp. 182–184): compression and 7-bit encoding over a file or a socket. Two axes, added instead of multiplied. |
| `Decorator.Io` | The pattern in the standard library, measured: `BinaryWriter` over `BufferedStream` over a file, then a `GZipStream` added as one more layer. |
| `Decorator.Hw` | The three homework exercises: a fee engine, an invoice pipeline and combat power-ups. |

### Four things worth stopping on

**Order is part of the meaning, and the tests measure it.** A voucher is a subtraction, so
where value-added tax sits relative to it changes the tax charged; a percentage discount is
a multiplication, so its position changes nothing. Encrypted bytes do not compress, so
compressing outside encryption produces a larger file than compressing inside. A checksum
has to cover the bytes that actually travel, so it belongs outermost. None of these is a
matter of taste.

**A decorator need not forward exactly once.** `CachingPriceFeed` forwards zero times on a
hit; `RetryingPriceFeed` forwards several. `Berserk` never asks what is beneath it at all —
a fighter under double damage and poison still hits for the berserk figure — and it is a
decorator all the same.

**GoF's third consequence, stated plainly.** A decorated feed is not the same object as the
feed. Anything that compares by reference identity, or reaches for a concrete type, breaks
— and a test asserts exactly that rather than describing it.

**Decoration works where subclassing cannot.** `VendorPriceFeed` is `sealed`. No subclass
of it can ever exist, and the decorators wrap it unchanged — the test stacks caching over
logging over it and counts one supplier call against two log lines.

## Architecture

- **One class library, `Decorator`**, holding every example as nested namespaces —
  `Gof`, `Hw`, `Io`, `Middleware` and `Toast`.
  Sources mirror namespaces: `src/Decorator/Middleware/Solution/Classic/…`.
- **A console runner, `Decorator.Demo`**, that runs each example on its own, so none of
  them needs a line commented out to be seen alone.
- **An xUnit project, `Decorator.Tests`**, described in `Test.md`.
- `net10.0`, nullable reference types on, implicit usings on — set once in
  `Directory.Build.props` and inherited by all three projects.
- The infrastructure the examples end at (`SimulatedRemotePriceFeed`, `ManualClock`,
  `CallLog`, `Codecs`, `TextLayout`) is simulated in memory and driven by a manual clock,
  so nothing is timing-dependent and a test can assert how many times the supplier was
  actually called.

## Differences from the Java original

The port is faithful in behavior and in every number. What had to change:

- **Interfaces take the `I` prefix**, as the rest of this solution does: `IToastable`,
  `IPriceFeed`, `IVisualComponent`, `ICharge`, `IPipeline`, `ICombatant`, `IBorderStyle`.
- **`java.io` became `System.IO`.** `DataOutputStream` over `BufferedOutputStream` over
  `FileOutputStream` is `BinaryWriter` over `BufferedStream` over a file, and
  `GZIPOutputStream` is `GZipStream`. The shapes are the same; only the names moved.
- **`final` became `sealed`, and the point survives intact.** Java's `VendorPriceFeed` is
  `final` and its test is named "decoration works on a final class, where subclassing
  cannot"; the C# one is `sealed` and says the same. This is a rename, not a new argument.
- **The functional variation is a named delegate.** Java's `PriceFeedMiddleware extends
  UnaryOperator<PriceFeed>`; C#'s is `delegate IPriceFeed PriceFeedMiddleware(IPriceFeed
  next)`, which says the same thing without the generic. A one-off concern still needs no
  class of its own — `Middleware.Lambda` turns a `Func<string, Quote>` into a feed.

## Run it with

The `dotnet` on `PATH` cannot build this repository — a tracked `global.json` at the root
pins the SDK to 10.0.0, and the error it reports is the misleading "The command could not
be loaded". Use the .NET 10 SDK directly:

```bash
cd "~/Development/NET/Design Patterns/Design Patterns with CSharp/Decorator"

# build everything
~/.dotnet/dotnet build

# the tests — 50 of them
~/.dotnet/dotnet test tests/Decorator.Tests

# every example, in the order the course presents them
~/.dotnet/dotnet run --project src/Decorator.Demo

# one example on its own
~/.dotnet/dotnet run --project src/Decorator.Demo -- classic
```

The runner accepts: `copypaste`, `flags`, `subclasses`, `classic`, `ordering`,
`ratelimit`, `vendor`, `functional`, `fluent`.

From the repository root, `~/.dotnet/dotnet build "Design Patterns with CSharp.sln"`
builds Decorator along with every other pattern.
