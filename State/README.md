# State — Design Patterns with C#

*Claude Opus 5.5 (claude-opus-5-5) — Created on 2026-10-08*

For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev

The C# port of the State material from **Design Patterns with Java**
(`kaldiroglu/DesignPatternsWithJava`, package root `dev.kaldiroglu.dp.behavioral.state`).
Every class that repository carries is here, in the same shape, under the root namespace
`dev.kaldiroglu.State`.

## What State is for

An object changes its behavior when its state changes. Each state is its own object, and the
context forwards every request to its current state. To the caller, the object seems to
change its class.

That is what lets an order refuse to be cancelled once it is shipped, without a switch in
every method. It is also what lets a shipment carry its own tracking number and its own count
of failed deliveries, so that a new shipment starts again at zero.

## The examples

| Namespace | What it shows |
|---|---|
| `State.Order` | The main worked example: an online order that is placed, paid, shipped, delivered or cancelled. A courier gets three delivery attempts; after three failures the parcel goes back to the warehouse and can be shipped again. `Problem` is three naive stages: four boolean flags (`FlagOrder`), one status field with a switch in every method (`SwitchingOrder`, `Status`), and an enum constant with its own behavior per status (`EnumOrder`, `OrderStatus`). `Solution` has the context `Order`, the state `IOrderState` and the five states `Placed`, `Paid`, `Shipped`, `Delivered` and `Cancelled`. `Main` runs the same story with stage three and with State objects. |
| `State.Gof` | GoF's own example (Design Patterns, pp. 305–313). `Problem.TCPConnection` switches over three states in every operation. `Solution` has the context `TCPConnection`, the abstract state `TCPState`, and `TCPClosed`, `TCPListen` and `TCPEstablished`, each a single shared object. |
| `State.Door` | A door that is open or closed. `Problem` uses a boolean and an `if`. In `Pattern1` the states change the door's state, and each state knows the other one. In `Pattern2` a central `DoorStateManager` changes it, and only the manager knows all the states. |
| `State.Account` | A bank account that is `Active`, `Overdrawn`, `Frozen` or `Closed`. |
| `State.Elevator` | An elevator that is stopped, going up or going down. The caller sets the state. |
| `State.Person` | A person who says hello and goodbye in a happy or a sad way. |
| `State.Pattern` | An earlier outline of GoF's TCP example. Its methods are empty, as in the Java, so there is nothing to run. |
| `State.Hw` | The three homework exercises: an air conditioner whose states decide the next state (`AirConditioner`), a document whose transitions are one central table (`Document`), and a vending machine with a coin slot (`Vending`). |

### Things worth stopping on

**Where stage three fails.** An enum constant is one object shared by every order, so the
failed-attempt count has to live in `EnumOrder`. Shipping again does not reset it. The
second shipment's first failure is attempt 4, and the check is `== 3`, so the parcel never
goes back again. The demo prints `delivery attempt 4 failed` for the enum and
`delivery attempt 1 failed` for the State objects, because a new `Shipped` record starts at
zero.

**Who changes the state.** In `Order.Solution`, `Gof.Solution`, `Door.Pattern1` and
`Hw.AirConditioner` the states decide. In `Door.Pattern2` and `Hw.Document` one central
object decides. In `Elevator` and `Person` the caller decides.

**The account has two bugs, and the port keeps them.** `Frozen.Deposit` and
`Overdrawn.Deposit` contain `balance = +amount`. In the Java it is written `balance =+ amount`,
and in both languages it sets the balance to the amount instead of adding the amount to it.
`Overdrawn.Deposit` also takes the overdraft limit off the amount first. In the demo the
frozen account at -1000 receives 2000 and its balance becomes 2000, not 1000. The lines are
kept so that the output is the same as the Java output, and each one carries a `// NOTE:`
comment.

**"Stooping!" is the Java spelling.** `GoingUpState.Stop` and `GoingDownState.Stop` print
`Stooping!`, as the Java does. The elevator demo never calls `Stop`, so the word does not
appear in the output.

## The C# form of a sealed interface

Java's `sealed interface … permits …` lists every class that may implement the interface.
C# has no sealed interface, so the port uses the closest form in each place:

- **`IOrderState` is a public interface, and the five states are `sealed record`s.** No state
  can be extended, but the compiler does not stop another class from implementing
  `IOrderState`. The Java's other benefit — the compiler knows the five states are all there
  are — has no use in the Java code, so nothing else changes.
