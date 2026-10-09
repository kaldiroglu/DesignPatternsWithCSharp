namespace dev.kaldiroglu.TemplateMethod.Export.Problem;

// Inside the namespace, so that the record Export wins over the namespace Export.
using dev.kaldiroglu.TemplateMethod.Export.Domain;

/// <summary>
/// Runs the three stages on the same report. The first two audit every export; in stage
/// three the Markdown export never writes its audit line.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/TemplateMethod.Demo -- export-problem</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var report = new Report("sales-october", [new Sale("Ayse", 1200), new Sale("Deniz", 800)]);
        var deniz = new User("Deniz", true);

        var one = new AuditLog();
        new StandaloneCsvExport(one).Export(deniz, report);
        new StandaloneHtmlExport(one).Export(deniz, report);
        Console.WriteLine("Stage one, a copy per format, audit log: " + Show(one.Lines));

        var two = new AuditLog();
        var exporter = new SwitchingExporter(two);
        exporter.Export(deniz, report, Format.Csv);
        exporter.Export(deniz, report, Format.Html);
        Console.WriteLine("Stage two, one class with a switch, audit log: " + Show(two.Lines));

        var three = new AuditLog();
        new CsvExport(three).Export(deniz, report);
        Console.Write("Stage three, the Markdown file:\n"
                + new MarkdownExport(three).Export(deniz, report).Content);
        Console.WriteLine("Stage three, a CSV and a Markdown export, audit log: " + Show(three.Lines));
        Console.WriteLine("The Markdown export is missing: MarkdownExport never calls RecordAudit.");
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
