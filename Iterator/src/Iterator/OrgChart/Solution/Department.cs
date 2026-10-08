using System.Collections;

using dev.kaldiroglu.Iterator.OrgChart.Domain;

namespace dev.kaldiroglu.Iterator.OrgChart.Solution;

/// <summary>
/// The <b>Aggregate</b>: a department that gives out iterators instead of its lists.
/// </summary>
/// <remarks>
/// <para>
/// Compare the three classes in <c>Problem</c>. This one has no getter for its members or
/// its sub-departments, it never copies them, and it never calls the caller back. It creates
/// an iterator, and the caller decides when to ask for the next person and when to stop.
/// </para>
/// <para>
/// Because it implements <see cref="IEnumerable{T}"/>, a caller can write
/// <c>foreach (var e in department)</c>. That loop gets its iterator from
/// <see cref="GetEnumerator"/>. A second order is one more method that returns an
/// <see cref="IEnumerable{T}"/>: <see cref="ByLevel"/>.
/// </para>
/// </remarks>
public sealed class Department(string name) : IEnumerable<Employee>
{
    private readonly List<Employee> _members = [];
    private readonly List<Department> _units = [];

    public string Name { get; } = name;

    public Department Add(Employee employee)
    {
        _members.Add(employee);
        return this;
    }

    public Department Add(Department unit)
    {
        _units.Add(unit);
        return this;
    }

    /// <summary>Members first, then each sub-department in turn: the order of the org chart.</summary>
    public IEnumerator<Employee> GetEnumerator() => new DepthFirstIterator(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>The top level first, then the level below it: the order of the phone book.</summary>
    public IEnumerable<Employee> ByLevel() =>
        new LambdaEnumerable<Employee>(() => new LevelOrderIterator(this));

    // Internal: only the iterators in this library may see the structure. Java uses package
    // access here; C# has no package access, so `internal` is the closest choice.
    internal IReadOnlyList<Employee> Members => _members;

    internal IReadOnlyList<Department> Units => _units;
}
