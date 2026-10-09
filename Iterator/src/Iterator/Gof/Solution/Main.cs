namespace dev.kaldiroglu.Iterator.Gof.Solution;

/// <summary>
/// Pairs every employee with every other one using two iterators, then walks backward and over
/// a chain list.
/// </summary>
/// <remarks>
/// <para>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Iterator.Demo -- gof-solution</c>.
/// </para>
/// <para>
/// In this namespace <c>List&lt;T&gt;</c> is GoF's list, so .NET's list is written in full,
/// as Java writes <c>java.util.List</c>.
/// </para>
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var list = new List<Employee>();
        var chain = new ChainList<Employee>();
        foreach (var name in new[] { "Ayse", "Deniz", "Elif" })
        {
            list.Append(new Employee(name));
            chain.Append(new Employee(name));
        }

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

        Console.WriteLine("Backward: " + Show(PrintEmployees.Print(new ReverseListIterator<Employee>(list))));
        Console.WriteLine("Chain list: " + Show(PrintEmployees.Print(chain.CreateIterator())));

        var firstTwo = new PrintNEmployees(list, 2);
        bool finished = firstTwo.Traverse();
        Console.WriteLine("First two: " + Show(firstTwo.Lines) + ", walked to the end: "
            + (finished ? "true" : "false"));
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
