namespace dev.kaldiroglu.Composite.Hw.OrgChart;

/// <summary>A Leaf: someone with no reports.</summary>
public sealed class IndividualContributor(string name, string role, long salary) : IEmployee
{
    public string Name { get; } = name ?? throw new ArgumentNullException(nameof(name));

    public long TotalCost() => salary;

    public int Headcount() => 1;

    public string Render(string indent) => $"{indent}{Name} — {role} ({salary})";
}
