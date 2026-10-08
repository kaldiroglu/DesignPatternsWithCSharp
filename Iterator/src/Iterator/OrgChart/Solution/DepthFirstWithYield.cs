using dev.kaldiroglu.Iterator.OrgChart.Domain;

namespace dev.kaldiroglu.Iterator.OrgChart.Solution;

/// <summary>
/// The same walk as <see cref="DepthFirstIterator"/>, written with <c>yield return</c>.
/// </summary>
/// <remarks>
/// <para>
/// This class is only in the C# port. It is here for comparison. A C# developer would
/// usually write an iterator this way: the method looks like an ordinary loop, and the
/// compiler turns it into a class that implements <see cref="IEnumerator{T}"/>, with
/// <c>MoveNext</c> and <c>Current</c>, and keeps the position in that class's fields.
/// </para>
/// <para>
/// The result is the same pattern. The ConcreteIterator still exists, and it still holds its
/// own position, so two walks still do not disturb each other. The only difference is that
/// the compiler writes it instead of the programmer.
/// </para>
/// </remarks>
public static class DepthFirstWithYield
{
    /// <summary>Members first, then each sub-department in turn.</summary>
    public static IEnumerable<Employee> Walk(Department root)
    {
        var departments = new Stack<Department>();
        departments.Push(root);
        while (departments.Count > 0)
        {
            Department department = departments.Pop();
            for (int i = department.Units.Count - 1; i >= 0; i--)
            {
                departments.Push(department.Units[i]);
            }

            foreach (Employee employee in department.Members)
            {
                yield return employee;
            }
        }
    }
}
