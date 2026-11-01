using System.Globalization;

namespace dev.kaldiroglu.CreationalIntroduction.Employee.Better;

/// <summary>Four employees made of the five smaller objects.</summary>
/// <remarks>
/// Run it with <c>dotnet run --project src/CreationalIntroduction.Demo -- employee-better</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        // 1) Office full-time employee
        Employee officeFullTime = new Employee(
                new PersonalInfo("Sarah", "Johnson", new DateOnly(2000, 2, 14),
                        "BS Computer Science", "Java, Spring", "N/A"),
                new ContactInfo("sarah.johnson@company.com", "+1-555-0101", "Jamie Johnson", "+1-555-0199"),
                new JobInfo("ENG2024001", "Engineering", "Junior Software Developer", "Pat Manager",
                        new DateOnly(2024, 7, 1), "Full-time", ""),
                new WorkInfo("NYC Office", false, "EST"),
                new Compensation(75000m)
        );

        // 2) Remote contractor
        Employee remoteContractor = new Employee(
                new PersonalInfo("Elena", "Rodriguez", new DateOnly(1996, 11, 3),
                        "BA Design", "Figma, UX Research", "Freelance"),
                new ContactInfo("elena.contractor@company.com", "+1-555-0142", "", ""),
                new JobInfo("", "Marketing", "UI/UX Designer", "Creative Director",
                        new DateOnly(2024, 1, 10), "Contract", ""),
                WorkInfo.Remote("PST"),
                new Compensation(0m)
        );

        // 3) Intern (limited job scope)
        Employee intern = new Employee(
                new PersonalInfo("Alex", "Kim", new DateOnly(2003, 5, 22),
                        "MS Data Science (Expected)", "Python, SQL", ""),
                new ContactInfo("alex.kim.intern@company.com", "", "", ""),
                new JobInfo("", "Data Science", "Data Analyst Intern", "Dr. Lisa Wang",
                        new DateOnly(2024, 6, 1), "Intern", ""),
                new WorkInfo("Palo Alto Office", false, "PST"),
                new Compensation(25000m)
        );

        // 4) Executive with clearance + high comp
        Employee executive = new Employee(
                new PersonalInfo("Michael", "Chen", new DateOnly(1982, 9, 9),
                        "MBA", "Leadership, Strategy", "Industry"),
                new ContactInfo("m.chen@company.com", "+1-555-0100", "", ""),
                new JobInfo("EXE2020045", "Defense Contracts", "Senior Director", "CEO",
                        new DateOnly(2020, 3, 15), "Full-time", "Top Secret"),
                new WorkInfo("Washington DC", false, "EST"),
                new Compensation(180000m)
        );

        PrintSummary("officeFullTime", officeFullTime);
        PrintSummary("remoteContractor", remoteContractor);
        PrintSummary("intern", intern);
        PrintSummary("executive", executive);
    }

    private static void PrintSummary(string label, Employee e)
    {
        Console.WriteLine(
            $"{label}: {e.Personal.FirstName} {e.Personal.LastName} | {e.Job.Department} | {e.Job.Position}"
            + $" | {e.Job.EmploymentType} | remote={(e.Work.IsRemoteWorker ? "true" : "false")}"
            + $" | tz={e.Work.TimeZone} | salary={e.Compensation.Salary.ToString(CultureInfo.InvariantCulture)}");
    }
}
