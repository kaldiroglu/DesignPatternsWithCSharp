# Strategy — Design Patterns with C#

*Claude Opus 5.5 (claude-opus-5-5) — Created on 2026-10-02*

For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev

The C# port of the Strategy material from **Design Patterns with Java**
(`kaldiroglu/DesignPatternsWithJava`, package root
`dev.kaldiroglu.dp.behavioral.strategy`). Every class that repository carries is here, in
the same shape, under the root namespace `dev.kaldiroglu.Strategy`.

## What Strategy is for

GoF, p. 315: "Define a family of algorithms, encapsulate each one, and make them
interchangeable. Strategy lets the algorithm vary independently from clients that use it."

A context holds one algorithm behind an interface and does not know which one it holds. That
is what lets a till be put on a different campaign on Thursday morning without being
replaced, lets one desk ask four carriers for a price and compare the answers, and lets a
document change its line-breaking algorithm while it is open.

## The examples

| Namespace | What it shows |
|---|---|
| `Strategy.Gof` | GoF's own example (Design Patterns, pp. 315–316): a document that breaks text into lines. `Problem.Composition` hard-wires two algorithms behind a `bool`; `Solution` gives the `Composition` an `ICompositor`, with `SimpleCompositor`, `TeXCompositor` and `ArrayCompositor`. |
| `Strategy.Pricing` | A retailer's till, where the campaign changes every Thursday and the receipt has to say what it saved. `Domain` is the basket, the customer and the receipt. `Problem` is three naive stages — a `switch` on a string, a `switch` on an enum, a subclass per campaign — and `Till`, the reversal at stage three. `Solution` is `IPricingRule`, four rules, the `Checkout` that holds one, and the `CampaignBook` that chooses. |
| `Strategy.Sorting` | Three sorting algorithms chosen by the array's size, in three stages: `Problem` (one class that decides and implements), `Subclassing` (a subclass per algorithm) and `Pattern` (`ISorter` with a `SortingContext` that selects). |
| `Strategy.Freight` | Four carriers' rate cards — by desi, by weight band, by zone, flat — that disagree about which parcel is cheap. Solution only: `IRateCard`, `ShippingDesk` and `CarrierBoard`. |
| `Strategy.Hw` | The three homework exercises: seating a booking (`Seating`), what an overdue item costs (`LateFee`), and what a market requires of a passphrase (`Validation`). |

### Things worth stopping on

**The reversal in the checkout.** Stage three is where a careful team lands, and it works
until the store asks for the best campaign the customer qualifies for. That needs one basket
priced several ways, and at stage three the campaign is the object's class — so `Till` has to
name every campaign class to do it. With the pattern, `CampaignBook.QuoteAll` points one
`Checkout` at each rule in turn: a student's three books at 400.00 list at 1200.00 and come
to 960.00, 720.00 or 800.00, and Black Friday wins with a saving of 480.00 (asserted
upstream by `ProblemTest.theSwitchWorks` and `SolutionTest.theReceiptPromise`).

**The member that decides the interface.** `CheapestOfEveryThird` is not a percentage,
`ByWeightBand` is not a rate per kilo, `ArrayCompositor` ignores the line width, and
`GraceThenDouble` is not a daily multiplier. Each would not fit an interface shaped around
the obvious member, which is why each interface is a method.

**The branch does not disappear.** `Sorting.Pattern.SortingContext` tests the same two
thresholds `Sorting.Problem.Sorter` tests. The difference is that it selects between three
objects and implements none of them.

**The cheapest carrier is not the same carrier.** The flat rate wins the large light parcel
(89.90) and the band table wins the small heavy one (70.00), as
`FreightTest.theCheapestIsNotAlwaysTheSameCarrier` asserts upstream.

## Architecture

- **One class library, `Strategy`**, holding every example as nested namespaces — `Gof`,
  `Pricing`, `Sorting`, `Freight` and `Hw`. Sources mirror namespaces:
  `src/Strategy/Pricing/Solution/…`.
- **A console runner, `Strategy.Demo`.** The Java original has no `main` methods for
  Strategy; its figures are asserted by tests. The runner prints the same figures from the
  same inputs, one example at a time or all of them in the order the course presents them.
- `net10.0`, nullable reference types on, implicit usings on — set once in
  `Directory.Build.props` and inherited by both projects.
- There is no test project yet.

## Differences from the Java original

The port is faithful in behavior: the figures the runner prints are the ones the Java tests
assert, and the output is the same under a Turkish locale. What had to change:

- **Interfaces take the `I` prefix**, as the rest of this solution does: `ICompositor`,
  `IPricingRule`, `ISorter`, `IRateCard`, `IFeeRule`, `ISeatingPolicy`, `IPassphraseRule`.
