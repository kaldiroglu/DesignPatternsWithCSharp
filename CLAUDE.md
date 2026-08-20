# CLAUDE.md

Guidance for Claude Code when working on **Design Patterns with C#**.

*Claude Opus 5 (claude-opus-5) — Created on 2026-08-19*

## What is here

The C# side of the design-patterns course — **all three families, not just the structural
ones**. One folder per pattern at the root, each a port of the Java in a **separate
repository**:

```
~/Development/Java/Idea/Design Patterns/Design Patterns with Java
```

package root `dev.kaldiroglu.dp.<family>.<pattern>`, where `<family>` is `structural`,
`behavioral` or `creational`. Ports follow that repository example for example, name for
name, and number for number. When a figure differs, the port is wrong.

The seven structural patterns — `Adapter`, `Bridge`, `Composite`, `Decorator`, `Facade`,
`Flyweight`, `Proxy` — are ported. **`Strategy` is complete upstream and not ported yet** —
four worked examples and a homework folder under `dp.behavioral.strategy`, all quoted by
a built deck, so
this repository is a family behind. `creational` exists in the Java repository and is still
empty. **A new family
adds pattern folders beside the existing ones; it does not need a new repository, and
nothing in this file is structural-specific.** The C# folder is named for the pattern, not
the family — `Observer`, not `Behavioral.Observer` — and its root namespace is
`dev.kaldiroglu.<Pattern>`, so the family shows in the Java package and nowhere else.

**This repository is downstream, always.** The push order and what "push all" means are in
`~/.claude/CLAUDE.md`, which is loaded everywhere. What it means when working *here*: a
change belongs in Java unless it is C#-only, which in practice means one of the language
differences recorded under "Porting Java to C#" below.

`Structural Patterns (Simpler)/` and `Structural Patterns (Business)/` are **older,
separate** single-project demos with their own `uml/` folders. They are not the ports.
Do not confuse the two, and do not "align" them.

The slide decks that quote all of this live in a third place, one folder per family:
`~/Development/Claude/Training/Design Patterns/<Family>`. Only `Structural` exists so far,
and it has its own `CLAUDE.md`.

## The SDK

- **Use `~/.dotnet/dotnet`, which is 10.0.102.** The `dotnet` on `PATH` is
  `/usr/local/share/dotnet/dotnet`, whose newest SDK is 9.0.202.
- **A tracked `global.json` at the root pins the SDK to 10.0.0** (`rollForward:
  latestMajor`). The PATH `dotnet` cannot satisfy it, and the failure it reports from
  inside the repository is misleading — not NETSDK1045 but
  `The command could not be loaded … The application '--version' does not exist`. Seeing
  that message means the wrong `dotnet` is being used, nothing more.
- **`dotnet new sln` produces a `.slnx` under .NET 10.** The repository uses classic
  `.sln` throughout, so pass `--format sln`. `dotnet sln Foo.sln add` against a `.slnx`
  fails with "Could not find solution or directory".

## Commands

```bash
cd "~/Development/NET/Design Patterns/Design Patterns with CSharp"

~/.dotnet/dotnet build "Design Patterns with CSharp.sln"   # every pattern
~/.dotnet/dotnet build Bridge                              # one pattern, via its own .sln
~/.dotnet/dotnet test  Bridge/tests/Bridge.Tests
~/.dotnet/dotnet run --project Bridge/src/Bridge.Demo -- violation
cd Bridge && ./render-uml.sh                               # needs `brew install plantuml`
```

- **Every pattern port targets `net10.0`, and a whole-solution `dotnet test` is green.**
  It was nine suites and 264 tests when the structural family was complete; that figure is
  a snapshot and will move, so read the run rather than this line. `Flyweight` was the last
  project on `net8.0` and failed the run on its own, because `~/.dotnet` carries no .NET 8
  runtime; only `/usr/local/share/dotnet` does. If a project is ever pinned back below 10,
  expect that run to die with "You must install or update .NET to run this application"
  while every other suite passes.
- **The 14 older `Simpler` / `Business` demo projects are still `net9.0`.** They build,
  they carry no tests, and they are not ports — leave them alone unless asked.

## Layout

Per pattern: `<Pattern>.sln`, `Directory.Build.props`, `README.md`, `Test.md`,
`src/<Pattern>/`, `src/<Pattern>.Demo/`, `tests/<Pattern>.Tests/`. Every project is also
registered in the root solution under a solution folder named for the pattern.

`Directory.Build.props` sets `net10.0`, `ImplicitUsings`, `Nullable`, `LangVersion latest`
and `RootNamespace dev.kaldiroglu.<Pattern>` once, for all three projects.

Source folders mirror namespaces: `src/Bridge/Notifications/Solution/Classic/…` is
`dev.kaldiroglu.Bridge.Notifications.Solution.Classic`.

**Every pattern folder carries the full set**, so any of them can be copied when a new
pattern starts. `Bridge` is the fullest worked example. Two shapes vary for a reason:
`Facade` holds three independent examples rather than one library, so each of its projects
overrides `RootNamespace`; and `Flyweight` has no separate `.Demo` project, because its
library is itself the console app.

## Porting Java to C#

These all cost real time at least once. In rough order of how quietly they fail:

