<!--
Model: Claude Opus 4.8 (1M context)
Created: 2026-07-06
-->

# Tests — Adapter Pattern (C#)

**Type:** Unit tests (xUnit).
**Location:** `tests/Adapter.Tests/` — one project, referenced by `Adapter.sln`.

The tests assert the **adapter behaviours** (the pattern payoff), not the console output of the
demos. Where an adapter takes an interface, the tests inject a small recording **test double** and
count the calls it forwards; where it exposes state, the tests assert the state directly.

## What is covered

| Test class | Example under test | Asserts |
|------------|--------------------|---------|
| `GofTests` | `Gof` drawing-editor | object adapter `TextShape` converts extent → `BoundingBox` (10,10)->(110,30) and delegates `IsEmpty`; class adapter yields the same box; the pluggable shape adapter adapts via `Func<>` lambdas |
| `ClassAdapterTests` | `Electricity.ClassAdapter` | `TurnOn`/`TurnOff` toggle the inherited source; `TurnOn` is idempotent; the adapter **is-a** `USPowerSource` |
| `PowerAdapter1Tests` | `Electricity.PowerAdapter1` | the object adapter forwards `TurnOn`/`TurnOff` to the adaptee's `PushSwitch`, and `TurnOn` is guarded (idempotent) — verified with a recording `USPowerSource` |
| `TwoWayAdapterTests` | `Electricity.TwoWayAdapter` | from the US side `TurnOn` pushes the US switch; from the Turkish side `PushSwitch` turns on the Turkish source |
| `AbstractOperationsTests` | `Pluggable.AbstractOperations` (a) | the abstract `PowerAdapter` Template Method routes `TurnOn`/`TurnOff` to the abstract `Deliver`/`Cut` hooks (recording subclass) |
| `DelegateObjectTests` | `Pluggable.DelegateObject` (b) | `DelegatingPowerAdapter` forwards to a swappable `PowerDelivery` delegate |
| `ParameterizedTests` | `Pluggable.Parameterized` (c) | `PluggablePowerAdapter` runs the injected `Action` blocks (`TurnOn` twice, `TurnOff` once → 2/1) |

**13 tests total.**

## Notes

- `Pluggable.AbstractOperations.PowerAdapter` is `internal` (its Java original is package-private).
  The project exposes it to the test assembly via
  `[assembly: InternalsVisibleTo("Adapter.Tests")]` (see `src/Pluggable.AbstractOperations/AssemblyInfo.cs`),
  so the test can subclass it and verify the Template Method wiring.
- The "problem" anti-pattern demos and the pure domain demos are illustrative console programs, so
  they are exercised by running them (see the root `README.md` *Run it with*) rather than unit-tested.

## Run it with

```bash
dotnet test Adapter.sln        # from the CSharp folder
```
