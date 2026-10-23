using dev.kaldiroglu.Iterator.OrgChart.Domain;

namespace dev.kaldiroglu.Iterator.OrgChart.Problem;

/// <summary>
/// Stage two: the department copies everyone into a new list, on every call.
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/Iterator.Demo -- orgchart-problem-stage-two</c>.
/// </remarks>
public static class StageTwoMain
{
    public static void Run()
    {
        var copying = new CopyingDepartment("Sales")
            .Add(new Employee("Deniz", "head of sales"));
        Console.WriteLine("Stage two: every call makes a new copy: "
            + (!ReferenceEquals(copying.Everyone(), copying.Everyone()) ? "true" : "false"));
    }
}
