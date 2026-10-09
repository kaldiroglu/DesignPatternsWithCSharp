*Claude Opus 5.5 (claude-opus-5-5) — Created on 2026-10-09*

# Tests — Strategy

For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev

51 xUnit tests in one project, `tests/Strategy.Tests`. All of them are **unit tests**: no
process boundary, no network. Four tests read C# source files of the library. They find the
files from the test file's own path (`CallerFilePath`), so they do not depend on the working
directory.

The tests are a port of the Java JUnit tests in `dev.kaldiroglu.dp.behavioral.strategy`.
Each Java test class has one C# test class (the name ends in `Tests`, as in the rest of this
repository), and each Java test method has a `[Fact]` with the same name in PascalCase, the
same display name, the same assertions and the same expected values. Every Java test is
ported; none is left out.

## What is tested

| File | Java class | Java tests | C# tests | What it checks |
|---|---|---|---|---|
| `Pricing/ProblemTests.cs` | `pricing.ProblemTest` | 8 | 8 | The three naive designs price the student basket at 1200.00, 960.00, 720.00 and 800.00. Five string arms in one method. One enum arm per `Campaign` constant and no discard arm. The reversal: `Till` names 3 campaign classes, and Black Friday saves 480.00. |
| `Pricing/SolutionTests.cs` | `pricing.SolutionTest` | 10 | 10 | The same prices from the rules. A rule replaced on a till that exists. One till prices five campaigns. A sixth campaign is one class. `Checkout` has no type test, no `switch` and no campaign name. Four rule classes, five campaigns, two interface members. |
| `Gof/CompositorTests.cs` | `gof.CompositorTest` | 7 | 7 | GoF's paragraph at 26 columns: four lines each, worst gap 8 against 5. `ArrayCompositor` makes a first row 33 wide. The naive class takes a `bool`; the context takes an `ICompositor` and holds no `bool`. |
| `Sorting/SortingTests.cs` | `sorting.SortingTest` | 5 | 5 | The three designs sort the same way and choose at the same sizes (99, 100, 1,000,000). The context tests the same two thresholds but contains no `Partition(`. |
| `Freight/FreightTests.cs` | `freight.FreightTest` | 10 | 10 | The pillow is 20 desi; the books are 4.2 kg. The four quotes for each parcel. The flat rate wins one parcel and the band table the other. A fifth carrier is one class. The desk holds only an `IRateCard`. |
| `Hw/HomeworkTests.cs` | `hw.HomeworkTest` | 11 | 11 | The three homework solutions, as three nested classes like Java's `@Nested` ones: seating (3), late fees (4), passphrase rules (4). |
| **Total** | | **51** | **51** | |

`SourceText.cs` holds the helpers for the source-text tests: `Read` reads a file under
`src/Strategy`, `StripComments` removes comments (the classes name in their comments what
they leave out, so a search over the raw file matches its own comments), `From` cuts the
text from a marker, and `CountOf` counts a substring.

## Changes from the Java tests

No expected value was changed. Where Java and C# differ, the check was moved to the closest
C# form:

- **Anonymous classes became nested classes.** Java writes a new campaign
  (`NEW_YEAR`), a new sorter (`InsertionSort`) and a new carrier (`Kurye`) as anonymous
  classes inside the test. C# has no anonymous class that implements an interface, so each
  is a small private nested class in the test file.
- **Exceptions.** `IllegalArgumentException` is `ArgumentException`, and the
  `NullPointerException` from `new Checkout(null)` is `ArgumentNullException`.
- **Source checks on a `switch`.** Java counts `case "` in `SwitchingCheckout` and `case `
  and `default ->` in `EnumCheckout`. The C# ports use switch expressions, which have no
  `case` keyword, so the tests count the arms instead: a string literal followed by `=>`
  (5), `Campaign.X =>` (one per constant), and the discard arm `_ =>` (0).
- **Type tests.** Java checks that `Checkout` contains no `instanceof`; the C# test checks
  that it contains neither ` is ` nor ` as `.
- **Names in the source.** `rule.priceFor` is `_rule.PriceFor`, `BUBBLE_LIMIT` and
  `QUICK_LIMIT` are `BubbleLimit` and `QuickLimit`, `return bubbleSorter` is
  `return _bubbleSorter`, and `quicksort(`/`partition(` are `Quicksort(`/`Partition(`.
- **The interface has two methods.** Java counts `name()` and `priceFor()`. In C# `Name` is
  a property; its getter `get_Name` is a method, so `GetMethods()` still counts two.
- **Fields are read with `DeclaredOnly`** and all access levels, which is what Java's
  `getDeclaredFields()` returns.
- **The random arrays differ.** `SortingTests` uses `new Random(42)` as Java does, but .NET
  and Java produce different numbers from the same seed. Every sorting test compares C#
  results with other C# results, so no expected value depends on the numbers.
- **Unmodifiable and empty lists.** `assertEquals(List.of(), …)` is `Assert.Empty`.

## Run it with

```bash
cd ~/"Development/NET/Design Patterns/Design Patterns with CSharp/Strategy"

# all 51
~/.dotnet/dotnet test Strategy.sln

# one class
~/.dotnet/dotnet test Strategy.sln --filter "FullyQualifiedName~FreightTests"

# one test
~/.dotnet/dotnet test Strategy.sln --filter "FullyQualifiedName~TheReversal"
```

`dotnet` on `PATH` cannot build `net10.0`; use `~/.dotnet/dotnet`.
