using dev.kaldiroglu.Iterator.OrgChart.Domain;

namespace dev.kaldiroglu.Iterator.OrgChart.Problem;

/// <summary>
/// Stage three: the department walks itself and calls the caller back for each person.
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/Iterator.Demo -- orgchart-problem-stage-three</c>.
/// </remarks>
public static class StageThreeMain
{
    public static void Run()
    {
        var byLevel = new List<Employee>();
        SampleCompany.Build("Deniz").ForEachMemberByLevel(byLevel.Add);
        Console.WriteLine("Stage three, level by level: " + Show(byLevel.Select(e => e.ToString())));
    }

    /// <summary>Prints a list as <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
