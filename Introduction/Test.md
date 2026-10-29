*Claude Opus 5.5 (claude-opus-5-5) — Created on 2026-10-29*

# Tests — Introduction

For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev

11 xUnit tests in one project, `tests/Introduction.Tests`. All of them are **unit tests**:
no process boundary, no network. Two of them read one source file,
`src/Introduction/Problems/ComplexIfs/UserProcessor.cs`, to count what the deck quotes.

The tests are a port of the Java JUnit tests in `dev.kaldiroglu.dp.introduction`. Each Java
test class has one C# test class, and each Java test method has a `[Fact]` with the same
name in PascalCase, the same display name and the same expected values. Every Java test is
ported.

## What is tested

| File | Java class | Java tests | C# tests | What it checks |
|---|---|---|---|---|
| `Problems/ComplexIfs/UserProcessorTests.cs` | `problems.complexIfs.UserProcessorTest` | 4 | 4 | `ProcessUser` takes 7 parameters. Its `if` statements are nested 15 levels deep (counted from the braces, comments removed). It has 43 `return` statements, each with a different result. The three users in `Main` get the answers `Main` prints. |
| `Proxy/Pm/PrimeMinisterTests.cs` | `proxy.pm.PrimeMinisterTest` | 7 | 7 | Stage one: the PM hears 2 problems and resolves 1. Stage two: the proxy screens 2 and passes 1 on, `Proxy` shares no type with `PM`, and `Citizen`'s field is a `Proxy`. Stage three: 2 screened, 1 passed on, 1 refused, and `Citizen`'s field is an `IPM`. The secretary hands out a `ProxyPM`, creates it only when asked, and only once. `FindJob` never reaches the `RealPM`. A `null` subject is rejected. |
| **Total** | | **11** | **11** | |

## Changes from the Java tests

- The depth test counts braces in the C# file, whose braces are on their own lines. The
  depth is the same: one level for the class, one for the method, and 15 for the `if`
  statements.
- A `null` subject throws `ArgumentNullException`, where Java throws
  `NullPointerException`.

## Run it with

```bash
cd "~/Development/NET/Design Patterns/Design Patterns with CSharp/Introduction"
~/.dotnet/dotnet test tests/Introduction.Tests
```
