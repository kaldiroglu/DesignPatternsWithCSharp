# Structural Design Patterns (C# — Simpler)

Each subdirectory is a standalone .NET console project. Run any pattern with:

```bash
cd <Pattern>
dotnet run
```

Both the problem (anti-pattern) and the solution (patterned) demos are printed in sequence.

| Pattern   | Scenario               |
|-----------|------------------------|
| Adapter   | MP3/MP4/VLC media player wrapping a third-party advanced player |
| Bridge    | Shape × Color collapsed from M×N classes to M+N |
| Composite | File / Directory with uniform `Size`             |
| Decorator | Coffee with stackable Milk/Sugar/Vanilla add-ons |
| Facade    | `HomeTheaterFacade` over amp/DVD/projector/screen/lights |
| Flyweight | Forest of trees with interned `TreeType` metadata |
| Proxy     | Lazy `ImageProxy` deferring expensive disk loads |
