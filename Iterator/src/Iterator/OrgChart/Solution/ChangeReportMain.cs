namespace dev.kaldiroglu.Iterator.OrgChart.Solution;

/// <summary>
/// HR compares two charts. The report moves one iterator in each chart, side by side.
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/Iterator.Demo -- orgchart-change-report</c>.
/// </remarks>
public static class ChangeReportMain
{
    public static void Run()
    {
        Department company = SampleCompany.Build("Deniz");
        // three people are new after the reorganization
        Department reorganized = SampleCompany.Build("Zeynep", "Burak", "Ece");
        var report = new ChangeReport();
        Console.WriteLine("First change: "
            + (report.FirstDifference(company, reorganized) ?? "none"));
        Console.WriteLine("All changes:");
        foreach (string change in report.AllDifferences(company, reorganized))
        {
            Console.WriteLine("  " + change);
        }
    }
}
