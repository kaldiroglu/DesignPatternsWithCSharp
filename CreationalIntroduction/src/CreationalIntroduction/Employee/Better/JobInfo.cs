namespace dev.kaldiroglu.CreationalIntroduction.Employee.Better;

public class JobInfo
{
    public string EmployeeId { get; set; }
    public string Department { get; set; }
    public string Position { get; set; }
    public string Manager { get; set; }
    public DateOnly HireDate { get; set; }
    public string EmploymentType { get; set; } // Full-time, Part-time, Contract, Intern
    public string SecurityClearance { get; set; }

    public JobInfo(string? employeeId, string? department, string? position, string? manager,
                   string? securityClearance)
    {
        EmployeeId = employeeId ?? "";
        Department = department ?? "";
        Position = position ?? "";
        Manager = manager ?? "";
        HireDate = DateOnly.FromDateTime(DateTime.Now);
        EmploymentType = "Full-time";
        SecurityClearance = securityClearance ?? "";
    }

    public JobInfo(string? employeeId, string? department, string? position, string? manager,
                   DateOnly? hireDate, string? employmentType, string? securityClearance)
    {
        EmployeeId = employeeId ?? "";
        Department = department ?? "";
        Position = position ?? "";
        Manager = manager ?? "";
        HireDate = hireDate ?? DateOnly.FromDateTime(DateTime.Now);
        EmploymentType = employmentType ?? "Full-time";
        SecurityClearance = securityClearance ?? "";
    }
}
