# Observer — Design Patterns with C#

*Claude Opus 5.5 (claude-opus-5-5) — Created on 2026-10-08*

For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev

The C# port of the Observer material from **Design Patterns with Java**
(`kaldiroglu/DesignPatternsWithJava`, package root
`dev.kaldiroglu.dp.behavioral.observer`). Every class that repository carries is here, in
the same shape, under the root namespace `dev.kaldiroglu.Observer`. One class,
`PriceFeedWithEvent`, is added; it has no Java counterpart and is marked as such.

## What Observer is for

One object, the subject, keeps a list of other objects, the observers, and tells all of
them when it changes. The subject knows them only through one small interface, so new
observers can be added, and old ones removed, while the program runs, and the subject does
not change.

That is what lets a price feed tell a chart, a ticker and an alert about every change at the
moment it happens — so a spike from 100 to 106 and back is not missed — without the feed
knowing any of them by class.

## The examples

| Namespace | What it shows |
|---|---|
| `Observer.Price` | The main worked example: a stock price with three readers — a chart, a ticker and an alert that must fire when the price reaches 105. `Problem` is three naive stages: the feed calls every reader itself (`DirectPriceFeed`), the readers poll the feed once a second (`PollingReaders`), and the readers poll ten times a second and act only on a change (`ChangeOnlyReaders`). `Solution` has the subject `PriceFeed`, the observer `IPriceListener`, the event `PriceChange`, and the observers `Chart`, `Ticker` and `PriceAlert`. `Main` runs the same prices through stage three and through listeners. `PriceFeedWithEvent` is the C# `event` form of the same subject. |
| `Observer.Gof` | GoF's own example (Design Patterns, pp. 293–303). `Problem.ClockTimer` draws both clocks itself. `Solution` has `Subject`, `IObserver`, the concrete subject `ClockTimer`, and the concrete observers `DigitalClock` and `AnalogClock`, which pull the time after they are told something changed. |
| `Observer.Publisher` | A publisher prints magazines (`Newsweek`, `FourFourTwo`), and people and institutions subscribe to them (`IndividualSubscriber`, `InstitutionalSubscriber`). When a new issue comes out, every subscriber receives it. |
| `Observer.Payment` | An invoice tells the boss and the accountant when a payment is made. In Java it uses the JDK's `java.util.Observable` and `java.util.Observer`; here it uses a small `Observable` class and `IObserver` interface written for the port. |
| `Observer.Hw` | The three homework exercises: an account whose listener writes the transaction records (`AccountLog`), an auction in which a bidder stops watching while it is being told about a bid (`Auction`), and an inbox whose views are lambdas (`Inbox`). |

### Things worth stopping on

**Where stage three fails.** In ten seconds of polling, ten times a second, the readers make
100 reads. The price goes 100 → 102 → 106 → 100 → 103, and the step to 106 and back happens
between two polls. The demo prints `chart [102, 100, 103], alert fired: false, reads 100`
for the polling readers, and `chart [102, 106, 100, 103], alert fired: true` for the
listeners.

**Push and pull.** `PriceFeed` pushes a `PriceChange` with the old and the new price. GoF's
`ClockTimer` sends only itself, and the clocks pull the hour, minute and second. The inbox
does both: it pushes the message, and the badge pulls the unread count. GoF
implementation issue 6 (avoiding observer-specific update protocols: the push and pull
models).

