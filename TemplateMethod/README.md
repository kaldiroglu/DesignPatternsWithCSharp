# Template Method — Design Patterns with C#

*Claude Opus 5.5 (claude-opus-5-5) — Created on 2026-10-08*

For further enquiry please contact Akin Kaldiroglu at akin@kaldiroglu.dev

The C# port of the Template Method material from **Design Patterns with Java**
(`kaldiroglu/DesignPatternsWithJava`, package root
`dev.kaldiroglu.dp.behavioral.templateMethod`). Every class that repository carries is here,
in the same shape, under the root namespace `dev.kaldiroglu.TemplateMethod`.

## What Template Method is for

A base class writes an algorithm once, as one method that calls its steps in a fixed order.
Subclasses write some of the steps, but they cannot change the order or leave a step out.

That is what makes every report export check the permission first and write the audit line
last, whichever format a team adds later. It is also what makes every call center's import
verify the audio, and what makes a record file reader close its file even when a line is
bad.

## The examples

| Namespace | What it shows |
|---|---|
| `TemplateMethod.Export` | The main worked example. A reporting tool exports the same report as CSV, HTML or Markdown. `Domain` holds `Report`, `Sale`, `User`, `Export`, `AuditLog` and `ExportNotAllowedException`. `Problem` is three naive stages: a full copy per format (`StandaloneCsvExport`, `StandaloneHtmlExport`), one class with a switch on `Format` (`SwitchingExporter`), and a base class with shared helpers where each format writes its own `Export()` (`ExportSupport`, `CsvExport`, `MarkdownExport`). `Solution` has `ReportExporter`, whose `Export()` is the template method, and `CsvExporter`, `HtmlExporter`, `MarkdownExporter`. |
| `TemplateMethod.Gof` | GoF's own example (Design Patterns, pp. 325–330). `Problem` has two applications that each write the whole of `OpenDocument`. `Solution` has `Application` with the template method `OpenDocument`, the primitive operation `CanOpenDocument`, the factory method `DoCreateDocument` and the hook `AboutToOpenDocument`, and `Document` with the primitive operation `DoRead`. |
| `TemplateMethod.Pattern` | The same example in a short form: `Application.OpenDocument` checks, creates and adds a document. `MyApplication` writes the two abstract steps. |
| `TemplateMethod.Task` | A task that repeats: `Task.Run()` is the template method, `DoTask()` is abstract, and `Prepare`, `Before`, `After` and `Clean` are hooks with a default. `Fax` and `Scan` write `DoTask()` only; `Print` also overrides `Prepare` and `Clean`. |
| `TemplateMethod.Hw` | The three homework exercises: importing call records from three call centers (`CallCenter.CallImport`), the first day of an employee or a contractor (`Onboarding.Onboarding`), and reading a file of records (`RecordFile.RecordFileReader<T>`). |

### Things worth stopping on

**Where stage three fails.** `MarkdownExport` was written later and never calls
`RecordAudit`. It compiles and makes a correct file, and its exports are missing from the
audit log: the demo prints one audit line for two exports. The steps were shared; the
algorithm was not. In the solution the audit line is written by the template method, and
the demo prints three audit lines for three exports.

**A hook is a step with a default.** `ReportExporter.Footer()` returns an empty string, and
only `HtmlExporter` overrides it, to close the table. `SpreadsheetApplication` uses GoF's
hook `AboutToOpenDocument` to remember the last file; in the `Problem` version it added the
same step in its own copy of the algorithm, in a place it chose.

**The template method can give a guarantee.** `CallImport.Run()` always verifies the audio,
so Ankara's short recording is rejected. `RecordFileReader<T>.ReadAll()` always closes the
reader, even when `Parse` throws, because a subclass never sees the reader.

## The C# form of `final`: a method that is not `virtual`

The Java template methods are `final`, so no subclass can override them. **In C# a method
is not virtual unless it says so.** The template method is therefore a plain public method
with no `virtual` on it — `ReportExporter.Export`, `Application.OpenDocument`, `Task.Run`,
`CallImport.Run`, `Onboarding.Start` and `RecordFileReader<T>.ReadAll` — and a subclass
cannot override it. The steps a subclass must write are `protected abstract`, and the hooks
are `protected virtual` with a default body.

The deck says "final/sealed". In C#, `sealed` is for a class, or for `sealed override` on a
method that overrides a virtual one; a new method that is not `virtual` is already closed,
so it needs no keyword at all.

Note the difference in direction. In Java every method can be overridden unless it is
marked `final`, so the template method must be marked. In C# a method can be overridden only
when it is marked `virtual`, so the hooks and the steps must be marked, and the template
method is left as it is.

## Architecture

- **One class library, `TemplateMethod`**, holding every example as nested namespaces —
  `Export`, `Gof`, `Pattern`, `Task` and `Hw`. Sources mirror namespaces:
  `src/TemplateMethod/Export/Solution/…`.
- **A console runner, `TemplateMethod.Demo`**, that runs the Java original's `main`
  methods — the export, GoF's applications, the short form and the task — and, in addition,
  the three homework exercises, each on its own.
- `net10.0`, nullable reference types on, implicit usings on — set once in
  `Directory.Build.props` and inherited by both projects.
- There is no test project, as requested.

## Differences from the Java original

The port is faithful in behavior. The output of `export`, `gof`, `pattern` and `task` is
byte for byte the output of the Java `main` methods. The homework has no `main` in Java, so
its output was compared with a short Java program that calls the Java classes and prints the
same lines; those are identical too. The whole runner prints the same under a Turkish
locale. What had to change:

- **`final` became "not `virtual`".** See the section above. Each template method's doc
  comment says this where the Java one says `final`.
