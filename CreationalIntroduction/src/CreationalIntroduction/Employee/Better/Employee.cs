namespace dev.kaldiroglu.CreationalIntroduction.Employee.Better;

/// <summary>The same employee, made of five smaller objects.</summary>
public class Employee
{
    public PersonalInfo Personal { get; }
    public ContactInfo Contact { get; }
    public JobInfo Job { get; }
    public WorkInfo Work { get; }
    public Compensation Compensation { get; }

    public Employee(PersonalInfo? personal, ContactInfo? contact, JobInfo? job, WorkInfo? work, Compensation? compensation)
    {
        Personal = personal ?? new PersonalInfo("", "", null, "", "", "");
        Contact = contact ?? new ContactInfo("", "", "", "");
        Job = job ?? new JobInfo("", "", "", "", DateOnly.FromDateTime(DateTime.Now), "", "");

        if (string.IsNullOrEmpty(Job.EmploymentType))
        {
            Job.EmploymentType = "Full-time";
        }

        Work = work ?? new WorkInfo("", false, "EST");

        // Ensure work defaults
        if (string.IsNullOrEmpty(Work.TimeZone))
        {
            Work.TimeZone = "EST";
        }
        // IsRemoteWorker is a bool, and its default is false.

        Compensation = compensation ?? new Compensation(0m);
    }
}
