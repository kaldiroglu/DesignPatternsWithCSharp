namespace dev.kaldiroglu.CreationalIntroduction.Employee.Problem;

/// <summary>The email and the department passed in the wrong order. Both are strings, so it compiles.</summary>
/// <remarks>
/// Run it with <c>dotnet run --project src/CreationalIntroduction.Demo -- employee-problem-swapped-arguments</c>.
/// </remarks>
public static class SwappedArgumentsMain
{
    public static void Run()
    {
        // The last two arguments are the wrong way around. All four are String, so this compiles.
        Employee sarah = new Employee("Sarah", "Johnson", "Engineering", "sarah.johnson@company.com");

        Console.WriteLine("Email     : " + sarah.Email);
        Console.WriteLine("Department: " + sarah.Department);
    }
}
