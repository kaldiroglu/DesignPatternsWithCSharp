using dev.kaldiroglu.Iterator.OrgChart.Domain;

namespace dev.kaldiroglu.Iterator.OrgChart.Solution;

/// <summary>
/// Builds a small company, walks it in both orders, and compares it with a reorganized copy.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Iterator.Demo -- orgchart</c>.
/// </remarks>
public static class Main
{
    public static Department Company(string salesHead)
    {
        Department export = new Department("Export")
            .Add(new Employee("Can", "export"));
        Department sales = new Department("Sales")
            .Add(new Employee(salesHead, "head of sales"))
            .Add(new Employee("Ali", "sales"))
            .Add(export);
        Department support = new Department("Support")
            .Add(new Employee("Elif", "support"));
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

        Department reorganized = Company("Zeynep");
        Console.WriteLine("First change: "
            + (new ChangeReport().FirstDifference(company, reorganized) ?? "none"));
    }
}
