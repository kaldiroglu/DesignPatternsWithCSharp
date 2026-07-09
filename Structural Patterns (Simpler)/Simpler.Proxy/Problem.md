# Proxy — Problem

A document viewer with many high-resolution images. Loading from disk is expensive; the user might never scroll to most of them.

## Without the pattern

`RealImage` loads bytes in its constructor. A hundred images = a hundred disk reads, even if only one is shown.

See `Problem/`.

## With the Proxy pattern

`IImage` has both `RealImage` (loads on construction) and `ImageProxy` (lazy: only constructs `RealImage` on first `Display()`). Client code is unchanged.

See `Solution/`.
