namespace dev.kaldiroglu.TemplateMethod.Export.Solution;

// Inside the namespace, so that the record Export wins over the namespace Export.
using dev.kaldiroglu.TemplateMethod.Export.Domain;
using dev.kaldiroglu.TemplateMethod.Export.Problem;

/// <summary>Runs stage three and the solution on the same report, and prints both audit logs.</summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/TemplateMethod.Demo -- export</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var report = new Report("sales-october", [new Sale("Ayse", 1200), new Sale("Deniz", 800)]);
        var deniz = new User("Deniz", true);

        var before = new AuditLog();
        new CsvExport(before).Export(deniz, report);
        new MarkdownExport(before).Export(deniz, report);
        Console.WriteLine("Stage three, two exports, audit log: " + Show(before.Lines));

        var after = new AuditLog();
        foreach (ReportExporter exporter in new ReportExporter[]
                 {
                     new CsvExporter(after), new HtmlExporter(after), new MarkdownExporter(after)
                 })
        {
            Export export = exporter.Export(deniz, report);
            Console.WriteLine("--- " + export.FileName);
            Console.Write(export.Content);
        }
        Console.WriteLine("Template method, three exports, audit log: " + Show(after.Lines));
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
