# Chain of Responsibility — Design Patterns with C#

*Claude Opus 5.5 (claude-opus-5-5) — Created on 2026-10-09*

For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev

The C# port of the Chain of Responsibility material from **Design Patterns with Java**
(`kaldiroglu/DesignPatternsWithJava`, package root
`dev.kaldiroglu.dp.behavioral.chainOfResponsibility`). Every class that repository carries
is here, in the same shape, under the root namespace `dev.kaldiroglu.ChainOfResponsibility`:
60 Java files and 62 C# files. The two extra files are `Hw.Maintenance.RequestKind` and
`Hw.Middleware.MiddlewareChain`; the differences below say why.

## What Chain of Responsibility is for

A request goes along a line of handlers. Each handler either handles it or passes it to the
next one. The sender knows only the first handler, and never which handler answers. A new
handler, or a new order, is a change to how the chain is built, not to the sender or to the
other handlers.

That is what lets an expense be approved by someone with enough authority, and never by the
person who spent it, without one class that knows every name and every limit. It also means
that a request can reach the end of the chain with nobody handling it, and the chain must
say so.

## The examples

| Namespace | What it shows |
|---|---|
| `ChainOfResponsibility.Expense` | The main worked example: Elif leads the team (up to 1,000), Burak manages (10,000), Cem directs (50,000) and Deniz is the CFO (200,000). `Problem.ApprovalService` is stage one: one method with every name, limit and rule. `Problem.TeamLead`, `Manager`, `Director` and `Cfo` are stage two: each approver creates the next one by class. `Problem.LimitTable` is stage three: a table of limits, which chooses by amount alone. `Solution` has the handler `ExpenseHandler`, the concrete handlers `Approver` and `AuditLog`, and `Main`, which runs six expenses through every design. |
| `ChainOfResponsibility.Gof` | GoF's own example (Design Patterns, pp. 223–232): context-sensitive help. `Problem.HelpDesk` knows every control by name. `Solution` has `HelpHandler`, `Widget`, `Button`, `Dialog` and `Application`; each widget's parent is its successor. |
| `ChainOfResponsibility.Hw` | The three homework exercises: a maintenance queue that hands requests to developers (`Maintenance`), a middleware chain where every link may act (`Middleware`), and a cash machine where each slot pays part of the amount (`CashDispenser`). |
| `ChainOfResponsibility.CallCenter` | Customers call a call center. Every call reaches the standard desk first, then the gold desk, then the VIP desk. `Test` creates random customers. |
| `ChainOfResponsibility.Pattern` | Three handlers give help for three contexts: more specific, specific and generic. A handler that does not match asks the next handler and adds its own help to the answer. |

### Things worth stopping on

**Where stage three fails.** Burak spends 5,000 on a conference, and the table sends it to
Burak, who approves his own expense. Cem's 30,000 goes to Cem. Stages one and two and the
chain send them to Cem and Deniz. An expense of 500,000 reaches the end of the chain, and the
answer is "no one may approve it". The demo prints all of these lines.

**The chain can already exist.** GoF implementation issue 1 (implementing the successor
chain). In `Gof.Solution` the chain is not a new set of links: each widget's parent is its
successor, so the chain is the window's own containment.

**Two forms of chain.** In `Expense.Solution` and `Hw.Maintenance` the request stops at the
first link that handles it. In `Hw.Middleware` every link may act and then call the rest of
the chain, and in `Hw.CashDispenser` every link handles part of the request.

## Middleware, and the same shape in ASP.NET Core

The Java `hw.middleware` has two functional interfaces: `Endpoint`, with the method `serve`,
and `Middleware`, with the method `handle` and a static method `chain`. In C# both are
delegates:

```csharp
public delegate string Endpoint(HttpRequest request);
public delegate string Middleware(HttpRequest request, Endpoint next);
```

A link is a lambda that is given the request and the rest of the chain, and it calls the
rest as `next(request)`. A delegate type cannot hold a method, so Java's `Middleware.chain`
is `MiddlewareChain.Chain(IReadOnlyList<Middleware>, Endpoint)`. The first middleware in the
list runs first.

