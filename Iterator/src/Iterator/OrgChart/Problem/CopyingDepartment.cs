using dev.kaldiroglu.Iterator.OrgChart.Domain;

namespace dev.kaldiroglu.Iterator.OrgChart.Problem;

/// <summary>
/// Stage two: <b>the department copies everyone into a new list.</b>
/// </summary>
/// <remarks>
/// <para>
/// A real improvement on stage one. The recursion is written once, here. Callers get a copy
/// they cannot change, and they no longer see how the department stores people.
/// </para>
/// <para>What it costs:</para>
/// <list type="bullet">
///   <item>Every call copies the whole department, even when the caller needs only the first
///     person who matches.</item>
///   <item>The copy has one order: members first, then each sub-department in turn. A caller
///     that needs another order must ask for another method.</item>
/// </list>
/// </remarks>
public sealed class CopyingDepartment(string name)
{
    private readonly List<Employee> _members = [];
    private readonly List<CopyingDepartment> _units = [];

    public string Name { get; } = name;

    public CopyingDepartment Add(Employee employee)
    {
        _members.Add(employee);
        return this;
    }

    public CopyingDepartment Add(CopyingDepartment unit)
    {
        _units.Add(unit);
        return this;
    }

    /// <summary>Everyone in this department and below it, as a new list.</summary>
    public IReadOnlyList<Employee> Everyone()
    {
        var everyone = new List<Employee>();
        Collect(this, everyone);
        return everyone.AsReadOnly();
    }

    private static void Collect(CopyingDepartment department, List<Employee> everyone)
    {
        everyone.AddRange(department._members);
        foreach (CopyingDepartment unit in department._units)
        {
            Collect(unit, everyone);
        }
    }
}
