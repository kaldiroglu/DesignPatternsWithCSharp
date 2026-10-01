# Command — Design Patterns with C#

*Claude Opus 5.5 (claude-opus-5-5) — Created on 2026-10-02*

For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev

The C# port of the Command material from **Design Patterns with Java**
(`kaldiroglu/DesignPatternsWithJava`, package root
`dev.kaldiroglu.dp.behavioral.command`). The examples that repository carries are here, in
the same shape, under the root namespace `dev.kaldiroglu.Command`. The two legacy packages
`account.account1` and `account.account2` are not ported.

## What Command is for

A request turned into an object. A method call happens when it is made and is gone
afterwards; a command is complete when it is built — receiver, action and arguments — so it
can be held, put on a list, run later, run again, or taken back.

That is what gives a teller screen undo and redo without the account knowing either word,
lets a toolkit's menu item ship in a library compiled before the application that uses it,
and lets a bank queue the day's standing orders for the night run.

## The examples

| Namespace | What it shows |
|---|---|
| `Command.Account` | A bank teller with an Undo button. `Domain` is the account that does banking and nothing else. `Problem` is three naive stages — the account remembers its last move, the account keeps a history, the history moves into a teller — and the third breaks the moment transfers are added. `Solution` makes each transaction an object: `Deposit`, `Withdraw`, `CloseOut`, a `Transfer` made of the first two, a `Teller` that holds them, and `StandingOrders` that queue them for the night. |
| `Command.Gof` | GoF's own example (Design Patterns, pp. 233–235): menu items, documents and an application. `Problem.MenuItem` branches on its own label; `Solution` gives it an `ICommand`, with `OpenCommand`, `PasteCommand`, `MacroCommand` and `SimpleCommand`. |
| `Command.Lender` | A classroom example in three steps: a lender bound to one borrower class, then to a borrower interface, then to a command that a borrower and a tax office both implement. |
| `Command.Ac` | A classroom example: a wall switch that turns an air conditioner on and off and drives its heater and cooler through one command object per button. |
| `Command.Hw` | The three homework exercises: a television remote with undo (`Remote`), a restaurant's order rail (`Kitchen`), and an editor that records a macro and replays it on another editor (`Macro`). |

### Things worth stopping on

**The reversal in the teller.** `Problem.Teller` is where a careful team lands, and it
works until a transfer reuses the withdrawal and the deposit: it records two entries, so one
Undo takes back the deposit and leaves the withdrawal. `Solution.Transfer` is one object, so
it goes on the history once and comes off it once.

**A command that must remember what it did.** `CloseOut` carries no amount, so its undo
cannot be computed from the request; it stores what it actually took. `SelectChannel`
stores the channel that was on before, and `VolumeUp` stores whether the press changed
anything at all. This is GoF implementation issue 2 (supporting undo and redo).

**A command with state has to be fresh on every press.** `RemoteControl` holds a
`Func<ICommand>` per button rather than a command, so pressing a button twice puts two
objects on the history, each remembering its own press.

**Cancelling is not undoing.** An order still on the rail has not run, so cancelling it is
removing a ticket; nothing needs reversing.

## Architecture

- **One class library, `Command`**, holding every example as nested namespaces —
  `Account`, `Gof`, `Lender`, `Ac` and `Hw`. Sources mirror namespaces:
  `src/Command/Account/Solution/…`.
- **A console runner, `Command.Demo`**, that runs the examples which have a `main` method
  in the Java original — the three lender steps and the air conditioner — each on its own.
- `net10.0`, nullable reference types on, implicit usings on — set once in
  `Directory.Build.props` and inherited by both projects.
- There is no test project yet.

## Differences from the Java original

The port is faithful in behavior. What had to change:

- **Interfaces take the `I` prefix**, as the rest of this solution does: `ITransaction`,
  `ICommand` (in `Gof.Solution`, `Lender.Pattern`, `Ac` and `Hw.Remote`), `IBorrower`,
  `IEditorCommand`. It also keeps every `Command` type clear of the `dev.kaldiroglu.Command`
  root namespace.
- **Accessors became properties** where they only read state: `Balance`, `Owner`,
  `Description`, `Label`, `Name`, `Text`, `IsOn`, `Channel`, `Volume`, `CanUndo`,
  `CanRedo`, `Size`, `Pending`, `Waiting`. Methods that hand back a copy of a list —
  `Journal()`, `Documents()`, `Labels()`, `Cooked()` — stay methods and return an
  `IReadOnlyList<T>`.
- **`Supplier` and `Consumer` became `Func` and `Action`.** `SimpleCommand<TReceiver>`
  takes an `Action<TReceiver>`, so Java's `Document::paste` is the lambda `d => d.Paste()`.
- **`BigDecimal` became `decimal`.** `Money` rounds half away from zero, which is Java's
  `HALF_UP`, and prints two decimal places in the invariant culture.
- **`Temperature.getTemperature()` is `Temperature.Value`.** C# does not allow a member to
  share its enclosing type's name.
- **The `main` methods became `Run()` methods** called by `Command.Demo`, which takes the
  example's name as an argument.

## Run it with

The `dotnet` on `PATH` cannot build this repository — a tracked `global.json` at the root
pins the SDK to 10.0.0. Use the .NET 10 SDK directly:

```bash
cd "~/Development/NET/Design Patterns/Design Patterns with CSharp/Command"

# build everything
~/.dotnet/dotnet build

# every example, in the order the course presents them
~/.dotnet/dotnet run --project src/Command.Demo

# one example on its own
~/.dotnet/dotnet run --project src/Command.Demo -- lender-pattern
```

The runner accepts: `lender-problem1`, `lender-problem2`, `lender-pattern`, `ac`.

There are no tests to run yet.

From the repository root, `~/.dotnet/dotnet build "Design Patterns with CSharp.sln"`
builds Command along with every other pattern.
