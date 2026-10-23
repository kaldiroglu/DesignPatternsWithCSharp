namespace dev.kaldiroglu.TemplateMethod.Export.Solution;

// Inside the namespace, so that the record Export wins over the namespace Export.
using dev.kaldiroglu.TemplateMethod.Export.Domain;
using dev.kaldiroglu.TemplateMethod.Export.Problem;

/// <summary>Runs stage three and the template method on the same report, and prints both audit logs.</summary>
/// <remarks>
/// Run it with <c>dotnet run --project src/TemplateMethod.Demo -- export-audit-logs</c>.
/// </remarks>
public static class AuditLogsMain
{
    public static void Run()
    {
        var before = new AuditLog();
        new CsvExport(before).Export(SampleReport.Deniz, SampleReport.Report);
        new MarkdownExport(before).Export(SampleReport.Deniz, SampleReport.Report);
        Console.WriteLine("Stage three, two exports, audit log: " + Show(before.Lines));

        var after = new AuditLog();
        new CsvExporter(after).Export(SampleReport.Deniz, SampleReport.Report);
        new HtmlExporter(after).Export(SampleReport.Deniz, SampleReport.Report);
        new MarkdownExporter(after).Export(SampleReport.Deniz, SampleReport.Report);
        Console.WriteLine("Template method, three exports, audit log: " + Show(after.Lines));
    }

    /// <summary>Prints a list as <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