This is the same shape as ASP.NET Core middleware, where a link is written
`app.Use(async (context, next) => { ...; await next(context); })`: the link gets the request
and the rest of the pipeline, and it decides whether to call the rest. The port does not use
ASP.NET Core. `HttpRequest` here is the example's own record of a path and a user, not
ASP.NET Core's class of the same name.

## Architecture

- **One class library, `ChainOfResponsibility`**, holding every example as nested
  namespaces — `Expense`, `Gof`, `Hw`, `CallCenter` and `Pattern`. Sources mirror
  namespaces: `src/ChainOfResponsibility/Expense/Solution/…`.
- **A console runner, `ChainOfResponsibility.Demo`**, that runs the Java original's `main`
  methods — the expense approval before and after the pattern, GoF's help, the three
  homework exercises, the call center and the help topics — each on its own.
- `net10.0`, nullable reference types on, implicit usings on — set once in
  `Directory.Build.props` and inherited by both projects.
- **An xUnit project, `ChainOfResponsibility.Tests`**, in `tests/ChainOfResponsibility.Tests`. It is described in `Test.md`.

## Differences from the Java original

The port is faithful in behavior. The output of `expense-problem`, `expense`,
`gof-problem`, `gof-solution`, `gof`, `hw-maintenance`, `hw-middleware`,
`hw-cashdispenser` and `pattern` is byte for byte the output of the Java `main` methods, and
a call-center driver that sends one VIP and one gold customer prints the same lines in both
languages. These outputs are the same under a Turkish and a Swedish locale. `callcenter` chooses customers at random, so its output has the
same shape as the Java's, but not the same lines. What had to change:

- **Interfaces take the `I` prefix**: `ICallTaker`, `ICustomer`, `IHandler` and `IHelp`.
- **Java records are `sealed record`s** with the Java accessor names as properties:
  `Expense(string Submitter, int Amount, string Purpose)` (in `Problem` and in `Solution`),
  `Request(RequestKind Kind, string Title, int Days)` and `HttpRequest(string Path, string
  User)`. No example prints a record, so their `ToString()` is left as C# writes it.
- **`Request.Kind` is the separate enum `RequestKind`.** In the Java it is nested in the
  record. A C# record cannot hold both a property `Kind` and a nested type `Kind` (error
  CS0102), so the enum is a type of its own in the same namespace. Its constants keep the
  Java names — `BUG`, `UI_CHANGE`, `IMPROVEMENT`, `PROJECT` — as `Pattern.Context` keeps
  `GENERIC`, `SPECIFIC` and `MORE_SPECIFIC`.
- **`Endpoint` and `Middleware` are delegates**, and `Middleware.chain` is
  `MiddlewareChain.Chain`. See the section above. `Links` is a `static` class; Java's is
  `final` with a private constructor. `Links.Logging` takes an `IList<string>`, because it
  adds to it.
- **`LimitTable` uses a `SortedDictionary<int, string>`** where the Java uses a `TreeMap`.
  Java's `ceilingEntry(amount)` is a loop over the sorted rows that returns the first row
  whose limit is at least the amount. When there is none, the answer is "no one may approve
  it", as in the Java.
- **Accessors became properties**: `AuditLog.Seen`, `AbstractHelp.Description`,
  `IHelp.OtherHelp` (Java's `getOtherHelp`), and `AbstractCallTaker.Next` (a getter and
  setter pair). `HelpHandler.HasHelp()`
  stays a method, as it is in GoF.
- **Lists are returned as `IReadOnlyList<T>`**: `AuditLog.Seen` (a copy, as Java's
  `List.copyOf`), `RequestQueue.ProcessAll()` and `NoteSlot.Pay()`.
- **`virtual` is explicit, and abstract classes name their interface members.**
  `AbstractCallTaker.Answer`, `AbstractCustomer.AskAQuestion` and
  `AbstractHandler.HandleRequest` are declared `abstract`: Java's abstract classes do not
  mention them, but a C# abstract class must declare every member of its interface. Java's
  `final` methods `ExpenseHandler.handle` and `Developer.take` are plain C# methods, which
  cannot be overridden.
