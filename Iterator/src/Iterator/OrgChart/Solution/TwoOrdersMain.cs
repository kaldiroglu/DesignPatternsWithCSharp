using dev.kaldiroglu.Iterator.OrgChart.Domain;

namespace dev.kaldiroglu.Iterator.OrgChart.Solution;

/// <summary>
/// Walks one department in two orders: department by department, then level by level.
/// </summary>
/// <remarks>
/// <para>
/// Neither loop names an enumerator class. <c>Department.GetEnumerator()</c> creates a
/// <see cref="DepthFirstIterator"/>, and <c>ByLevel()</c> creates a
/// <see cref="LevelOrderIterator"/>.
/// </para>
/// <para>
/// Run it with
/// <c>dotnet run --project src/Iterator.Demo -- orgchart-two-orders</c>.
/// </para>
/// </remarks>
public static class TwoOrdersMain
{
    public static void Run()
    {
        Department company = SampleCompany.Build("Deniz");

        Console.WriteLine("Department by department:");
        foreach (Employee employee in company)              // a DepthFirstIterator
        {
            Console.WriteLine("  " + employee);
        }

        Console.WriteLine("Level by level:");
        foreach (Employee employee in company.ByLevel())    // a LevelOrderIterator
        {
            Console.WriteLine("  " + employee);
        }
    }
}