- **The task demo does not wait.** Java's `Test` runs a print task ten times with a
  one-second interval: nine waits, about nine seconds. `Task.Test.Run()` does the same; the
  runner calls `Task.Test.Run(interval: 0)` instead, which prints the same lines without the
  waits. The `Run(int)` overload exists only for that. `Task.Run()` itself waits
  `Interval` seconds only between repetitions, as the Java does.
- **Fields became properties.** The Java protected fields `name`, `interval` and
  `repetition` are protected properties `Name`, `Interval` and `Repetition`.
- **An interrupt stops the loop.** Java catches `InterruptedException`, keeps the interrupt
  flag and stops repeating. C# catches `ThreadInterruptedException` and stops repeating;
  .NET has no interrupt flag to keep. In both, `Clean()` still runs.
- **The task's hooks stay public.** In Java `prepare`, `before`, `after`, `clean` and
  `doTask` are `public`, so in C# they are `public virtual` and `public abstract`, not
  `protected`.
- **The short form keeps its open methods.** In `pattern`, the Java methods have package
  access and are not `final`. In C# they are `internal`, and `OpenDocument` and
  `AddDocument` are `internal virtual`, so a subclass could still override them, as in the
  Java. The `Gof` namespace has the closed form. The Java `Document`'s protected field
  `name` with its getter `getName()` is the property `Name`, with a protected setter.
- **GoF's `Document.DoRead` is `protected internal`.** `Application.OpenDocument` calls it,
  and `Application` is not a subclass of `Document`. In Java `protected` also opens a member
  to its package; C#'s `protected` does not, and `protected internal` opens it to the
  assembly. C# has nothing closer.
- **`RecordFileReader<T>` reads a `TextReader` inside `using`.** Java wraps the `Reader` in
  a `BufferedReader` inside try-with-resources; `TextReader.ReadLine()` needs no wrapper,
  and `using (source)` closes it on every path, including when `Parse` throws. There is no
  `throws IOException`, because C# has no checked exceptions. `isBlank()` is
  `string.IsNullOrWhiteSpace`.
- **`CustomerFileReader` splits the way Java does.** Java's `split(";")` drops empty parts
  at the end of the line, so `"Ayse;"` is one part and an error. C#'s `Split(';')` keeps
  them, so a small private method removes them.
- **The record `Export` is reached by a `using` inside the namespace.** The namespace
  `dev.kaldiroglu.TemplateMethod.Export` and the record `Export.Domain.Export` share a name.
  Without help, the name `Export` in `Export.Problem` and `Export.Solution` would find the
  namespace first. A `using` written after the file-scoped `namespace` line is searched
  before the enclosing namespaces, so `Export` means the record there.
- **The task's namespace and class are both `Task`.** Inside
  `dev.kaldiroglu.TemplateMethod.Task`, the name `Task` means the class, not
  `System.Threading.Tasks.Task`, because a type in the current namespace wins over one
  brought in by an implicit `using`. Nothing in the library is asynchronous.
- **Accessors became properties**: `AuditLog.Lines`, `Application.Documents` and
  `Application.Events`, `Document.Name` and `Document.IsOpen`, `CallImport.Stored` and
  `CallImport.Rejected`. Record components are PascalCase: `Report.Title`, `Sale.Amount`,
  `User.MayExport`, `CallRecord.Id`, `Recording.CallId`, `Customer.City` and so on.
- **Lists are `IReadOnlyList<T>`.** Where Java returns `List.copyOf(…)`, the port returns a
  read-only copy. `Report` copies its sales in the constructor, as the Java record does. One
  small difference: two Java `Report` records with equal sales are equal; two C# `Report`
  records are equal only when they share the same list object, because a C# record compares
  a list by reference. No example compares reports.
- **`Format`'s constants are `Csv` and `Html`**, in C# casing, and each `switch` has a
  `_ =>` arm that throws. Java's switch over an enum is exhaustive without one; C#'s is not,
  because an enum variable can hold any integer.
- **Exceptions.** `ExportNotAllowedException` extends `Exception` (Java: `RuntimeException`),
  and `IllegalArgumentException` is `ArgumentException`.
- **Nullable types say what Java leaves unsaid.** `DoCreateDocument` returns `Document?`,
  because `OpenDocument` checks for `null`, and `RetrieveAudio` returns `Recording?`,
  because `Verify` does.
- **`endsWith` is `EndsWith(…, StringComparison.Ordinal)`**, which compares the way Java
  does on every locale.
- **The `main` methods became `Run()` methods** called by `TemplateMethod.Demo`, and the
  runner prints lists as `[a, b, c]`, as Java's `List.toString()` does. The homework demos
  are only in the runner.
- **The `uml/` diagrams and the `CD1.png` / `ClassDiagram1.png` images in the Java packages
  are not ported yet.**

## Run it with

The `dotnet` on `PATH` cannot build this repository — a tracked `global.json` at the root
pins the SDK to 10.0.0. Use the .NET 10 SDK directly:

```bash
cd ~/"Development/NET/Design Patterns/Design Patterns with CSharp/TemplateMethod"

# build everything
~/.dotnet/dotnet build TemplateMethod.sln

# every example, in the order the course presents them
~/.dotnet/dotnet run --project src/TemplateMethod.Demo

# one example on its own
~/.dotnet/dotnet run --project src/TemplateMethod.Demo -- export
```

The runner accepts: `export`, `gof`, `pattern`, `task`, `hw-callcenter`, `hw-onboarding`,
`hw-recordfile`.

There are no tests to run.

From the repository root, `~/.dotnet/dotnet build "Design Patterns with CSharp.sln"`
builds Template Method along with every other pattern.
