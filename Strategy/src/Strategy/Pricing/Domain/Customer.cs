namespace dev.kaldiroglu.Strategy.Pricing.Domain;

/// <summary>
/// Who is at the till, and the two facts a campaign is allowed to ask about them.
/// </summary>
/// <param name="Name">for the receipt</param>
/// <param name="IsStudent">whether they showed a student card</param>
/// <param name="IsStaff">whether they work here</param>
public sealed record Customer(string Name, bool IsStudent, bool IsStaff)
{
    public static Customer Shopper(string name) => new(name, false, false);

    public static Customer Student(string name) => new(name, true, false);

    public static Customer Staff(string name) => new(name, false, true);
}
