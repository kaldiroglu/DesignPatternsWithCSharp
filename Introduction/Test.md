*Claude Opus 5.5 (claude-opus-5-5) — Created on 2026-10-29*

# Tests — Introduction

For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev

15 xUnit tests in one project, `tests/Introduction.Tests`. All of them are **unit tests**:
no process boundary, no network. Three of them read a source file
(`UserProcessor.cs` or `SwitchingCheckout.cs`) to count what the deck quotes.

The tests are a port of the Java JUnit tests in `dev.kaldiroglu.dp.introduction`. Each Java
test class has one C# test class, and each Java test method has a `[Fact]` with the same
name in PascalCase, the same display name and the same expected values. Every Java test is
ported.

## What is tested

| File | Java class | Java tests | C# tests | What it checks |
|---|---|---|---|---|
| `Problems/Users/UserProcessorTests.cs` | `problems.user.UserProcessorTest` | 4 | 4 | `ProcessUser` takes 7 parameters. Its `if` statements are nested 15 levels deep (counted from the braces, comments removed). It has 43 `return` statements, each with a different result. The three users in `Main` get the answers `Main` prints. |
| `Problems/Pricing/SwitchingCheckoutTests.cs` | `problems.pricing.SwitchingCheckoutTest` | 4 | 4 | The switch prices one basket at 1200.00, 960.00, 720.00 and 800.00. A typo in a campaign name throws `ArgumentException` at run time. Five campaigns are five arms of one switch in one `Ring` method. `StageOneMain` prints the six lines the slide's run shows. |
| `Proxy/Pm/PrimeMinisterTests.cs` | `proxy.pm.PrimeMinisterTest` | 7 | 7 | Stage one: the PM hears 2 problems and resolves 1. Stage two: the proxy screens 2 and passes 1 on, `Proxy` shares no type with `PM`, and `Citizen`'s field is a `Proxy`. Stage three: 2 screened, 1 passed on, 1 refused, and `Citizen`'s field is an `IPM`. The secretary hands out a `ProxyPM`, creates it only when asked, and only once. `FindJob` never reaches the `RealPM`. A `null` subject is rejected. |
| **Total** | | **15** | **15** | |

## Changes from the Java tests

- The depth test counts braces in the C# file, whose braces are on their own lines. The
  depth is the same: one level for the class, one for the method, and 15 for the `if`
  statements.
- A switch expression has no `case` keyword, so the branch count matches the arms of the
  switch (`"NONE" =>`), where Java counts `case "`. A typo throws `ArgumentException`,
  where Java throws `IllegalArgumentException`.
- A `null` subject throws `ArgumentNullException`, where Java throws
  `NullPointerException`.

## Run it with

```bash
cd "~/Development/NET/Design Patterns/Design Patterns with CSharp/Introduction"
~/.dotnet/dotnet test tests/Introduction.Tests
```
