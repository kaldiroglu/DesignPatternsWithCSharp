using dev.kaldiroglu.Iterator.OrgChart.Domain;

namespace dev.kaldiroglu.Iterator.OrgChart.Problem;

/// <summary>
/// Runs the three stages: give out the lists, copy them, call back, and shows what each one
/// costs.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Iterator.Demo -- orgchart-problem</c>.
/// </remarks>
public static class Main
{
    public static CallbackDepartment Company(string salesHead) => Company(salesHead, "Can", "Elif");

    public static CallbackDepartment Company(string salesHead, string exporter, string supporter)
    {
        var sales = new CallbackDepartment("Sales")
            .Add(new Employee(salesHead, "head of sales"))
            .Add(new Employee("Ali", "sales"))
            .Add(new CallbackDepartment("Export").Add(new Employee(exporter, "export")));
        var operations = new CallbackDepartment("Operations")
            .Add(new Employee("Mert", "head of operations"))
            .Add(new CallbackDepartment("Support").Add(new Employee(supporter, "support")));
        return new CallbackDepartment("Head office")
            .Add(new Employee("Ayse", "CEO")).Add(sales).Add(operations);
    }

    public static void Run()
    {
        var ali = new Employee("Ali", "sales");
        var open = new OpenDepartment("Sales")
            .Add(new Employee("Deniz", "head of sales")).Add(ali);
        open.Members.Remove(ali);
        Console.WriteLine("Stage one: a caller removed Ali from the real list. Payroll: "
            + Show(new PayrollRun().Payslips(open)));

        var copying = new CopyingDepartment("Sales")
            .Add(new Employee("Deniz", "head of sales"));
        Console.WriteLine("Stage two: every call makes a new copy: "
            + (!ReferenceEquals(copying.Everyone(), copying.Everyone()) ? "true" : "false"));

        var before = Company("Deniz");
        var byLevel = new List<Employee>();
        before.ForEachMemberByLevel(byLevel.Add);
        Console.WriteLine("Stage three, level by level: " + Show(byLevel.Select(e => e.ToString())));

        // three people are new after the reorganization
        var after = Company("Zeynep", "Burak", "Ece");
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