- **Accessors became properties**: every `name()` is `Name`, `carrier()` is `Carrier`, and
  `ruleName()`, `compositorName()`, `policyName()`, `lastUsed()`, `size()`, `ruleCount()`,
  `threshold()` and `campaignsNamedHere()` follow. So do the parameterless derived values:
  `Basket.ListTotal`, `Basket.ItemCount`, `Line.ListTotal`, `Receipt.Saved`,
  `Layout.LineCount`, `Layout.WorstSlack`, and `Shipment`'s `VolumeCm3`, `DesiGrams`,
  `ChargeableGrams` and `IsDomestic`. Stage three's protected `campaignName()` is the
  protected property `CampaignName`. Setters stay methods — `SetRule`, `SetCard`,
  `SetCompositor`, `SetPolicy`, `SetFuelSurchargePercent` — as `SetCommand` does in the
  Command port. Methods that hand back a list — `Render()`, `InCategory()`, `QuoteAll()`,
  `Free()`, `All()`, `Allocate()`, `Seat()`, `Complaints()` — return an `IReadOnlyList<T>`.
- **`Customer`'s flags are `IsStudent` and `IsStaff`.** Java's record has both a `student()`
  accessor and a static `student(String)` factory; C# cannot give a property and a method the
  same name, so the factories keep the names (`Customer.Student("Ceyda")`) and the flags take
  the `Is` prefix.
- **Records stay records**, with Java's compact-constructor checks moved into property
  initializers (`Line.Quantity`, `Shipment.Grams`, `Loan.DaysLate`) and list components
  copied on construction. A C# record compares a list component by reference where Java's
  compares it by content; nothing in the examples compares two baskets, layouts or seat plans.
- **`BigDecimal` became `decimal`.** Both `Money` types round half away from zero, which is
  Java's `HALF_UP`, and print two decimal places in the invariant culture. Freight's
  `Times(double)` reads the `double` through its shortest round-trip text, as
  `BigDecimal.valueOf(double)` does.
- **`Predicate<Basket>` became `Func<Basket, bool>`** in `PercentageOff`.
- **The stage-two enum keeps Java's constant names** (`BLACK_FRIDAY`, not `BlackFriday`),
  because `EnumCheckout` prints the constant's name on the receipt and the receipt has to
  read the same in both languages.
- **The exhaustive `switch` needed two adjustments.** A C# enum may hold a value no constant
  names, so a switch with every constant covered still draws warning CS8524; `EnumCheckout`
  suppresses that one warning and has no default case. A missing constant is warning CS8509
  in C# where Java refuses to compile, so `Strategy.csproj` raises CS8509 to an error — adding
  a constant to `Campaign` breaks the build until the branch is written, as it does in Java.
- **The fifth branch's loop moved into a private helper** in `SwitchingCheckout` and
  `EnumCheckout`. Java's switch expression can hold a block ending in `yield`; C#'s cannot.
- **`JavaSorter` keeps its name and reports `JavaSort`**, so all three sorting stages report
  the same three names, but the library it hands the array to is `Array.Sort`.
- **Sorting is stable where Java's is.** `Basket.InCategory` and `ByWeightBand` sort with
  LINQ's `OrderBy`, which is stable as Java's `List.sort` and `Stream.sorted` are;
  `List<T>.Sort` is not.
- **`NoCommonWords` lower-cases with `ToLowerInvariant`.** Java's `toLowerCase()` uses the
  default locale, under which a Turkish machine turns `ADMIN` into `admın` and lets it past
  the banned list.
- **`IllegalArgumentException` is `ArgumentException`, and `Objects.requireNonNull` is
  `ArgumentNullException`.**
- **The namespace for `hw.latefee` is `Hw.LateFee`.**
- **The `uml/` diagrams in the Java packages are not ported yet.**

## Run it with

The `dotnet` on `PATH` cannot build this repository — a tracked `global.json` at the root
pins the SDK to 10.0.0. Use the .NET 10 SDK directly:

```bash
cd ~/"Development/NET/Design Patterns/Design Patterns with CSharp/Strategy"

# build everything
~/.dotnet/dotnet build

# every example, in the order the course presents them
~/.dotnet/dotnet run --project src/Strategy.Demo

# one example on its own
~/.dotnet/dotnet run --project src/Strategy.Demo -- freight
```

The runner accepts: `gof`, `pricing-problem`, `pricing-solution`, `sorting`, `freight`,
`hw-seating`, `hw-latefee`, `hw-validation`.

There are no tests to run yet.

From the repository root, `~/.dotnet/dotnet build "Design Patterns with CSharp.sln"`
builds Strategy along with every other pattern.
