using dev.kaldiroglu.Iterator.OrgChart.Domain;

namespace dev.kaldiroglu.Iterator.OrgChart.Solution;

/// <summary>
/// Walks one department in two orders: department by department, then level by level.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Iterator.Demo -- orgchart-two-orders</c>.
/// </remarks>
public static class TwoOrdersMain
{
    public static void Run()
    {
        Department company = SampleCompany.Build("Deniz");

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
    }
}
