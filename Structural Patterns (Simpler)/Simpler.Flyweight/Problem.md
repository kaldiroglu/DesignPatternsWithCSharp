# Flyweight — Problem

Render a forest with millions of trees. Each tree has a position, plus a name, color, and a (large) texture. Most trees share name/color/texture across the forest.

## Without the pattern

Every `Tree` stores its own copy of the texture. A million trees ≈ a million copies — memory blows up.

See `Problem/`.

## With the Flyweight pattern

Split state:

- **Intrinsic** (shared, immutable): `name`, `color`, `texture` → `TreeType`. A `TreeFactory` interns one per distinct kind.
- **Extrinsic** (per-instance): `x`, `y` → stays on `Tree` with a reference to a shared `TreeType`.

A million `Tree`s, a handful of `TreeType`s.

See `Solution/`.
