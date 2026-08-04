namespace dev.kaldiroglu.Composite.Hw.OrgChart;

/// <summary>
/// The Component: anyone on the org chart, whether or not they have reports.
/// </summary>
/// <remarks>
/// Both operations are roll-ups. A client asks one person and gets an answer for the whole
/// organization beneath them — which is the only reason to build a Composite at all.
/// </remarks>
public interface IEmployee
{
    string Name { get; }

    /// <summary>Salary cost of this person and everyone below them, in whole liras.</summary>
    long TotalCost();

    /// <summary>This person and everyone below them.</summary>
    int Headcount();

    string Render(string indent);
}
