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

### `NetworkProxyTests` — the protection proxy controls access

| Test | What it proves |
|------|----------------|
| `ServerReturnsProxy` | The server hands the client a `ProxyServer`, never the real `Gateway`. |
| `FtpTo192IsForbidden` | FTP to a `192.*` address throws `AccessDeniedException`. |
| `TelnetFrom10IsForbidden` | Telnet from a `10.*` address throws `AccessDeniedException`. |
| `AllowedFtpIsDelegated` | An allowed request passes through to the real gateway without throwing. |

### `PmProxyTests` — the canonical GoF proxy (Pm3)

| Test | What it proves |
|------|----------------|
| `SecretaryReturnsProxyTypedAsInterface` | The `PMSecretary` returns a `ProxyPM` typed as `IPM`. |
| `RealAndProxyShareTheSameInterface` | `RealPM` and `ProxyPM` both satisfy the `IPM` subject contract. |
| `ProxyLogsThenDelegatesToRealPM` | Calling through the proxy logs the proxy first, then reaches the real PM. |

## Run it with

```bash
dotnet test
```
