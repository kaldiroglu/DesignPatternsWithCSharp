namespace dev.kaldiroglu.CreationalIntroduction.Employee.Better;

public class PersonalInfo
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string Education { get; set; }
    public string Skills { get; set; }
    public string PreviousCompany { get; set; }

    public PersonalInfo(string? firstName, string? lastName, DateOnly? birthDate,
                        string? education, string? skills, string? previousCompany)
    {
        FirstName = firstName ?? "";
        LastName = lastName ?? "";
        BirthDate = birthDate;
        Education = education ?? "";
        Skills = skills ?? "";
        PreviousCompany = previousCompany ?? "";
    }
}
