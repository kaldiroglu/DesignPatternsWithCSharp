using System.Collections;

using dev.kaldiroglu.Iterator.OrgChart.Domain;

namespace dev.kaldiroglu.Iterator.OrgChart.Solution;

/// <summary>
/// A second <b>ConcreteIterator</b> over the same department: level by level.
/// </summary>
/// <remarks>
/// The only difference from <see cref="DepthFirstIterator"/> is a queue instead of a stack.
/// The department did not change at all to get a second order of walking.
/// </remarks>
internal sealed class LevelOrderIterator : IEnumerator<Employee>
{
    private readonly Department _root;
    private readonly Queue<Department> _waiting = new();
    private IEnumerator<Employee> _members = Enumerable.Empty<Employee>().GetEnumerator();
    private Employee? _current;

    internal LevelOrderIterator(Department root)
    {
        _root = root;
        _waiting.Enqueue(root);
    }

    public Employee Current =>
        _current ?? throw new InvalidOperationException("no current employee: call MoveNext first");

    object IEnumerator.Current => Current;

    public bool MoveNext()
    {
        while (!_members.MoveNext())
        {
            if (_waiting.Count == 0)
            {
                _current = null;
                return false;
            }

            Department department = _waiting.Dequeue();
            foreach (Department unit in department.Units)
            {
                _waiting.Enqueue(unit);
            }

            _members.Dispose();
            _members = department.Members.GetEnumerator();
        }

        _current = _members.Current;
        return true;
    }

    /// <summary>Starts the walk again from the top department.</summary>
    public void Reset()
    {
        _waiting.Clear();
        _waiting.Enqueue(_root);
        _members.Dispose();
        _members = Enumerable.Empty<Employee>().GetEnumerator();
        _current = null;
    }

    public void Dispose() => _members.Dispose();
}
