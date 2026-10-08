namespace dev.kaldiroglu.Iterator.Gof.Solution;

/// <summary>GoF's <c>PrintNEmployees</c>: an internal iterator that stops after the first n.</summary>
public sealed class PrintNEmployees(AbstractList<Employee> list, int total) : ListTraverser<Employee>(list)
{
    // .NET's list, written in full: in this namespace `List<T>` is GoF's list.
    private readonly System.Collections.Generic.List<string> _lines = [];

    protected override bool ProcessItem(Employee employee)
    {
        _lines.Add(employee.Name);
        return _lines.Count < total;
    }

    /// <summary>A copy of the lines printed so far.</summary>
    public IReadOnlyList<string> Lines => _lines.ToArray();
}
