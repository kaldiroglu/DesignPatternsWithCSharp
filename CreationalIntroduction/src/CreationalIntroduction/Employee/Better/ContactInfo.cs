namespace dev.kaldiroglu.CreationalIntroduction.Employee.Better;

public class ContactInfo
{
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string EmergencyContact { get; set; }
    public string EmergencyPhone { get; set; }

    // Parameterized constructor - null-safe assignment
    public ContactInfo(string? email, string? phoneNumber, string? emergencyContact, string? emergencyPhone)
    {
        Email = email ?? "";
        PhoneNumber = phoneNumber ?? "";
        EmergencyContact = emergencyContact ?? "";
        EmergencyPhone = emergencyPhone ?? "";
    }
}
