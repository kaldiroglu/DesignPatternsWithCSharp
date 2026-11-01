using System.Globalization;

namespace dev.kaldiroglu.CreationalIntroduction.Employee.Problem;

/// <summary>
/// An employee with 21 settable properties and four constructors. Any property can be left
/// empty, and four string parameters in a row can be passed in the wrong order.
/// </summary>
public class Employee
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Department { get; set; }
    public string? Position { get; set; }
    public string? EmployeeId { get; set; }
    public DateOnly? HireDate { get; set; }
    public DateOnly? BirthDate { get; set; }
    public decimal? Salary { get; set; }
    public string? Manager { get; set; }
    public string? WorkLocation { get; set; }
    public string? EmploymentType { get; set; } // Full-time, Part-time, Contract, Intern
    public string? SecurityClearance { get; set; }
    public string? EmergencyContact { get; set; }
    public string? EmergencyPhone { get; set; }
    public bool IsRemoteWorker { get; set; }
    public string? TimeZone { get; set; }
    public string? Skills { get; set; }
    public string? Education { get; set; }
    public string? PreviousCompany { get; set; }

    // Default constructor for post-construction initialization
    public Employee()
    {
        HireDate = DateOnly.FromDateTime(DateTime.Now);
        EmploymentType = "Full-time";
        IsRemoteWorker = false;
        TimeZone = "EST";
    }

    // Imagine trying to create constructors for all meaningful combinations:

    // Basic employee constructor
    public Employee(string firstName, string lastName, string email, string department)
        : this()
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Department = department;
    }

    // New hire constructor
    public Employee(string firstName, string lastName, string email, string department,
                    string position, DateOnly hireDate)
        : this(firstName, lastName, email, department)
    {
        Position = position;
        HireDate = hireDate;
    }

    // Full employee constructor - but this is getting unwieldy!
    public Employee(string firstName, string lastName, string email, string phoneNumber,
                    string department, string position, string employeeId, DateOnly hireDate,
                    decimal salary, string manager)
        : this(firstName, lastName, email, department, position, hireDate)
    {
        PhoneNumber = phoneNumber;
        EmployeeId = employeeId;
        Salary = salary;
        Manager = manager;
    }

    // We'd need hundreds more constructors for different combinations!
    // What about contractors? Remote workers? Interns with different fields?

    /// <summary>A value that was never set prints as the word <c>null</c>.</summary>
    public override string ToString() =>
        Shown(FirstName) + " " + Shown(LastName) + " | " + Shown(EmploymentType)
        + " | id=" + Shown(EmployeeId)
        + " | salary=" + (Salary?.ToString(CultureInfo.InvariantCulture) ?? "null");

    private static string Shown(string? value) => value ?? "null";
}

/*
Why constructors would be a nightmare for this domain object:

1. DIFFERENT EMPLOYEE TYPES: New grads, executives, contractors, interns,
   international transfers, rehires - each has different relevant fields.

2. WORKFLOW DIFFERENCES:
   - New hires: Basic info first, then HR adds employee ID, salary later
   - Contractors: No employee ID or salary initially
   - Interns: Limited fields, temporary data
   - Transfers: Some fields carry over, others are new

3. TEMPORAL ASPECTS: Employee data is built up over time:
   - Initial application: Name, email, position
   - Background check: Previous company, education
   - Onboarding: Employee ID, manager, work location
   - Payroll setup: Salary, emergency contacts

4. OPTIONAL COMBINATIONS:
   - Remote workers need timezone, not work location
   - Government contractors need security clearance
   - Part-time workers might not have all benefits fields
   - International employees need different emergency contact formats

5. BUSINESS RULES: Different combinations are valid for different scenarios,
   and trying to encode all these rules into constructors would be impossible.

The post-construction approach perfectly models how employee data is actually
collected and managed in real business processes - gradually, with different
fields relevant at different times and for different types of employees.
*/
