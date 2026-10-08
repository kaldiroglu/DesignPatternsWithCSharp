using dev.kaldiroglu.Iterator.OrgChart.Domain;

namespace dev.kaldiroglu.Iterator.OrgChart.Problem;

/// <summary>
/// Stage one: <b>the department gives out its own lists.</b>
/// </summary>
/// <remarks>
/// <para>
/// A department has members and sub-departments. Payroll, the phone book and every other
/// program need "everyone in this department", so the department returns its two internal
/// lists and each caller walks them.
/// </para>
/// <para>It works. What it costs:</para>
/// <list type="bullet">
///   <item>Every caller writes the same recursion. See <see cref="PayrollRun"/>.</item>
///   <item>Callers get the real lists, so a caller can add or remove people by mistake.</item>
///   <item>The department can never change how it stores people. Every caller depends on
///     <c>List</c>.</item>
/// </list>
/// </remarks>
public sealed class OpenDepartment(string name)
{
    private readonly List<Employee> _members = [];
    private readonly List<OpenDepartment> _units = [];

    public string Name { get; } = name;

    public OpenDepartment Add(Employee employee)
    {
        _members.Add(employee);
        return this;
    }

    public OpenDepartment Add(OpenDepartment unit)
    {
        _units.Add(unit);
        return this;
    }

    /// <summary>The internal list itself.</summary>
    public List<Employee> Members => _members;

    /// <summary>The internal list itself.</summary>
    public List<OpenDepartment> Units => _units;
}
