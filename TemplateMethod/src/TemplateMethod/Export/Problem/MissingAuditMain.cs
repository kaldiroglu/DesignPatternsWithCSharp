namespace dev.kaldiroglu.TemplateMethod.Export.Problem;

// Inside the namespace, so that the record Export wins over the namespace Export.
using dev.kaldiroglu.TemplateMethod.Export.Domain;

/// <summary>
/// Stage three with the new Markdown format. The Markdown file is correct, but the audit log
/// has only the CSV export: <see cref="MarkdownExport"/> never calls <c>RecordAudit</c>.
/// </summary>
/// <remarks>
/// Run it with <c>dotnet run --project src/TemplateMethod.Demo -- export-problem-missing-audit</c>.
/// </remarks>
public static class MissingAuditMain
{
    public static void Run()
    {
        var audit = new AuditLog();
        new CsvExport(audit).Export(SampleReport.Deniz, SampleReport.Report);
        Console.Write("Stage three, the Markdown file:\n"
                + new MarkdownExport(audit).Export(SampleReport.Deniz, SampleReport.Report).Content);
        Console.WriteLine("Stage three, Mert asks for a Markdown file: " + SampleReport.Refusal(
            () => new MarkdownExport(audit).Export(SampleReport.Mert, SampleReport.Report)));
        Console.WriteLine("Stage three, a CSV and a Markdown export, audit log: " + Show(audit.Lines));
        Console.WriteLine("The Markdown export is missing: MarkdownExport never calls RecordAudit.");
    }

    /// <summary>Prints a list as <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
