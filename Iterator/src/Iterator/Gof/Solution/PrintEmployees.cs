namespace dev.kaldiroglu.Iterator.Gof.Solution;

/// <summary>
/// GoF's <c>PrintEmployees</c>: a client written against the <see cref="IIterator{T}"/>
/// interface.
/// </summary>
/// <remarks>
/// It works with a <see cref="ListIterator{T}"/>, a <see cref="ReverseListIterator{T}"/> or a
/// <see cref="ChainListIterator{T}"/>, and does not know which one it has. It returns the
/// printed lines instead of printing them, so a caller can check them.
/// </remarks>
public static class PrintEmployees
{
    public static IReadOnlyList<string> Print(IIterator<Employee> employees)
    {
        // .NET's list, written in full: in this namespace `List<T>` is GoF's list.
        var lines = new System.Collections.Generic.List<string>();
        for (employees.First(); !employees.IsDone(); employees.Next())
        {
            lines.Add(employees.CurrentItem().Name);
        }

        return lines;
    }
}
