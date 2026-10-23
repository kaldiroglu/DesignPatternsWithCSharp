using dev.kaldiroglu.Iterator.OrgChart.Domain;

namespace dev.kaldiroglu.Iterator.OrgChart.Problem;

/// <summary>
/// Stage three: the department walks itself and calls the caller back for each person.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Iterator.Demo -- orgchart-stage-three</c>.
/// </remarks>
public static class StageThreeMain
{
    public static void Run()
    {
        var byLevel = new List<Employee>();
        SampleCompany.Build("Deniz").ForEachMemberByLevel(byLevel.Add);
        Console.WriteLine("Stage three, level by level: " + Show(byLevel.Select(e => e.ToString())));
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
