using dev.kaldiroglu.Iterator.OrgChart.Domain;

namespace dev.kaldiroglu.Iterator.OrgChart.Problem;

/// <summary>
/// Stage one: the department gives out its internal lists, so a caller can change them.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Iterator.Demo -- orgchart-problem-stage-one</c>.
/// </remarks>
public static class StageOneMain
{
    public static void Run()
    {
        var ali = new Employee("Ali", "sales");
        var open = new OpenDepartment("Sales")
            .Add(new Employee("Deniz", "head of sales")).Add(ali);
        open.Members.Remove(ali);
        Console.WriteLine("Stage one: a caller removed Ali from the real list. Payroll: "
            + Show(new PayrollRun().Payslips(open)));
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