- **`IVendingState` is an `internal` interface, and its three states are `internal sealed`
  classes.** The Java interface and its states are package-private, so here no code outside
  the assembly can implement the interface.

Java's default interface methods are C# default interface methods. They work cleanly here:
`IOrderState` refuses every operation by default through a private interface method `Reject`,
as the Java does, and `IVendingState.Refilled` returns the same state by default. A state
that allows an operation writes a public method with the same signature, and that method
implements the interface member.

## Architecture

- **One class library, `State`**, holding every example as nested namespaces — `Order`,
  `Gof`, `Door`, `Account`, `Elevator`, `Person`, `Pattern` and `Hw`. Sources mirror
  namespaces: `src/State/Order/Solution/…`.
- **A console runner, `State.Demo`**, that runs the Java original's `main` methods — the
  order, GoF's connection, the three doors, the account, the elevator and the person — and,
  in addition, the three homework exercises, each on its own.
- `net10.0`, nullable reference types on, implicit usings on — set once in
  `Directory.Build.props` and inherited by both projects.
- There is no test project, as requested.

## Differences from the Java original

The port is faithful in behavior. The output of `order`, `gof`, `door-problem`,
`door-pattern1`, `door-pattern2`, `account`, `elevator` and `person` is byte for byte the
output of the Java `main` methods. The homework output is byte for byte the output of a
Java driver that makes the same calls. The whole runner prints the same under a Turkish and
a Swedish locale. What had to change:

- **Interfaces take the `I` prefix**: `IOrderState`, `IDoorState` (in both door patterns),
  `IAccountStatus`, `IElevatorState`, `IEmotionalState`, `IVendingState`, and the outline's
  `ITCPConnection` and `ITCPState`. GoF's `TCPState` in `Gof.Solution` is an abstract class,
  so it keeps its name.
- **`OrderStatus` is a class, not an enum.** A C# enum cannot have methods, and stage three
  is an enum whose constants each have their own body. The port is an abstract class with a
  private constructor and five shared instances, `OrderStatus.PLACED` to
  `OrderStatus.CANCELLED`. Each instance is a private nested class that overrides the
  operations it allows. Nothing outside the class can make a sixth one, as with an enum, and
  each instance is still one object shared by every order — which is the point of the stage.
- **Enum constants keep the Java names** (`PLACED`, `IN_REVIEW`, `SUBMIT`, `CLOSED`), not
  C#'s PascalCase. The examples print the names or put them in error messages, and
  `IN_REVIEW` has to read `in_review` in both languages.
- **The exhaustive switch needed two adjustments.** `SwitchingOrder`'s switches have no
  default case, as in the Java. A C# enum may hold a value no constant names, so a switch
  that covers every constant still draws warning CS8524; `SwitchingOrder` suppresses that
  one warning. A missing constant is warning CS8509 in C# where Java refuses to compile, so
  `State.csproj` makes CS8509 an error — adding a status breaks the build until every switch
  has its case, as in the Java.
- **GoF's switching connection uses `case … break`.** The Java uses the arrow form of the
  `switch` statement; C# has the arrow form only for switch expressions.
- **The records print the way Java prints them.** Java prints the shipped state as
  `Shipped[trackingNumber=TR-2, failedAttempts=1]`; a C# record would print
  `Shipped { TrackingNumber = TR-2, FailedAttempts = 1 }`. Each of the five order records
  overrides `ToString()` to print the Java form, so the order demo prints the same line.
- **Singletons: `INSTANCE` is `static readonly … Instance`** in `TCPClosed`, `TCPListen`,
  `TCPEstablished` and the four air-conditioner states.
- **Accessors became properties**: `State`, `Status`, `Events`, `Log`, `Name`, `IsOpen`,
  `Room`, `Target`, `Stock`, `OverdraftLimit`. The account's three getter and setter pairs
  became read-write properties: `Status`, `Balance` and `IsFrozen` (Java: `isFrozen()` and
  `setFrozen`). Setters that are alone or that do more than store a value stay methods:
  `SetTarget`, `SetState`, `SetEmotionalState`, `SetDoor`, `SetOpenState`, `SetClosedState`.
