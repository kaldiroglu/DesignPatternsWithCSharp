*Claude Opus 5.5 (claude-opus-5-5) — Created on 2026-11-01*

# CreationalIntroduction — C# (.NET)

For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev

**Project:** Design Patterns course — the chapter before the five creational patterns (C#
implementation)
**Created:** 2026-11-01
**Source:** a port of the Java package `dev.kaldiroglu.dp.creational.introduction` in
`DesignPatternsWithJava`. The deck quotes the Java; this port runs the same examples and
prints the same lines.

The folder is named `CreationalIntroduction`, not `Introduction`, because `Introduction`
already exists and ports a different package, the course introduction
(`dev.kaldiroglu.dp.introduction`).

## Expected benefit

The examples show why creating an object is harder than using it. No pattern is taught here.
A C# audience can run what the slides show, one run for each slide.

## The examples

| Namespace | What it shows |
|---|---|
| `Employee.Problem` | 21 properties: assignments leave properties empty, and swapped arguments compile |
| `Employee.Better` | The same employee made of five smaller objects |
| `Db.Problem` | A connection with settable properties accepts a missing database |
| `Db.Better` | The same connection refuses to be created without one |
| `Bank` | One invariant: setters, a constructor that checks, an object that calculates, a record |

## Architecture

One class library (`src/CreationalIntroduction`), one console runner
(`src/CreationalIntroduction.Demo`) and one xUnit project
(`tests/CreationalIntroduction.Tests`). Folders mirror namespaces under the root namespace
`dev.kaldiroglu.CreationalIntroduction`. Class diagrams sit in `uml/` folders beside the code;
`render-uml.sh` renders them.

## Notes on the port

- **`ShortTerm`.** Java 25 lets the constructor run an `if` before `super(months)`. C# has no
  such form: a constructor body runs after the base constructor, and no statement can come
  before `base(...)`. The argument of `base(...)` is evaluated first, so the check sits there
  as a `throw` expression. The order of events is the same: the check runs first, and `Term`
  never runs for a bad value. What differs is the form: one expression, not a statement.
- **`DatabaseConnection.CreateBuilder`** is `builder` in Java. A C# class cannot have a method
  and a nested type with the same name, so the method cannot be called `Builder`.
- **`Builder`'s fields and constructor are `internal`**, where Java has them `private`. In C#
  an enclosing class cannot read the private members of its nested class. Code outside this
  assembly still has only `CreateBuilder`; code inside it could call the constructor directly.
- **Setters are settable properties**, and getters are properties, following the repository.
  `newGrad.setFirstName("Sarah")` is `newGrad.FirstName = "Sarah"`, and `Employee.Problem`
  therefore has public getters as well, where Java has only two.
- **`LoanRecord` is a `sealed record` that declares its constructor.** A positional record has
  no constructor body, so it has no place for the three checks. The properties have no `init`
  accessor, so `with` cannot skip them. `totalRepayment()` is the property `TotalRepayment`.
- **`LoanRecord.ToString` is overridden** to print `LoanRecord[principal=1000.0, rate=0.01,
  months=12]`, because the default record text has a different form.
- **`ArgumentException`** replaces `IllegalArgumentException`. Each is created with its message
  only, because a parameter name would add text to `Message`.
- **Types:** `LocalDate` is `DateOnly`, and `BigDecimal` is `decimal`. Test.md lists every place
  where C# would print a value differently.
- `Employee` is both a namespace and a class here. `Employee.Problem.Employee` works because
  the class sits one level below the namespace of the same name.

## Run it with

```bash
cd ~/"Development/NET/Design Patterns/Design Patterns with CSharp/CreationalIntroduction"

~/.dotnet/dotnet test tests/CreationalIntroduction.Tests
~/.dotnet/dotnet run --project src/CreationalIntroduction.Demo                     # every example
~/.dotnet/dotnet run --project src/CreationalIntroduction.Demo -- bank-secure-loan # or one
./render-uml.sh && python3 ../tools/redraw-diagrams.py src/CreationalIntroduction  # the diagrams
```

The entries are `employee-problem-setters`, `employee-problem-constructors`,
`employee-problem-swapped-arguments`, `employee-better`, `db-problem`, `db-better`,
`bank-setters`, `bank-constructor-check`, `bank-secure-loan`, `bank-loan-record` and
`bank-short-term`.
