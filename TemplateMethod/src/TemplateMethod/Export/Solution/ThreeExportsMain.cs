namespace dev.kaldiroglu.TemplateMethod.Export.Solution;

// Inside the namespace, so that the record Export wins over the namespace Export.
using dev.kaldiroglu.TemplateMethod.Export.Domain;

/// <summary>Exports the report in the three formats and prints each file.</summary>
/// <remarks>
/// Run it with <c>dotnet run --project src/TemplateMethod.Demo -- export-three-exports</c>.
/// </remarks>
public static class ThreeExportsMain
{
    public static void Run()
    {
        var audit = new AuditLog();
        foreach (ReportExporter exporter in new ReportExporter[]
                 {
                     new CsvExporter(audit), new HtmlExporter(audit), new MarkdownExporter(audit)
                 })
        {
            Export export = exporter.Export(SampleReport.Deniz, SampleReport.Report);
            Console.WriteLine("--- " + export.FileName);
            Console.Write(export.Content);
        }
    }
}
