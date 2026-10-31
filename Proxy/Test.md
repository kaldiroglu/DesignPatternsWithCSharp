<!-- Model: Claude Opus 4.8 (claude-opus-4-8) — Created: 2026-07-13 -->

# Tests — Proxy (C#)

**Type:** Unit tests (xUnit).
**Location:** `tests/Proxy.Tests/`.

## What is covered

### `GofProxyTests` — the virtual proxy is lazy

| Test | What it proves |
|------|----------------|
| `FreshProxyHasNotLoadedImage` | Creating an `ImageProxy` does **not** create the real `Image`. |
| `GetExtentDoesNotLoadImage` | `GetExtent()` is answered from the cached extent — still no load. |
| `DrawLoadsImageExactlyOnce` | The first `Draw` loads the real image; a second `Draw` does not reload (`LoadCount == 1`). |
| `LayoutNeverLoadsImages` | Laying out a whole document reads only extents — no image is loaded. |
| `ProxyIsSubstitutableForRealImage` | Real image and proxy are interchangeable through `IGraphic`. |
| `TwentyFiguresDemoPrintsTheFigures` | The `gof-twenty-figures` entry prints 20 figures, a height of 5940, and 0 images read. |

### `NetworkProxyTests` — the protection proxy controls access

| Test | What it proves |
|------|----------------|
| `ServerReturnsProxy` | The server hands the client a `ProxyServer`, never the real `Gateway`. |
| `FtpTo192IsForbidden` | FTP to a `192.*` address throws `AccessDeniedException`. |
| `TelnetFrom10IsForbidden` | Telnet from a `10.*` address throws `AccessDeniedException`. |
| `TelnetFrom100IsAllowed` | `100.*` and `101.*` are not in the `10` network, so telnet from them is forwarded. |
| `AllowedFtpIsDelegated` | An allowed request passes through to the real gateway without throwing. |

### `PmProxyTests` — the three stages (Pm1, Pm2, Pm3)

The tests assert the same figures in every stage: two calls, one passed on, one refused.

| Test | What it proves |
|------|----------------|
| `StageOne` | Pm1: the PM heard 2 problems and resolved 1. She screens her own calls. |
| `StageTwo` | Pm2: 2 screened, 1 passed on, the PM heard 1. `PmProxy` shares no type with `PM`, and `Citizen`'s field is `PmProxy`. |
| `StageThree` | Pm3: 2 screened, 1 passed on, 1 refused. `Citizen`'s field is `IPM`. |
| `Substitutable` | `RealPM` and `ProxyPM` both implement `IPM`, and the secretary hands out the proxy. |
| `CreatedOnce` | The secretary creates the PM on the first `GetPM`, and only once. |
| `TheFirstCitizenCreatesThePM` | The first `Citizen` creates the PM, before any call. |
| `AnsweredWithoutForwarding` | `FindJob` never reaches the real PM. |
| `NullSubjectIsRejected` | A `ProxyPM` needs a subject. |
| `ScreeningDemoPrintsTheFigures` | The `pm3-screening` entry prints the three counters and does not claim a virtual proxy. |
| `SecretaryDemoPrintsTheFlag` | The `pm3-secretary` entry prints `False`, then `True`. |

### `Pm4ProxyTests` — a protection and virtual proxy (Pm4)

| Test | What it proves |
|------|----------------|
| `CreatedOnTheFirstForwardedCall` | A refused call and `FindJob` do not create the Prime Minister; the first forwarded call does. |
| `TheClientKnowsOnlyTheInterface` | `Citizen` has one constructor, and it takes `IPM`. |
| `TheRealSubjectIsNotPublic` | `RealPM` is `internal`. |
| `StageThreeLimits` | In Pm3, `Citizen` takes a `PMSecretary` and `RealPM` is public. |

## Run it with

```bash
dotnet test
```
