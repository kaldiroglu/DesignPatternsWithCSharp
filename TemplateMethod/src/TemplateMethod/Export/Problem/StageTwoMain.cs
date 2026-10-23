namespace dev.kaldiroglu.TemplateMethod.Export.Problem;

// Inside the namespace, so that the record Export wins over the namespace Export.
using dev.kaldiroglu.TemplateMethod.Export.Domain;

/// <summary>Stage two: one class switches on the format. Both exports are audited.</summary>
/// <remarks>
/// Run it with <c>dotnet run --project src/TemplateMethod.Demo -- export-problem-stage-two</c>.
/// </remarks>
public static class StageTwoMain
{
    public static void Run()
    {
        var audit = new AuditLog();
        var exporter = new SwitchingExporter(audit);
        exporter.Export(SampleReport.Deniz, SampleReport.Report, Format.Csv);
        exporter.Export(SampleReport.Deniz, SampleReport.Report, Format.Html);
        Console.WriteLine("Stage two, one class with a switch, audit log: " + Show(audit.Lines));
    }

    /// <summary>Prints a list as <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
