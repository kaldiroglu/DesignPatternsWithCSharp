# Flyweight — C# (.NET)

*For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev*

**Project:** Structural Design Patterns course — Flyweight (C# implementation)
**Created:** 2026-06-22
**Source:** GoF, *Design Patterns*, Flyweight, **pp. 195–206**.

C#/.NET implementation of GoF's Lexi document-editor example for the Flyweight pattern. See the [topic README](../README.md) for the full pattern explanation and the UML diagram.

## Expected benefit

Demonstrates how *sharing* fine-grained objects keeps memory bounded: a 20-character document is rendered using only **9** `CharacterGlyph` objects, because each distinct letter is a single shared flyweight.

## Functional properties

- `GlyphFactory.CreateCharacter(c)` returns a **shared** `CharacterGlyph` per distinct char code.
- Intrinsic state (`Charcode`) lives in the immutable `CharacterGlyph`; extrinsic state (font, position) is supplied by `GlyphContext` / `Window` at draw time.
- `Row` and `Column` are *unshared* composites that hold the shared characters.

## Architecture

- **Namespace:** `dev.kaldiroglu.Flyweight`.
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
```
