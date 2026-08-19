# Tests — Flyweight (C#)

**Type:** Unit tests (xUnit).
**Location:** `tests/Flyweight.Tests/FlyweightTests.cs`.

## What is covered

The suite is a port of Java's `GlyphFlyweightTest`, test for test and figure for figure. It
types the same two lines GoF's editor types there — `flyweight is a nice solution` and
`lightweight is also a nice solution` — so the numbers below are the ones that repository
asserts and the deck quotes.

| Test | What it proves |
|------|----------------|
| `SharingIsMeasured` | 63 characters of text cost 16 objects; 47 occurrences are free. |
| `TheFactoryShares` | Same letter ⇒ the **same** instance (`Assert.Same`); pool size = 1. |
| `DistinctLettersAreDistinctObjects` | Different letters ⇒ different flyweights. |
| `UnsharedConcreteFlyweightsAreNotPooled` | `Row`/`Column` are UnsharedConcreteFlyweights — a fresh instance each call, and still a `Glyph`. |
| `TheConstructorIsNotPublic` | Only the factory can make one; a public constructor would defeat the sharing. |
| `TheSameObjectRendersInTwoFonts` | The `'i'` shared across both lines draws in Times on one and Courier on the other. |
| `TheFlyweightHasNoFontField` | One field, the character code. The font is extrinsic; storing it would end the sharing. |
| `TheDocumentIsCorrect` | The tree renders back to the text that was typed. |

## Run it with

```bash
~/.dotnet/dotnet test
```

The `dotnet` on `PATH` will not do: a tracked `global.json` at the repository root pins the
SDK to 10.0.0, and the PATH SDK reports that as "The command could not be loaded".
