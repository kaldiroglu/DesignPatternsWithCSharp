namespace dev.kaldiroglu.Composite.Hw.OrgChart;

/// <summary>
/// The Composite: someone with reports, who is also somebody's report.
/// </summary>
/// <remarks>
/// A manager <b>counts their own salary</b> as well as their reports'. That is a decision,
/// not an accident, and it is the first thing to settle: if a manager did not count
/// themselves, the cost of the company would not include the chief executive.
/// </remarks>
public sealed class Manager(string name, string role, long salary) : IEmployee
{
    private readonly List<IEmployee> _reports = [];

    public string Name { get; } = name ?? throw new ArgumentNullException(nameof(name));

    public Manager Add(IEmployee report)
    {
        if (ReferenceEquals(report, this))
        {
            throw new ArgumentException("nobody reports to themselves", nameof(report));
        }

        if (ReachesFrom(report, this))
        {
            throw new ArgumentException(
                $"that would make a cycle: {report.Name} already manages {Name}", nameof(report));
        }

        _reports.Add(report);
        return this;
    }

    public IReadOnlyList<IEmployee> Reports => _reports.AsReadOnly();

    public long TotalCost() => salary + _reports.Sum(r => r.TotalCost());

    public int Headcount() => 1 + _reports.Sum(r => r.Headcount());

    public string Render(string indent)
    {
        var output = new System.Text.StringBuilder($"{indent}{Name} — {role} ({salary})");
        foreach (var report in _reports)
        {
            output.AppendLine().Append(report.Render(indent + "    "));
        }

        return output.ToString();
    }

    /// <summary>
    /// Counts how many <b>distinct</b> people are below this manager.
    /// </summary>
    /// <remarks>
    /// This is the honest answer to the second half of the exercise. <see cref="Headcount"/>
    /// adds up the tree, so anyone reachable by two routes is counted twice. Identity-based
    /// de-duplication gives the true number — and the gap between the two is the exercise.
    /// </remarks>
    public int DistinctHeadcount()
    {
        var seen = new HashSet<IEmployee>(ReferenceEqualityComparer.Instance);
        Collect(this, seen);
        return seen.Count;
    }

    private static void Collect(IEmployee employee, HashSet<IEmployee> seen)
    {
        if (!seen.Add(employee))
        {
            return;
        }

        if (employee is Manager manager)
        {
            foreach (var report in manager._reports)
            {
                Collect(report, seen);
            }
        }
    }

    private static bool ReachesFrom(IEmployee start, IEmployee target) =>
        ReferenceEquals(start, target)
        || (start is Manager manager && manager._reports.Any(r => ReachesFrom(r, target)));
}
