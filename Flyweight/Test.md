# Tests — Flyweight (C#)

**Type:** Unit tests (xUnit).
**Location:** `tests/Flyweight.Tests/FlyweightTests.cs`.

## What is covered

| Test | What it proves |
|------|----------------|
| `FactorySharesCharacterFlyweights` | Same letter ⇒ the **same** instance (`Assert.Same`); pool size = 1. |
| `DistinctLettersGetDistinctFlyweights` | Different letters ⇒ different flyweights; a repeat is a pool hit. |
| `RowsAndColumnsAreNeverShared` | `Row`/`Column` are UnsharedConcreteFlyweights — a fresh instance each call. |
| `SameFlyweightRendersInDifferentFonts` | One shared `'t'` draws in Times then Courier via the `GlyphContext`. |
| `DocumentDrawsInOrder` | A `Column`→`Row`→`CharacterGlyph` tree renders its text in order. |
| `SharingReducesObjectCount` | `"banana"` = 6 occurrences but only 3 distinct flyweights. |

## Run it with

```bash
~/.dotnet/dotnet test
```

The `dotnet` on `PATH` will not do: a tracked `global.json` at the repository root pins the
SDK to 10.0.0, and the PATH SDK reports that as "The command could not be loaded".
