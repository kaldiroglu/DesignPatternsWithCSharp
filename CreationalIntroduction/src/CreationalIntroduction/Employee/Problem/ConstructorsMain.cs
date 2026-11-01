namespace dev.kaldiroglu.CreationalIntroduction.Employee.Problem;

/// <summary>One employee from each of the four constructors, and a second from the longest one.</summary>
/// <remarks>
/// Run it with <c>dotnet run --project src/CreationalIntroduction.Demo -- employee-problem-constructors</c>.
/// </remarks>
public static class ConstructorsMain
{
    public static void Run()
    {
        // 1) Default constructor (uses defaults: hireDate=now, employmentType="Full-time", etc.)
        Employee e0 = new Employee();

        // 2) Basic employee constructor
        Employee e1 = new Employee(
                "Sarah",
                "Johnson",
                "sarah.johnson@company.com",
                "Engineering"
        );

        // 3) New hire constructor (adds position + explicit hireDate)
        Employee e2 = new Employee(
                "Alex",
                "Kim",
                "alex.kim@company.com",
                "Data Science",
                "Data Analyst Intern",
                new DateOnly(2024, 6, 1)
        );

        // 4) Full employee constructor (the long one)
        Employee e3 = new Employee(
                "Michael",
                "Chen",
                "m.chen@company.com",
                "+1-202-555-0100",
                "Defense Contracts",
                "Senior Director",
                "EXE2020045",
                new DateOnly(2020, 3, 15),
                180000m,
                "CEO"
        );

        // 5) Another “full” example (different data, same constructor)
        Employee e4 = new Employee(
                "Elena",
                "Rodriguez",
                "elena.rodriguez@company.com",
                "+1-415-555-0142",
                "Marketing",
                "UI/UX Designer",
                "MKT2024019",
                new DateOnly(2024, 1, 10),
                95000m,
                "Creative Director"
        );

        Console.WriteLine(e0);
        Console.WriteLine(e1);
        Console.WriteLine(e2);
        Console.WriteLine(e3);
        Console.WriteLine(e4);
    }
}
