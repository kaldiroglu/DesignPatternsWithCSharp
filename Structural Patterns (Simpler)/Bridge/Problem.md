# Bridge — Problem

Shapes (`Circle`, `Square`) need to be drawn in different colors (`Red`, `Blue`).

## Without the pattern

One subclass per combination — `RedCircle`, `BlueCircle`, `RedSquare`, `BlueSquare`. *M × N* classes; adding one color forces *M* new classes.

See `Problem/`.

## With the Bridge pattern

Split the hierarchy in two:

- **Abstraction**: `Shape` (holds an `IColor`); subclasses `Circle`, `Square`.
- **Implementor**: `IColor`; impls `RedColor`, `BlueColor`.

`M + N` instead of `M × N`.

See `Solution/`.
