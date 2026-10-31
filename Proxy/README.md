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
| **PM** | `dev.kaldiroglu.Proxy.Pm.Pm1/Pm2/Pm3/Pm4` | Teaching progression | A four-stage refactoring of a "citizen ↔ Prime Minister" scenario. `Pm1`: no proxy, the PM screens her own calls. `Pm2`: a stand-in screens, but it shares no type with the PM. `Pm3`: a protection proxy sharing an `IPM` interface; the first citizen creates the PM. `Pm4`: the proxy also creates the PM on the first forwarded call. Each entry prints the counters on the slides: two calls, one passed on, one refused. |

## Participants (GoF roles)

- **Subject** — `IGraphic` (Gof) · `INetwork` (Network) · `IPM` (Pm3, Pm4).
- **RealSubject** — `Image` · `Gateway` · `RealPM`.
- **Proxy** — `ImageProxy` · `ProxyServer` · `ProxyPM` / `PmProxy`.
- **Client** — `TextDocument` · `NetworkDemo` · `Citizen`.

## Architecture

- **Root namespace:** `dev.kaldiroglu.Proxy`.
- **net10.0**; nullable reference types and implicit usings enabled; common settings in `Directory.Build.props`.
- **Layout:** `src/Proxy` (class library with the three examples), `src/Proxy.Demo` (console app that runs all three), and `tests/Proxy.Tests` (xUnit), wired by `Proxy.sln`.

## Notes on the port

The Java `network` example used Turkish identifiers and messages (`YasakKardesimException`, Turkish log text). These were translated to English — `YasakKardesimException` became `AccessDeniedException` — while the filtering rules and behaviour were preserved exactly.

## Run it with

```bash
dotnet test                             # run the xUnit tests (see Test.md)
dotnet run --project src/Proxy.Demo     # run all three demos
dotnet run --project src/Proxy.Demo -- pm3-screening     # run one entry
```

The runner accepts: `gof`, `gof-twenty-figures`, `network`, `pm1`, `pm2`, `pm3-secretary`, `pm3-screening`, `pm4`.
