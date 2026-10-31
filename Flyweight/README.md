# Flyweight — C# (.NET)

*For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev*

**Project:** Structural Design Patterns course — Flyweight (C# implementation)
**Created:** 2026-06-22
**Source:** GoF, *Design Patterns*, Flyweight, **pp. 195–206**.

C#/.NET implementation of GoF's Lexi document-editor example for the Flyweight pattern. See the [topic README](../README.md) for the full pattern explanation and the UML diagram.

## Expected benefit

Demonstrates how *sharing* fine-grained objects keeps memory bounded: a 63-character document is rendered using only **16** `CharacterGlyph` objects, because each distinct letter is a single shared flyweight.

The port also carries three further examples, named on the slides:

- **Forest** (`Forest.Problem`, `Forest.Solution`): 10,000 trees of 2 kinds. Without sharing, 10,000 texture loads and 40,960,000 bytes; with a `TreeFactory`, 2 loads and 8,192 bytes.
- **Book** (`Book.Wrong`, `Book.Correct`): the first version shares nothing, stores position on the character and prints `this book` for a capital T. The corrected version types 236 characters with 29 objects, and 207 occurrences cost no object.
- **Quote** (`Quote.Problem`): 100,000 ticks for 4 instruments retain 100,000 distinct symbol objects, because `Wire.Decode` makes a new string for each field. Only the problem half is ported, because only that half has a run on a slide. There is no `Quote.Solution` here.

Every figure above is a count of loads, objects or array lengths, so it is the same as in the Java repository. The five runner entries print byte for byte what the Java classes print, compared by hand on 2026-10-31. No figure here measures heap size, so none differs between the runtimes.

## Functional properties

- `GlyphFactory.CreateCharacter(c)` returns a **shared** `CharacterGlyph` per distinct char code.
- Intrinsic state (`Charcode`) lives in the immutable `CharacterGlyph`; extrinsic state (font, position) is supplied by `GlyphContext` / `Window` at draw time.
- `Row` and `Column` are *unshared* composites that hold the shared characters.

## Architecture

- **Namespace:** `dev.kaldiroglu.Flyweight`, with `Forest`, `Book` and `Quote` sub-namespaces.
- **net10.0**; `Font`/`RenderedGlyph` modelled as `record` types; nullable reference types enabled.
- **Participants:** `Glyph` (Flyweight) · `CharacterGlyph` (ConcreteFlyweight) · `Row`, `Column` (UnsharedConcreteFlyweight) · `GlyphFactory` (FlyweightFactory) · `GlyphContext` (extrinsic-state holder) · `Program` (Client).
- **Layout:** `src/Flyweight` (console app) and `tests/Flyweight.Tests` (xUnit), wired by `Flyweight.sln`.

## Run it with

The `dotnet` on `PATH` cannot build this repository — a tracked `global.json` at the root
pins the SDK to 10.0.0, and the error it reports is the misleading
"The command could not be loaded". Use the .NET 10 SDK directly:

```bash
~/.dotnet/dotnet test                          # run the xUnit tests (see Test.md)
~/.dotnet/dotnet run --project src/Flyweight   # run the demo
~/.dotnet/dotnet run --project src/Flyweight -- forest-problem   # run one entry
```

The demo accepts: `gof-font-ranges`, `gof-sharing-count`, `forest-problem`, `forest`, `book-wrong`, `book-correct`, `quote-problem`.