- **Nullable references are marked.** Every `next` and `successor` is nullable, because the
  last link has none. Where the Java calls `next` without a check (`StandardCallTaker`,
  `GoldCallTaker`, `ConcreteHandler1`, `ConcreteHandler2`), the port writes `next!` or
  `successor!`: it fails in the same place the Java would. `AbstractHandler.help` starts as
  `null!`, because every concrete handler sets it in its constructor.
- **`Math.random()` is `Random.Shared.NextDouble()`** in `CallCenter.Test`, with the same
  thresholds.
- **Number formats do not depend on the machine's locale.** Numbers that are printed are
  formatted with the invariant culture, and `Expense.Solution.Main` parses the amounts with
  it.
- **The `main` methods became `Run()` methods** called by `ChainOfResponsibility.Demo`. The
  homework demos print lists as `[a, b, c]`, as Java's `List.toString()` does.
- **Names that clash, and how they are resolved.**
  - The namespace `Expense` holds two records named `Expense`, in `Expense.Problem` and
    `Expense.Solution`. Inside each of those namespaces the plain name means that
    namespace's record. `Expense.Solution.Main` writes the problem's record as
    `Problem.Expense`: from inside `Expense.Solution`, `Problem` is found as a sibling
    namespace. The Java writes it with its full package name.
  - The namespace `Hw.Middleware` holds the delegate `Middleware`. Inside the namespace the
    plain name means the delegate, because a type in the current namespace wins. The runner
    uses `MiddlewareChain`, `Links` and `Endpoint`, and never names the delegate.
  - `Gof.Solution` has `Application`, `Button` and `Dialog`. The library uses no UI
    framework, so nothing else in scope has these names.
  - `Pattern.Context` and `Hw.Middleware.HttpRequest` do not meet any type that implicit
    usings bring in.
  - Ten namespaces have a class named `Main` or `Test`, as in the Java. `Main` is a static
    class with a `Run()` method, which C# allows. In `ChainOfResponsibility.Demo` the runner
    reaches them through aliases: `ExpenseProblemMain`, `ExpenseMain`, `GofMain`,
    `GofProblemMain`, `GofSolutionMain`, `MaintenanceMain`, `MiddlewareMain`,
    `CashDispenserMain`, `CallCenterTest` and `PatternTest`. `Gof.Main` sits next to the
    namespaces `Gof.Problem` and `Gof.Solution`, which have a `Main` each, so a file that
    imports all three writes it as `global::dev.kaldiroglu.ChainOfResponsibility.Gof.Main`.
- **The `uml/` diagrams, the `CD1.png`, `ClassDiagram1.png`, `SD1.png` and
  `SequenceDiagram*.png` images, and the per-package `README.md` files in the Java packages
  are not ported yet.** This README carries their content.

## Tests

`tests/ChainOfResponsibility.Tests` holds 34 xUnit tests in 7 test classes. They are ported from the
JUnit tests of the Java repository, one C# test class for each Java test class, with the
same expected values, except where `Test.md` notes a change. `Test.md` lists what each
class tests, and the Java tests that are not ported, with the reason.

## Run it with

The `dotnet` on `PATH` cannot build this repository — a tracked `global.json` at the root
pins the SDK to 10.0.0. Use the .NET 10 SDK directly:

```bash
cd ~/"Development/NET/Design Patterns/Design Patterns with CSharp/ChainOfResponsibility"

# build everything
~/.dotnet/dotnet build ChainOfResponsibility.sln

# every example, in the order the course presents them
~/.dotnet/dotnet run --project src/ChainOfResponsibility.Demo

# one example on its own
~/.dotnet/dotnet run --project src/ChainOfResponsibility.Demo -- expense
```

The runner accepts: `expense-problem`, `expense`, `gof-problem`, `gof-solution`, `gof`,
`hw-maintenance`, `hw-middleware`, `hw-cashdispenser`, `callcenter`, `pattern`.

Run the tests:

```bash
~/.dotnet/dotnet test ChainOfResponsibility.sln
```

From the repository root, `~/.dotnet/dotnet build "Design Patterns with CSharp.sln"`
builds Chain of Responsibility along with every other pattern.
