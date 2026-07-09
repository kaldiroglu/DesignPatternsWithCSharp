<!--
Model: Claude Opus 4.8 (1M context)
Created: 2026-07-06
-->

# Adapter Pattern — C# Course Code

*For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev*

**Project:** C# port of the Adapter-pattern examples from the Java course repo
(`StructuralPatternsInJava`), for the structural-patterns training.
**Created:** 2026-07-06
**Target:** .NET 10 · C# (latest lang version)

## Expected benefits

- Gives students of the course a **C# rendering** of exactly the same Adapter examples they see in
  Java, so they can compare the two languages side by side.
- Every example is **independently runnable** (`dotnet run`), so each idea can be demonstrated on
  its own.

## What's inside (functional overview)

A faithful 1:1 port of the Java `adapter` package tree. The examples build the story the slides
tell — from the problem, through the basic object/class adapters, to the pluggable adapters:

| Area | Project | What it shows |
|------|---------|---------------|
| Domain | `Electricity.Domain` (lib) | The Turkish (`tr`) and US (`us`) power/appliance domain reused by several examples. |
| Domain demos | `Electricity.Domain.Tr.Demo`, `Electricity.Domain.Us.Demo` | Running each domain on its own. |
| Problem | `Electricity.Problem1`, `Electricity.Problem2` | Why *not* to embed adaptation in the appliance (OCP/SRP violations). |
| Object adapter | `Electricity.PowerAdapter1`, `Electricity.PowerAdapter2` | `USTurkishPowerAdapter` wrapping a US source; #2 adds services (check/regulate). |
| Two-way adapter | `Electricity.TwoWayAdapter` | An adapter usable from both sides. |
| Class adapter | `Electricity.ClassAdapter` | `USTurkishPowerAdapter : USPowerSource, TurkishPowerSource`. |
| Pluggable (a) | `Pluggable.AbstractOperations` | Plug-in by **inheritance** (Template Method). |
| Pluggable (b) | `Pluggable.DelegateObject` | Plug-in by **composition** (swappable delegate). |
| Pluggable (c) | `Pluggable.Parameterized` | Plug-in by **functions** (`Action` lambdas). |
| GoF example | `Gof` (lib) + `Gof.Demo` | The drawing-editor `Shape`/`TextView`/`TextShape` example (object + class adapter). |
| Pluggable shape | `Gof.Pluggable` | The parameterized shape adapter (reuses the `Gof` library). |

## Architecture / conventions

- **Faithful 1:1 port**: type names match the Java exactly, including interfaces **without an `I`
  prefix** (`TurkishPowerSource`, `Shape`, `PowerDelivery`) so the code lines up with the Java and
  the slides. Java methods become PascalCase C# methods.
- Root namespace **`dev.kaldiroglu.Adapter`** (mandated `dev.kaldiroglu` root).
- Shared build settings live in `Directory.Build.props`; each `.csproj` is minimal.
- Cross-example reuse is expressed as **project references** (e.g. the domain examples reference
  `Electricity.Domain`; the shape example references `Gof`) — the C# equivalent of the Java
  cross-package `import`s.
- A few examples define more than one entry point in Java; those are exposed through a `Program`
  that dispatches on a command-line argument (noted under *Run it with*).

## Run it with

From the `CSharp` folder, build everything:

```bash
dotnet build Adapter.sln
```

Run any example (from its project folder, or with `--project`):

```bash
dotnet run --project src/Electricity.Domain.Tr.Demo
dotnet run --project src/Electricity.Domain.Us.Demo
dotnet run --project src/Electricity.Problem1
dotnet run --project src/Electricity.Problem2
dotnet run --project src/Electricity.PowerAdapter1
dotnet run --project src/Electricity.PowerAdapter2
dotnet run --project src/Electricity.TwoWayAdapter
dotnet run --project src/Electricity.ClassAdapter
dotnet run --project src/Pluggable.AbstractOperations
dotnet run --project src/Pluggable.DelegateObject
dotnet run --project src/Pluggable.Parameterized             # the appliance Main
dotnet run --project src/Pluggable.Parameterized -- demo     # the PluggableDemo (US/UK/Kenya)
dotnet run --project src/Gof.Demo                            # the DrawingEditor
dotnet run --project src/Gof.Pluggable                       # the pluggable shape demo
```
