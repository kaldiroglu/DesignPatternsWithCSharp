namespace dev.kaldiroglu.Iterator.Gof.Solution;

/// <summary>
/// Pairs every employee with every other one: a loop inside a loop, with two iterators.
/// </summary>
/// <remarks>
/// <para>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Iterator.Demo -- gof-two-iterators</c>.
/// </para>
/// <para>
/// In this namespace <c>List&lt;T&gt;</c> is GoF's list, so .NET's list is written in full,
/// as Java writes <c>java.util.List</c>.
/// </para>
/// </remarks>
public static class TwoIteratorsMain
{
    public static void Run()
    {
        var list = SampleLists.List();

        var pairs = new System.Collections.Generic.List<string>();
        IIterator<Employee> outer = list.CreateIterator();
        for (outer.First(); !outer.IsDone(); outer.Next())
        {
            IIterator<Employee> inner = list.CreateIterator();
            for (inner.First(); !inner.IsDone(); inner.Next())
            {
                pairs.Add(outer.CurrentItem() + "-" + inner.CurrentItem());
            }
        }
        Console.WriteLine("Two iterators, " + pairs.Count + " pairs: " + Show(pairs));
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
