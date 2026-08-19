# CLAUDE.md

Guidance for Claude Code when working on **Design Patterns with C#**.

*Claude Opus 5 (claude-opus-5) — Created on 2026-08-19*

## What is here

The C# side of the design-patterns course. Seven pattern folders — `Adapter`, `Bridge`,
`Composite`, `Decorator`, `Facade`, `Flyweight`, `Proxy` — each a port of the Java in a
**separate repository**:

```
~/Development/Java/Idea/Design Patterns/Design Patterns with Java
```

package root `dev.kaldiroglu.dp.structural.<pattern>`. Ports follow that repository
example for example, name for name, and number for number. When a figure differs, the
port is wrong.

**This repository is downstream, always.** A worked example is designed in Java, tested
there, and pushed; only then is it carried across here and pushed. Nothing is invented in
C# first and back-ported, and no port is pushed while the Java it came from is still
uncommitted. So when something needs changing, the question is which repository the change
belongs in — and the answer is Java unless the thing is C#-only, which in practice means a
language difference recorded under "Porting Java to C#" below.

`Structural Patterns (Simpler)/` and `Structural Patterns (Business)/` are **older,
separate** single-project demos with their own `uml/` folders. They are not the ports.
Do not confuse the two, and do not "align" them.

The slide decks that quote all of this live in a third place,
`~/Development/Claude/Training/Design Patterns/Structural`, which has its own `CLAUDE.md`.

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

- **A whole-solution `dotnet test` exits non-zero even when every suite passes.**
  `Flyweight` still targets `net8.0`, and `~/.dotnet` carries no .NET 8 runtime — only
  `/usr/local/share/dotnet` does — so its test host dies with "You must install or update
  .NET to run this application" while the other eight suites pass. Everything else is
  `net10.0`. Read the per-suite lines, not the exit code, until Flyweight is moved up.

## Layout

Per pattern: `<Pattern>.sln`, `Directory.Build.props`, `README.md`, `Test.md`,
`src/<Pattern>/`, `src/<Pattern>.Demo/`, `tests/<Pattern>.Tests/`. Every project is also
registered in the root solution under a solution folder named for the pattern.

`Directory.Build.props` sets `net10.0`, `ImplicitUsings`, `Nullable`, `LangVersion latest`
and `RootNamespace dev.kaldiroglu.<Pattern>` once, for all three projects.

Source folders mirror namespaces: `src/Bridge/Notifications/Solution/Classic/…` is
`dev.kaldiroglu.Bridge.Notifications.Solution.Classic`.

**Not every folder is complete.** `Decorator` has no `.sln`, `README.md` or `Test.md`;
`Facade` and `Flyweight` have no `Directory.Build.props`. `Bridge` is the fullest example
of the intended shape — copy it.

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

## Tests

- **xUnit, one test project per pattern**, `Microsoft.NET.Test.Sdk` 17.12.0 and xunit
  2.9.2 (Flyweight is still on 2.7.0).
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

- Remote is `kaldiroglu/DesignPatternsWithCSharp`, branch `master`. Commit and push when
  asked — but check the Java repository is pushed first, because this one trails it. "Push
  all" means Java then C#; the deck repository is local and is never pushed.
- `.DS_Store` and `Design Patterns with CSharp.sln.DotSettings.user` are untracked on
  purpose. Never stage them; `git add -A` at the root will.
- Root `.gitignore` covers `bin/` and `obj/`, so a plain `git add <Pattern>` is safe.

## Documents

Per the global standards, every pattern folder carries a `README.md` and a `Test.md`, each
opening with the model name and creation date and the line "For further enquiry please
contact Akin Kaldiroglu at akin@kaldiroglu.dev", and each ending with a "Run it with"
section. Every command printed in those files must have been run before it is written
down, and every number in them must be asserted by a test.
