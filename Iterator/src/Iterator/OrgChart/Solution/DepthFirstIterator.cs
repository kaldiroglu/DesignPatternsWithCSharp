using System.Collections;

using dev.kaldiroglu.Iterator.OrgChart.Domain;

namespace dev.kaldiroglu.Iterator.OrgChart.Solution;

/// <summary>
/// A <b>ConcreteIterator</b>: members first, then each sub-department in turn.
/// </summary>
/// <remarks>
/// <para>
/// It keeps its own position — a stack of departments still to visit, and an enumerator over
/// the current department's members — so two of these can walk the same department at the
/// same time without disturbing each other. It copies nothing: it finds the next person only
/// when <see cref="MoveNext"/> is called.
/// </para>
/// <para>
/// This is GoF implementation issue 7 (iterators for composites): an external iterator over
/// a recursive structure must remember the path it took, here as a stack.
/// </para>
/// <para>
/// A C# developer would usually write this walk with <c>yield return</c>, and the compiler
/// would build a class like this one. <see cref="DepthFirstWithYield"/> shows that version.
/// This class is written out by hand so that the iterator and its position stay visible.
/// </para>
/// </remarks>
internal sealed class DepthFirstIterator : IEnumerator<Employee>
{
    private readonly Department _root;
    private readonly Stack<Department> _departments = new();
    private IEnumerator<Employee> _members = Enumerable.Empty<Employee>().GetEnumerator();
    private Employee? _current;

    internal DepthFirstIterator(Department root)
    {
        _root = root;
        _departments.Push(root);
    }

    public Employee Current =>
        _current ?? throw new InvalidOperationException("no current employee: call MoveNext first");

    object IEnumerator.Current => Current;

    public bool MoveNext()
    {
        while (!_members.MoveNext())
        {
            if (_departments.Count == 0)
            {
                _current = null;
                return false;
            }

            Department department = _departments.Pop();
            // Push in reverse, so sub-departments come back in the order they were added.
            for (int i = department.Units.Count - 1; i >= 0; i--)
            {
                _departments.Push(department.Units[i]);
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
        _departments.Clear();
        _departments.Push(_root);
        _members.Dispose();
        _members = Enumerable.Empty<Employee>().GetEnumerator();
        _current = null;
    }

    public void Dispose() => _members.Dispose();
}
