<!-- Model: Claude Opus 4.8 (claude-opus-4-8) — Created: 2026-07-13 -->

# Proxy — C# (.NET)

*For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev*

**Project:** Structural Design Patterns course — Proxy (C# implementation)
**Created:** 2026-07-13
**Source:** GoF, *Design Patterns*, Proxy, **pp. 207–217**. Ported from the Java repository `StructuralPatternsInJava` (package `dev.kaldiroglu.dp.structural.proxy`).

C#/.NET port of the Proxy examples used in the Structural Design Patterns course. The "Provide a surrogate or placeholder for another object to control access to it" intent (GoF, p. 207) is shown through three complementary examples.

## Expected benefit

The client always programs to the **Subject** interface and never learns whether it holds the real object or a stand-in. That indirection lets a proxy add behaviour — *lazy creation*, *access control*, *logging* — without the client changing at all.

## The three examples

| Example | Namespace | Proxy kind | What it shows |
|---------|-----------|------------|---------------|
| **GoF** | `dev.kaldiroglu.Proxy.Gof` | Virtual proxy | The book's document-editor example (p. 207). `ImageProxy` caches the `Extent` and defers loading the expensive real `Image` until the first `Draw`. |
| **Network** | `dev.kaldiroglu.Proxy.Network` | Protection proxy | `ProxyServer` implements the same `INetwork` as the real `Gateway`, but logs and enforces an access policy (blocks FTP to `192.*`, telnet from `10.*`) before delegating. |
| **PM** | `dev.kaldiroglu.Proxy.Pm.Pm1/Pm2/Pm3` | Teaching progression | A three-stage refactoring of a "citizen ↔ Prime Minister" scenario from *no proxy* (`Pm1`) → a *hand-rolled proxy* (`Pm2`) → the *canonical GoF proxy* sharing an `IPM` interface (`Pm3`). |

## Participants (GoF roles)

- **Subject** — `IGraphic` (Gof) · `INetwork` (Network) · `IPM` (Pm3).
- **RealSubject** — `Image` · `Gateway` · `RealPM`.
- **Proxy** — `ImageProxy` · `ProxyServer` · `ProxyPM` / `PmProxy`.
- **Client** — `TextDocument` · `NetworkDemo` · `Citizen`.

## Architecture

- **Root namespace:** `dev.kaldiroglu.Proxy`.
- **net8.0**; nullable reference types and implicit usings enabled; common settings in `Directory.Build.props`.
- **Layout:** `src/Proxy` (class library with the three examples), `src/Proxy.Demo` (console app that runs all three), and `tests/Proxy.Tests` (xUnit), wired by `Proxy.sln`.

## Notes on the port

The Java `network` example used Turkish identifiers and messages (`YasakKardesimException`, Turkish log text). These were translated to English — `YasakKardesimException` became `AccessDeniedException` — while the filtering rules and behaviour were preserved exactly.

## Run it with

```bash
dotnet test                             # run the xUnit tests (see Test.md)
dotnet run --project src/Proxy.Demo     # run all three demos
```
