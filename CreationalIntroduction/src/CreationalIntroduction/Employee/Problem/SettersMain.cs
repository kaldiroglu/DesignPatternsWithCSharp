namespace dev.kaldiroglu.CreationalIntroduction.Employee.Problem;

/// <summary>Seven employees, each given only the values someone remembered to set. The last is given none.</summary>
/// <remarks>
/// Run it with <c>dotnet run --project src/CreationalIntroduction.Demo -- employee-problem-setters</c>.
/// </remarks>
public static class SettersMain
{
    public static void Run()
    {
        // Example usage scenarios showing different meaningful states:

        // Scenario 1: New college graduate hire
        Employee newGrad = new Employee();
        newGrad.FirstName = "Sarah";
        newGrad.LastName = "Johnson";
        newGrad.Email = "sarah.johnson@company.com";
        newGrad.Department = "Engineering";
        newGrad.Position = "Junior Software Developer";
        newGrad.EmployeeId = "ENG2024001";
        newGrad.Education = "BS Computer Science, MIT";
        newGrad.EmploymentType = "Full-time";
        newGrad.Salary = 75000m;

        // Scenario 2: Senior executive with security clearance
        Employee executive = new Employee();
        executive.FirstName = "Michael";
        executive.LastName = "Chen";
        executive.Email = "m.chen@company.com";
        executive.Department = "Defense Contracts";
        executive.Position = "Senior Director";
        executive.EmployeeId = "EXE2020045";
        executive.HireDate = new DateOnly(2020, 3, 15);
        executive.Salary = 180000m;
        executive.SecurityClearance = "Top Secret";
        executive.WorkLocation = "Washington DC";
        executive.PreviousCompany = "Lockheed Martin";
        executive.Skills = "Project Management, Government Relations, Systems Architecture";

        // Scenario 3: Remote contractor
        Employee contractor = new Employee();
        contractor.FirstName = "Elena";
        contractor.LastName = "Rodriguez";
        contractor.Email = "elena.contractor@company.com";
        contractor.Department = "Marketing";
        contractor.Position = "UI/UX Designer";
        contractor.EmploymentType = "Contract";
        contractor.IsRemoteWorker = true;
        contractor.TimeZone = "PST";
        contractor.WorkLocation = "Remote - California";
        contractor.Skills = "Figma, Adobe Creative Suite, User Research";
        contractor.HireDate = new DateOnly(2024, 1, 10);

        // Scenario 4: Part-time intern
        Employee intern = new Employee();
        intern.FirstName = "Alex";
        intern.LastName = "Kim";
        intern.Email = "alex.kim.intern@company.com";
        intern.Department = "Data Science";
        intern.Position = "Data Analyst Intern";
        intern.EmploymentType = "Intern";
        intern.Manager = "Dr. Lisa Wang";
        intern.Education = "MS Data Science, Stanford (Expected 2025)";
        intern.HireDate = new DateOnly(2024, 6, 1);
        intern.WorkLocation = "Palo Alto Office";

        // Scenario 5: International employee transferring
        Employee international = new Employee();
        international.FirstName = "Raj";
        international.LastName = "Patel";
        international.Email = "raj.patel@company.com";
        international.Department = "Global Operations";
        international.Position = "Operations Manager";
        international.EmployeeId = "OPS2023012";
        international.HireDate = new DateOnly(2023, 9, 1);
        international.Salary = 95000m;
        international.WorkLocation = "London Office";
        international.TimeZone = "GMT";
        international.PreviousCompany = "Company India Subsidiary";
        international.EmergencyContact = "Priya Patel (Spouse)";
        international.EmergencyPhone = "+44-20-1234-5678";

        // Scenario 6: Returning employee (rehire)
        Employee rehire = new Employee();
        rehire.FirstName = "Jennifer";
        rehire.LastName = "Thompson";
        rehire.Email = "jennifer.thompson@company.com";
        rehire.Department = "Product Management";
        rehire.Position = "Senior Product Manager";
        rehire.EmployeeId = "PM2018003"; // Keeping old ID
        rehire.HireDate = new DateOnly(2024, 7, 15); // New hire date
        rehire.Salary = 125000m;
        rehire.PreviousCompany = "Meta"; // Where they worked between
        rehire.Skills = "Product Strategy, A/B Testing, SQL, Python";
        rehire.Manager = "VP Product";

        Employee forgotten = new Employee();

        Console.WriteLine(newGrad);
        Console.WriteLine(executive);
        Console.WriteLine(contractor);
        Console.WriteLine(intern);
        Console.WriteLine(international);
        Console.WriteLine(rehire);
        Console.WriteLine(forgotten);
    }
}