- **`Type.IsAbstract` is `true` for interfaces.** Anywhere a test counts "the abstract
  kinds", write `t.IsAbstract && !t.IsInterface` or the interface inflates the count by
  one. The Java original has no equivalent trap.
- **Test namespaces shadow library namespaces.** With `dev.kaldiroglu.Bridge.Tests.Retrofit`
  in scope, a bare `Retrofit.IVendorClient` binds to the *test* namespace and does not
  compile. Qualify the anchor type with `global::`.
- **An alias does not import extension methods.** `using Foo = a.b.c;` brings in types
  only; an extension method needs a plain `using a.b.c;`.
- **Interfaces take the `I` prefix**, so Java's `WindowImp` is `IWindowImp` and
  `NotificationChannel` is `INotificationChannel`. Diagrams and prose must follow.
- **C# has no constructor method group.** Java's `UrgentNotification::new` becomes
  `c => new UrgentNotification(c)`, and the parameter is a `Func<,>`.
- **`virtual` is explicit**, which is worth a sentence in the material rather than a
  silent fix: permitting an override is not sanctioning every override.
- **Names Java can use and .NET cannot.** `Console` is taken by `System.Console`; `Demo`
  is taken by the `<Pattern>.Demo` namespace. Bridge's helper ended up `Scenario`.
- **`RoundingMode.DOWN` on `BigDecimal`** is `Math.Truncate(x * 100m) / 100m` on `decimal`.
  Do not reach for `Math.Round`, which is banker's rounding by default.
- **When the language already is the pattern, port it anyway.** This will bite hardest in
  the behavioral family, where C# has `event` for Observer, `IEnumerable` and `yield` for
  Iterator, and delegates for Strategy and Command. The temptation is to write the
  idiomatic version instead of the GoF one. Don't: the port's job is to show the pattern
  the deck teaches, in the shape the Java shows it. If the idiomatic rendering is worth
  teaching — and it usually is — it belongs beside the classic one as a named variation,
  the way `Middleware.Solution` carries `Classic`, `Fluent` and `Functional` side by side.
  And because this repository is downstream, that variation is designed in Java first and
  mirrored here, not invented in C#.

## Tests

- **xUnit, one test project per pattern**, `Microsoft.NET.Test.Sdk` 17.12.0 and xunit
  2.9.2. `Flyweight` is the outlier, on 17.9.0 and xunit 2.7.0; it runs clean on `net10.0`
  at those versions, so this is untidiness rather than a fault.
- **Tests count the code; they do not restate the numbers.** `Assert.Equal(9, 3 * 3)`
  proves something about integers and goes on passing the day a fourth channel is added
  and the slide still says nine. Count types out of the namespace instead — see
  `Bridge/tests/Bridge.Tests/TypeCensus.cs`. Two Bridge tests count *source text*, because
  the fault they describe is a missing line rather than a missing type.
- **`CallerFilePath` is filled in at the call site**, so a helper that resolves the source
  root must capture it in its own file, not on a parameter of the public method. A caller
  in a subfolder otherwise anchors the path one directory too deep.
- **Parallelism and `Console.SetOut` do not mix.** The examples print, and Bridge's
  violation test captures `Console.Out` to prove a method prints *nothing* — under xUnit's
  default a capture opened by one class swallows another class's output. `Bridge.Tests`
  carries `[assembly: CollectionBehavior(DisableTestParallelization = true)]`. Any test
  project whose examples print needs the same.

## UML

`Bridge` is the pattern to follow: a `uml/` folder beside the code it describes —
`src/Bridge/Retrofit/uml/` — holding `.puml`, `.png` and `.svg`, with `render-uml.sh` at
the pattern root to regenerate. This mirrors the Java repository. Nothing in the build
depends on them; `.csproj` globs `*.cs` only.

- **A `<b>` span does not survive a line break.** PlantUML closes it at end of line and
  then prints the stray `</b>` literally on the image. Keep each `<b>…</b>` on one line.
  Six of these were found in one pass, two inherited verbatim from the Java originals —
  which are still wrong there.
- **Graphviz orders packages by content, not by declaration.** Two "three naive designs"
  diagrams came out 2, 1, 3. Hidden edges between the packages' members pin the order:
  `SwitchingNotifier -[hidden]right- UrgentEmailNotification`.
- **Bold opens a note with a complete claim; it is not for emphasis mid-sentence.**
  `It <b>is an</b> EmailSender` is the failure — reword so the bold half stands alone.
- **Verify names, do not recall them.** Every class, interface and member named in a
  diagram should be greppable in the sources; a short script over the `.puml` files
  catches drift after a rename.

## Repository arrangement

- Branch is `master`. The remote and the push order are in `~/.claude/CLAUDE.md`; check the
  Java repository is pushed before this one, because it trails it.
- `.DS_Store` and `Design Patterns with CSharp.sln.DotSettings.user` are untracked on
  purpose. Never stage them; `git add -A` at the root will.
- Root `.gitignore` covers `bin/` and `obj/`, so a plain `git add <Pattern>` is safe.

## Documents

Per the global standards, every pattern folder carries a `README.md` and a `Test.md`, each
opening with the model name and creation date and the line "For further enquiry please
contact Akin Kaldiroglu at akin@kaldiroglu.dev", and each ending with a "Run it with"
section. Every command printed in those files must have been run before it is written
down, and every number in them must be asserted by a test.
