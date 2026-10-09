*Claude Opus 5.5 (claude-opus-5-5) — Created on 2026-10-09*

# Tests — Template Method

For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev

35 xUnit tests in one project, `tests/TemplateMethod.Tests`. They are ported from the Java
JUnit tests in `dev.kaldiroglu.dp.behavioral.templateMethod`: one C# test class for each
Java test class, and one C# test for each Java test, with the same name in PascalCase and
the same expected values. All of them are **unit tests**: no files, no network, no other
process. One test is slow on purpose (see below).

## What is tested

| File | Java | C# | What it checks |
|---|---|---|---|
| `Export/ProblemTests.cs` | 8 | 8 | The three naive stages of the report export. Stage one copies the permission check and the audit line into every class; stage two has three switches on the format; in stage three the Markdown export never calls `RecordAudit`, so the audit log has one line after two exports. |
| `Export/SolutionTests.cs` | 7 | 7 | The template method: three exports, three audit lines; `Export` is not `virtual`; `Header`, `Row` and `Extension` are `protected abstract`; `Footer` is a hook; the subclasses never mention the permission or the audit; the exact output of `Export.Solution.Main.Run()`. |
| `Gof/ApplicationTests.cs` | 7 | 7 | GoF's applications before and after the pattern produce the same events; `OpenDocument` is not `virtual`; only the spreadsheet overrides the hook; a `null` document stops the algorithm; the exact output of `Gof.Main.Run()`. |
| `Task/TaskTests.cs` | 6 | 6 | The repeated task: ten prints with nine one-second waits, the order of the steps, an interrupt, a failing task that skips `Clean`, and which methods each task overrides. |
| `Hw/HomeworkTests.cs` | 7 | 7 | The three homework solutions: call centers and the private `Verify` step, the onboarding equipment hook, and the record file reader that closes its reader when a line fails. |
| **Total** | **35** | **35** | |

The `Pattern` namespace (the short `Application` and `Document` version) has no Java test,
so it has no C# test either.

Helpers in the test root:

- `Printed.By(action)` captures `Console.Out` with a `StringWriter` and returns the printed
  lines. It restores the console in `finally`.
- `Code` reads the library's C# sources with comments removed, counts text in them, and
  lists the methods a type declares. It finds the source folder from its own path with
  `CallerFilePath`, so the tests do not depend on the working folder.
- `AssemblyInfo.cs` turns off parallel test runs, because several tests capture the console.

### The slow test

`TaskTests.TheClientPrintsTenTimesWithNineWaits` runs `Task.Test.Run()`: a print task ten
times, one second apart. It checks that the run takes at least 9,000 ms and less than
10,000 ms. It is the only slow test; the whole run takes about ten seconds.

## Where the C# checks differ from the Java

Every Java test is ported. Some checks are written differently, because C# has no direct
equivalent:

- **Java's `final` is "not `virtual`" in C#.** `Modifier.isFinal(...)` became
  `Assert.False(method.IsVirtual)` in `SolutionTests.TheTemplateMethodIsFinal`,
  `ApplicationTests.TheKindsOfMethod`, `TaskTests.TheKindsOfMethod` and
  `HomeworkTests.VerificationCannotBeSkipped`.
- **Stage two's switches are switch expressions.** The Java counts three `switch (format)`
  statements and six `case ` labels. The C# counts three `format switch` expressions and the
  arms that name a format (`Format.Csv =>`, `Format.Html =>`), which must be three times the
  number of `Format` values. The `_ =>` arm that throws is not counted, because it names no
  format.
- **Source-text needles use the C# names**: `user.MayExport`, `audit.Record(`,
  `RecordAudit(` and `MayExport`, in the `.cs` files.
- **The interrupt test checks the opposite of the Java's flag.**
  `TaskTests.AnInterruptStopsTheLoopAndCleansUp` checks, as in Java, that one fax is sent
  and `Clean` still runs. The Java test also checks that the thread keeps its interrupt
  flag. .NET has no interrupt flag: the interrupt is used up when `Thread.Sleep` throws
  `ThreadInterruptedException` (the README says this too). So the C# test checks that the
  thread can sleep again without an exception. It runs on its own thread, so the
  interrupt cannot reach the test runner's thread.
- **Anonymous subclasses became private nested classes**: `NoFooterExporter`,
  `FailingTask`, `RefusingApplication` and `TrackingReader`.
- **Exception types**: `IllegalStateException` is `InvalidOperationException`, and
  `IllegalArgumentException` is `ArgumentException`.
- **The tracking reader overrides `Dispose(bool)`**, not `close()`. In .NET both `Close()`
  and `Dispose()` end in `Dispose(bool)`, and `RecordFileReader` closes its reader with
  `using`.

## Run it with

```bash
cd ~/"Development/NET/Design Patterns/Design Patterns with CSharp/TemplateMethod"

# all 35 (about ten seconds)
~/.dotnet/dotnet test TemplateMethod.sln

# one class
~/.dotnet/dotnet test tests/TemplateMethod.Tests --filter "FullyQualifiedName~SolutionTests"

# everything except the slow test
~/.dotnet/dotnet test tests/TemplateMethod.Tests --filter "FullyQualifiedName!~TheClientPrintsTenTimesWithNineWaits"
```

`dotnet` on `PATH` cannot build `net10.0`; use `~/.dotnet/dotnet`.
