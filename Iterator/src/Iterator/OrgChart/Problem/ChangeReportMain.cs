namespace dev.kaldiroglu.Iterator.OrgChart.Problem;

/// <summary>
/// HR compares two charts. A callback walks one chart at a time, so the report copies both
/// charts first.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Iterator.Demo -- orgchart-change-report</c>.
/// </remarks>
public static class ChangeReportMain
{
    public static void Run()
    {
        var before = SampleCompany.Build("Deniz");
        // three people are new after the reorganization
        var after = SampleCompany.Build("Zeynep", "Burak", "Ece");
        var report = new ChangeReport();
        Console.WriteLine("HR compares two charts: "
            + (report.FirstDifference(before, after) ?? "none"));
        Console.WriteLine("All changes: " + Show(report.AllDifferences(before, after)));
        Console.WriteLine("A callback walks one chart at a time,"
            + " so the report first copied both charts whole.");
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
