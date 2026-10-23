namespace dev.kaldiroglu.TemplateMethod.Export.Problem;

// Inside the namespace, so that the record Export wins over the namespace Export.
using dev.kaldiroglu.TemplateMethod.Export.Domain;

/// <summary>Stage one: each format has its own copy of the algorithm. Both exports are audited.</summary>
/// <remarks>
/// Run it with <c>dotnet run --project src/TemplateMethod.Demo -- export-problem-stage-one</c>.
/// </remarks>
public static class StageOneMain
{
    public static void Run()
    {
        var audit = new AuditLog();
        new StandaloneCsvExport(audit).Export(SampleReport.Deniz, SampleReport.Report);
        new StandaloneHtmlExport(audit).Export(SampleReport.Deniz, SampleReport.Report);
        Console.WriteLine("Stage one, a copy per format, audit log: " + Show(audit.Lines));
    }

    /// <summary>Prints a list as <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