- **The internal log method is `Record`.** `AirConditioner` and `VendingMachine` have
  `log(String)` to add a line and `log()` to read the lines. C# cannot give a method and a
  property the same name, so the reader is the property `Log` and the writer is the
  internal method `Record`, the name GoF's `TCPConnection` already uses for the same job.
- **GoF's switching connection calls its enum `ConnectionState`.** The Java nested enum is
  named `State`. A C# class cannot have a nested type and a property with the same name,
  and the property `State` is what `Main` reads.
- **Names that clash, and how they are resolved.** The namespaces `Order`, `Door`,
  `Account`, `Elevator` and `Person` each hold a class with the same name. Inside each
  namespace the plain name means the class, because a type in the current namespace wins.
  In `State.Demo`, one level up, the plain name would mean the namespace, so the runner uses
  aliases (`OrderMain`, `DoorPattern1Test`, `AccountTest` and so on). The homework enum
  `Hw.Document.Action` shares its name with `System.Action`; inside its namespace the enum
  wins, and the runner reaches it through the alias `DocumentAction`, because the runner's
  own table uses `System.Action`. `SwitchingOrder.Status` and `Document.Status` are
  properties whose type is also named `Status`, which C# allows. The root namespace ends in
  `State`, and many classes have a property called `State`; the two never meet, because
  inside a class the property is found first.
- **Package access became `internal`**: `TCPConnection.ChangeState` and `Record`,
  `Order.Event`, `EnumOrder`'s `TrackingNumber`, `FailedAttempts` and `MaxAttempts`, both
  `Door.ChangeState` methods, `Door.dsm` in `Pattern2`, `Person.emotionalState`, the
  air-conditioner and vending-machine members the states use, and the package-private types
  `AcState` and `IVendingState` with its states. `internal` is the whole assembly rather than
  one package. C# has nothing closer.
- **`Optional<Status>` became `Status?`.** `Workflow.After` returns `null` when an action is
  not allowed, and `Document.Apply` throws with `??`, as the Java throws with `orElseThrow`.
  `EnumMap` is `Dictionary`.
- **`IllegalStateException` is `InvalidOperationException`**, and the account's
  `RuntimeException` is `Exception`.
- **`getClass().getSimpleName()` is `GetType().Name`** in `VendingMachine.State`.
- **The door demos print `true` and `false` in lower case**, as Java prints a `boolean`;
  C# would print `True` and `False`. The runner prints lists as `[a, b, c]`, as Java's
  `List.toString()` does.
- **Text does not depend on the machine's locale.** Status names are lower-cased with
  `ToLowerInvariant`; under a Turkish locale `ToLower` would turn `PAID` into `paıd`. The
  account prints its balances with the invariant culture; under a Swedish locale -1000 would
  otherwise print with the minus sign U+2212.
- **Fields set after construction use `null!`.** In `Door.Pattern1` the states get their
  door and their other state after they are created, and in both door patterns the door's
  state is set by a method the constructor calls. The compiler cannot see this, so these
  fields start as `null!`, with a comment. In `Door.Pattern1` the Java instance initializer
  block is the start of the constructor body.
- **The outline's unused field draws warning CS0169**, "the field is never used".
  `ConcreteTCPConnection` suppresses that one warning around the field, and keeps the field
  and the empty methods as the Java has them.
- **The `main` methods became `Run()` methods** called by `State.Demo`. The commented-out
  lines in the account's Java `Test` are not ported. The homework demos are only in the
  runner.
- **The `uml/` diagrams and the `CD1.png` / `SD1.png` images in the Java packages are not
  ported yet.**

## Run it with

The `dotnet` on `PATH` cannot build this repository — a tracked `global.json` at the root
pins the SDK to 10.0.0. Use the .NET 10 SDK directly:

```bash
cd ~/"Development/NET/Design Patterns/Design Patterns with CSharp/State"

# build everything
~/.dotnet/dotnet build State.sln

# every example, in the order the course presents them
~/.dotnet/dotnet run --project src/State.Demo

# one example on its own
~/.dotnet/dotnet run --project src/State.Demo -- order
```

The runner accepts: `order`, `gof`, `door-problem`, `door-pattern1`, `door-pattern2`,
`account`, `elevator`, `person`, `hw-airconditioner`, `hw-document`, `hw-vending`.

There are no tests to run.

From the repository root, `~/.dotnet/dotnet build "Design Patterns with CSharp.sln"`
builds State along with every other pattern.