**An observer that leaves during a notification.** `PriceFeed`, `Subject`, `Account`,
`Auction` and `Inbox` loop over a copy of their list. In the auction, Ali stops watching
while he is being told about Can's bid of 200; the loop goes on, and from the next bid only
Can is told. Without the copy, .NET throws `InvalidOperationException` ("Collection was
modified"), as Java throws `ConcurrentModificationException`.

## The C# forms: lambdas, `event`, and `IObservable<T>`

**A lambda as a listener.** Java's `PriceListener` has one method, so Java passes a lambda
where a `PriceListener` is expected. C# turns a lambda into a delegate, never into an
interface. The port keeps `IPriceListener` as an interface — it is the Observer the deck
teaches — and adds an overload, `PriceFeed.Subscribe(Action<PriceChange>)`, that wraps the
lambda in a small private adapter and returns it, so it can be unsubscribed later. The
inbox's views are `Action<Message>`, the C# delegate for Java's `Consumer<Message>`.

**The `event` keyword.** This is how a C# developer would usually write a subject.
`PriceFeedWithEvent` declares `public event EventHandler<PriceChange>? PriceChanged;` and
raises it in `SetPrice`. The language keeps the list of handlers; `+=` subscribes and `-=`
unsubscribes; only the class itself can raise the event. A delegate cannot change once it is
made, so raising an event calls the handlers that were there when it started — the copy that
`PriceFeed` makes by hand. `EventHandler<T>` does not require `T` to extend `EventArgs`, so
the `PriceChange` record is the event data as it is. The runner's `price-event` example
attaches the same `Chart`, `Ticker` and `PriceAlert` with `+=` and prints the same result as
the second line of `price`. It is the only class here with no Java counterpart, and the
runner prints it after the Java examples' output, so that output is unchanged.

**`IObservable<T>` and `IObserver<T>`.** .NET has a pair of interfaces in `System` with
these names. They are the .NET counterpart of `java.util.Observable`, but a different shape:
the subject pushes values with `OnNext`, and also reports an error with `OnError` and the
end of the stream with `OnCompleted`; `Subscribe` returns an `IDisposable` that
unsubscribes. They are the base of Reactive Extensions (Rx). The payment example does not
use them, because its Java uses the JDK's pull-style `Observable`, whose observers receive
the subject itself.

## Architecture

- **One class library, `Observer`**, holding every example as nested namespaces — `Price`,
  `Gof`, `Publisher`, `Payment` and `Hw`. Sources mirror namespaces:
  `src/Observer/Price/Solution/…`.
- **A console runner, `Observer.Demo`**, that runs the Java original's `main` methods — the
  price, GoF's clocks, the magazines and the invoice — and, in addition, the C# `event`
  version of the price feed and the three homework exercises, each on its own.
- `net10.0`, nullable reference types on, implicit usings on — set once in
  `Directory.Build.props` and inherited by both projects.
- **A test project, `Observer.Tests`** (xUnit), under `tests/`. See `Test.md`.

## Differences from the Java original

The port is faithful in behavior. The output of `price`, `gof` and `payment` is byte for
byte the output of the Java `main` methods. The output of `publisher` is the same apart from
the date and time, which are the current ones. The homework output is byte for byte the
output of a Java driver that makes the same calls. The whole runner prints the same under a
Turkish and a Swedish locale. What had to change:

- **Interfaces take the `I` prefix**: `IPriceListener`, `IObserver` (twice: GoF's and the
  payment one), `IPublication`, `ISubscriber`, `ITransactionListener`, `IBidListener`.
  GoF's `Subject` is an abstract class, so it keeps its name.
- **The JDK's `Observable` and `Observer` are written for the port.** .NET has no such
  class. `Payment.Observable` and `Payment.IObserver` behave like the JDK ones in the ways
  the example can see: `AddObserver` ignores an observer that is already there,
  `DeleteObserver`, a protected `SetChanged`, and `NotifyObservers()`, which does nothing
  unless the object has changed and clears the flag before it notifies. **It notifies in the
  reverse order of addition**, as the JDK code does, so the accountant is told before the
  boss, who was added first. The JDK documentation calls this order unspecified; the port
  copies what the JDK does, so the output matches. It also has `DeleteObservers`,
  `CountObservers`, `ClearChanged`, `HasChanged` and `NotifyObservers(object?)`, as the JDK
  class has, and it locks as the JDK class synchronizes.
- **Java prints a `double` with `.0`.** `Invoice` prints `balance=5000.0`, as Java does;
  C# would print `5000`. A small helper in `Invoice.ToString()` adds the `.0` to a whole
  number. It does not copy Java's switch to E notation at ten million.
- **The date has the shape of Java's `Date.toString()`**, for example
  `Thu Oct 08 21:52:22 GMT+03:00 2026`, printed with the invariant culture. Java prints a
  time zone name such as `CET` where it knows one; the port always prints the offset.
- **The abstract classes declare the interface members they leave open.** Java's
  `AbstractPublication` does not mention `publish`, and `AbstractSubscriber` does not
  mention `receive`. A C# abstract class must declare every member of its interface, so
  both are declared `abstract`. `ListSubscribers` is `virtual`, because `FourFourTwo`
  overrides it. Printing a subscriber shows its type name in C# and `Class@hash` in Java;
  the demo never lists subscribers.
- **`FourFourTwo`'s constructor is `protected internal`.** In Java, `protected` also opens
  it to the package, and `Publisher` calls it from there.
- **`Problem.PriceFeed.Price()` stays a method.** Every call counts as a read. A debugger
  that shows property values would read the price, and change the count. The solution's
  `PriceFeed.Price`, which counts nothing, is a property.
- **Accessors became properties**: `Points`, `Shown`, `Fired`, `Reads`, `Price`, `Screen`,
  `Hour`, `Minute`, `Second`, `ObserverCount`, `Name`, `Newsweek`, `FourFourTwo`, `Owner`,
  `Transactions`, `Highest`, `Leader`, `WatcherCount`, `Heard`, `Unread`. Methods that
  change something stay methods: `SetPrice`, `Subscribe`, `Tick`, `PayBalance`, `Publish`.
- **Lists are returned as `IReadOnlyList<T>`**, copied first, as the Java returns
  `List.copyOf`. Loops over listeners use `ToList()` where the Java uses `List.copyOf`.
- **Records**: `PriceChange`, `Transaction` and `Inbox.Message` are `sealed record`s with
  PascalCase members. `Transaction` overrides `ToString()` to print the Java form,
  `Transaction[owner=Deniz, kind=deposit, amount=500, balanceAfter=1500]`, because the
  homework prints it.
- **`IllegalArgumentException` is `ArgumentException`**, in `Account.Withdraw` and
  `Auction.Bid`. No parameter name is passed, so the message is the same text.
- **Number formats do not depend on the machine's locale.** The clocks format with the
  invariant culture; `String.format("%02d")` is `{0:00}`.
- **The `main` methods became `Run()` methods** called by `Observer.Demo`. The
  commented-out lines in the Java `Test` classes of `payment` and `publisher` are not
  ported. The homework demos are only in the runner.
- **Names that clash, and how they are resolved.** The namespaces `Publisher`, `Auction`
  and `Inbox` each hold a class with the same name. Inside each namespace the plain name
  means the class, because a type in the current namespace wins. In `Observer.Demo` the
  runner reaches them through aliases (`PublisherTest`, `Auction`, `Inbox`), and the
  `Main` and `Test` classes through `PriceMain`, `GofMain` and `PaymentTest`. The root
  namespace ends in `Observer`, but no type is named `Observer`: the interfaces are
  `IObserver`, so the namespace and the types never meet. `Payment.IObserver` and
  `Gof.Solution.IObserver` do not clash with `System.IObserver<T>`, because a generic type
  with one type parameter is a different name to the compiler. `Inbox.Message` stays nested,
  as in the Java. `Publisher` has properties `Newsweek` and `FourFourTwo` of type
  `IPublication` beside the classes of the same names; `new Newsweek(…)` still means the
  class, because a type name is never looked up among properties.
- **The `uml/` diagrams and the `ClassDiagram1.png`, `SequenceDiagram1.png` and `CD1.png`
  images in the Java packages are not ported yet.**

## Tests

28 xUnit tests in `tests/Observer.Tests`, ported from the Java JUnit tests. `Test.md` lists
what they check and the Java tests that are not ported.

## Run it with

The `dotnet` on `PATH` cannot build this repository — a tracked `global.json` at the root
pins the SDK to 10.0.0. Use the .NET 10 SDK directly:

```bash
cd ~/"Development/NET/Design Patterns/Design Patterns with CSharp/Observer"

# build everything
~/.dotnet/dotnet build Observer.sln

# every example, in the order the course presents them
~/.dotnet/dotnet run --project src/Observer.Demo

# one example on its own
~/.dotnet/dotnet run --project src/Observer.Demo -- price
```

The runner accepts: `price`, `price-event`, `gof`, `publisher`, `payment`,
`hw-accountlog`, `hw-auction`, `hw-inbox`.

To run the tests:

```bash
~/.dotnet/dotnet test Observer.sln
```

From the repository root, `~/.dotnet/dotnet build "Design Patterns with CSharp.sln"`
builds Observer along with every other pattern.
