using dev.kaldiroglu.Iterator.OrgChart.Domain;

namespace dev.kaldiroglu.Iterator.OrgChart.Problem;

/// <summary>
/// Stage three: <b>the department walks itself and calls the caller back.</b>
/// </summary>
/// <remarks>
/// <para>
/// The best of the three. Nothing is copied. The department hides how it stores people. And
/// it offers two orders: department by department (<see cref="ForEachMember"/>), and level
/// by level (<see cref="ForEachMemberByLevel"/>), which the phone book wants.
/// </para>
/// <para>
/// What it cannot do: the department decides when the walk starts, how fast it goes and
/// when it ends. The caller only receives one person at a time. So a caller cannot walk two
/// departments side by side. After a reorganization, HR asks "what changed?", and
/// <see cref="ChangeReport"/> has to copy both departments into lists to answer. The copy
/// from stage two is back.
/// </para>
/// <para>
/// GoF implementation issue 1 (who controls the iteration?) says the same: comparing two
/// collections is easy with an external iterator and "practically impossible" with an
/// internal one.
/// </para>
/// </remarks>
public sealed class CallbackDepartment(string name)
{
    private readonly List<Employee> _members = [];
    private readonly List<CallbackDepartment> _units = [];

    public string Name { get; } = name;

    public CallbackDepartment Add(Employee employee)
    {
        _members.Add(employee);
        return this;
    }

    public CallbackDepartment Add(CallbackDepartment unit)
    {
        _units.Add(unit);
        return this;
    }

    /// <summary>Members first, then each sub-department in turn.</summary>
    public void ForEachMember(Action<Employee> action)
    {
        _members.ForEach(action);
        foreach (CallbackDepartment unit in _units)
        {
            unit.ForEachMember(action);
        }
    }

    /// <summary>The top level first, then the level below it, and so on.</summary>
    public void ForEachMemberByLevel(Action<Employee> action)
    {
        var waiting = new Queue<CallbackDepartment>();
        waiting.Enqueue(this);
        while (waiting.Count > 0)
        {
            CallbackDepartment department = waiting.Dequeue();
            department._members.ForEach(action);
            foreach (CallbackDepartment unit in department._units)
            {
                waiting.Enqueue(unit);
            }
        }
    }
}
