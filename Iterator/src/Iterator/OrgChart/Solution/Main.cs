using dev.kaldiroglu.Iterator.OrgChart.Domain;

namespace dev.kaldiroglu.Iterator.OrgChart.Solution;

/// <summary>
/// Builds a small company, walks it in both orders, and compares it with a reorganized copy
/// in which three people are new.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Iterator.Demo -- orgchart</c>.
/// </remarks>
public static class Main
{
    public static Department Company(string salesHead) => Company(salesHead, "Can", "Elif");

    public static Department Company(string salesHead, string exporter, string supporter)
    {
        Department export = new Department("Export")
            .Add(new Employee(exporter, "export"));
        Department sales = new Department("Sales")
            .Add(new Employee(salesHead, "head of sales"))
            .Add(new Employee("Ali", "sales"))
            .Add(export);
        Department support = new Department("Support")
            .Add(new Employee(supporter, "support"));
        Department operations = new Department("Operations")
            .Add(new Employee("Mert", "head of operations"))
            .Add(support);
        return new Department("Head office")
            .Add(new Employee("Ayse", "CEO"))
            .Add(sales)
            .Add(operations);
    }

    public static void Run()
    {
        Department company = Company("Deniz");

        Console.WriteLine("Department by department:");
        foreach (Employee employee in company)
        {
            Console.WriteLine("  " + employee);
        }

        Console.WriteLine("Level by level:");
        foreach (Employee employee in company.ByLevel())
        {
            Console.WriteLine("  " + employee);
        }

        // three people are new after the reorganization
        Department reorganized = Company("Zeynep", "Burak", "Ece");
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
