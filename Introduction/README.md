<!-- Model: Claude Opus 5.5 (claude-opus-5-5) — Created: 2026-10-29 -->

# Introduction — C# (.NET)

*For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev*

**Project:** Design Patterns course — the Introduction chapter (C# implementation)
**Created:** 2026-10-29
**Source:** a port of the Java package `dev.kaldiroglu.dp.introduction` in
`DesignPatternsWithJava`. The Introduction deck quotes the Java; this port runs the same
examples with the same figures.

## Expected benefit

The Introduction deck shows one recurring problem and previews one pattern before the
pattern decks start. Both examples run here as well, so a C# audience can run what the
slides show.

## The three examples

| Namespace | What it shows |
|---|---|
| `Problems.Users` | `UserProcessor.ProcessUser`: one method with 7 parameters, `if` statements nested 15 levels deep, and 43 different results. Every rule about a user is in this one method, so its cohesion is as low as it gets. `Main` runs three users through it. |
| `Problems.Pricing` | `SwitchingCheckout.Ring`: one switch with a branch for each of five campaigns. It is a copy of the first stage of the `Strategy` folder's `Pricing.Problem`, so this project runs on its own. `StageOneMain` prices one basket under four campaigns, then shows that a typo in a campaign name fails only at run time. |
| `Proxy.Pm.Pm1`–`Pm3` | Citizens who call the Prime Minister, in three stages. Pm1: the PM screens her own calls. Pm2: a `Proxy` class screens, but it shares no type with `PM`, so every `Citizen` had to change. Pm3: `IPM`, `RealPM`, `ProxyPM` and a `PMSecretary`, which is the Proxy pattern. |

The Prime Minister example is also in the `Proxy` folder of this repository, in an earlier
form without the counters. This copy follows the version the Introduction deck runs.

## Architecture

One class library (`src/Introduction`), one console runner (`src/Introduction.Demo`) and
one xUnit project (`tests/Introduction.Tests`). Folders mirror namespaces under the root
namespace `dev.kaldiroglu.Introduction`.

## Notes on the port

- `User` has no setters, as in Java. Its properties are `virtual`, so `Main.AdultUser`
  overrides `Age`; Java uses an anonymous subclass for the same purpose.
- `UserProcessor.cs` was converted from the Java source by a script: getters became
  properties, `equals` became `==`, and braces moved to their own lines. The nesting and the
  43 results are unchanged, and the tests count them in this file.
- The namespace is `Problems.Users`, plural, where the Java package is `problems.user`. A
  class `User` in a namespace `User` cannot be named by its short name from outside that
  namespace, because the namespace is found first.
- The Pm3 interface is `IPM`, following .NET naming; Java calls it `PM`.

## Run it with

```bash
cd "~/Development/NET/Design Patterns/Design Patterns with CSharp/Introduction"

~/.dotnet/dotnet test tests/Introduction.Tests
~/.dotnet/dotnet run --project src/Introduction.Demo                 # every example
~/.dotnet/dotnet run --project src/Introduction.Demo -- user   # or one: pricing, pm1, pm2, pm3
```
