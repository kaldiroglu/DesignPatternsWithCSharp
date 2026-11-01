*Claude Opus 5.5 (claude-opus-5-5) — Created on 2026-11-01*

# Tests — CreationalIntroduction

For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev

23 xUnit tests in one project, `tests/CreationalIntroduction.Tests`. All of them are **unit
tests**: no process boundary, no network. `SlideRunsTests` runs the demo program in the test
process, through its entry point.

The tests are a port of the Java JUnit tests in `dev.kaldiroglu.dp.creational`. Each Java test
class has one C# test class, and each ported test method has the same name in PascalCase and
the same expected values.

## What is tested

| File | Java class | Java tests | C# tests | What it checks |
|---|---|---|---|---|
| `ChapterFiguresTests.cs` | `introduction.ChapterFiguresTest` | 13 | 12 | 21 fields, 4 constructors and 21 setters in `Employee.Problem.Employee`. 5 parts holding 6, 4, 7, 3 and 1 fields. 15 fields in both connections. The better connection has no setters, no public constructor and only read-only fields. The total of 1360 passes the tolerance. Wrong totals, a term of 0 months and a short term of 36 months are rejected. `SecureLoan` has no public constructor. `LoanRecord` is a record. |
| `SlideRunsTests.cs` | `SlideRunsTest`, the "Introduction" rows | 11 | 11 | Each of the 11 runner entries returns normally and prints the lines the slide quotes. |
| **Total** | | **24** | **23** | |

`AssemblyInfo.cs` turns off parallel test runs, because `SlideRunsTests` captures `Console.Out`.

## What is not ported

- **`theEasyCreationCallsCompileAndReturnObjects`.** It checks that five JDK calls compile and
  return objects: `LocalDateTime.now()`, `Runtime.getRuntime()`, `List.of`, `Path.of` and
  `HttpClient.newBuilder()`. These are the objects on the slide "Infrastructure Objects Are
  Easy to Create". The slide quotes JDK types, and the test proves nothing about this port's
  code.

## Where C# would print a value differently

Every value below is made to match the Java output, and `SlideRunsTests` asserts the result.

| Value | C# by default | Printed here | How |
|---|---|---|---|
| A `null` string in `Employee.Problem.Employee.ToString` | empty text | `null` | `Shown` returns the word `null` |
| A `null` `Salary` (`decimal?`) | empty text | `null` | `?.ToString(...) ?? "null"` |
| A `null` database in `Db.Problem.DatabaseConnection.ToString` | empty text | `null` | `?? "null"` |
| The `bool` in `ssl=` and `remote=` | `True`, `False` | `true`, `false` | a conditional expression |
| A whole `double`, such as the total | `1120` | `1120.0` | `Bank.DoubleText.Of` |
| `LoanRecord.ToString` | `LoanRecord { Principal = 1000, Rate = 0.01, Months = 12 }` | `LoanRecord[principal=1000.0, rate=0.01, months=12]` | an override |
| `decimal` and `double` text | follows the machine's culture | invariant | `CultureInfo.InvariantCulture` |

`DoubleText.Of` covers the values these examples print. It adds `.0` to the shortest
round-trip text, and does not reproduce Java's scientific notation for values from 10^7.

`decimal` has no scale in its equality, where `BigDecimal.equals` compares the scale as well, so
`Compensation.Equals` treats `1.0m` and `1.00m` as equal. No example or test depends on this.

## Changes from the Java tests

- "Setters" counts properties with a public `set` or `init` accessor, where Java counts methods
  whose names begin with `set`.
- Instance fields are counted with reflection, which includes the backing fields of automatic
  properties. The count is the same as in Java.
- `Modifier.isFinal` is `FieldInfo.IsInitOnly`, which is true for the backing field of a
  get-only property.
- `Class.isRecord` has no equivalent. A record is recognized by its compiler-generated
  `<Clone>$` method.
- `assertEquals(BigDecimal.ZERO, ...)` is `Assert.Equal(0m, ...)`.
- `theCheckRunsBeforeTheSuperclassConstructor` is `TheCheckRunsBeforeTheBaseConstructor`. Its form
  differs because C# puts the check in the argument of `base(...)`: see `ShortTerm` in README.md.
- `IllegalArgumentException` is `ArgumentException`.

## Run it with

```bash
cd ~/"Development/NET/Design Patterns/Design Patterns with CSharp/CreationalIntroduction"
~/.dotnet/dotnet test tests/CreationalIntroduction.Tests
```
