namespace dev.kaldiroglu.Iterator.OrgChart.Domain;

/// <summary>A person in the company. A value: two employees with the same name and role are equal.</summary>
public sealed record Employee(string Name, string Role)
{
    public override string ToString() => $"{Name} ({Role})";
}
