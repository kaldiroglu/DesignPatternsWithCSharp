# Composite — Problem

A file system: `File` (leaf, has size) and `Directory` (composite, can contain others). Asking "what's the total size of this node?" should not depend on which type the caller is holding.

## Without the pattern

`File` and `Directory` are unrelated types with different APIs. Every walker writes type-tests and inlines its own recursion.

See `Problem/`.

## With the Composite pattern

Both implement `IFileSystemNode` (`Size`, `Name`). `Directory.Size` recurses uniformly over `IFileSystemNode` children.

See `Solution/`.
