namespace dev.kaldiroglu.Iterator.Gof;

/// <summary>
/// Pairs every employee with every other one — a loop inside a loop — first with a list that
/// walks itself, then with iterators. Then the other iterators of GoF's sample code.
/// </summary>
/// <remarks>
/// <para>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Iterator.Demo -- gof</c>.
/// </para>
/// <para>
/// GoF's list is written <c>Solution.List&lt;Employee&gt;</c>. From this namespace,
/// <c>Solution</c> names the child namespace, so GoF's class keeps its name and does not
/// clash with .NET's <c>List&lt;T&gt;</c>, which the plain name <c>List&lt;string&gt;</c>
/// still means here.
/// </para>
/// </remarks>
public static class Main
{
    private static readonly string[] Names = ["Ayse", "Deniz", "Elif"];

    public static void Run()
    {
        var cursorList = new Problem.CursorList<Employee>();
        foreach (string name in Names) cursorList.Append(new Employee(name));

        var pairs = new List<string>();
        for (cursorList.First(); !cursorList.IsDone(); cursorList.Next())
        {
            Employee a = cursorList.CurrentItem();
            for (cursorList.First(); !cursorList.IsDone(); cursorList.Next())
            {
                pairs.Add(a + "-" + cursorList.CurrentItem());
            }
        }

        Console.WriteLine("One cursor:      " + Show(pairs));

        var list = new Solution.List<Employee>();
        foreach (string name in Names) list.Append(new Employee(name));

        pairs.Clear();
        Solution.IIterator<Employee> outer = list.CreateIterator();
        for (outer.First(); !outer.IsDone(); outer.Next())
        {
            Solution.IIterator<Employee> inner = list.CreateIterator();
            for (inner.First(); !inner.IsDone(); inner.Next())
            {
                pairs.Add(outer.CurrentItem() + "-" + inner.CurrentItem());
            }
        }

        Console.WriteLine("Two iterators:   " + Show(pairs));

        Console.WriteLine("Forward:         " + Show(Solution.PrintEmployees.Print(list.CreateIterator())));
        Console.WriteLine("Backward:        "
            + Show(Solution.PrintEmployees.Print(new Solution.ReverseListIterator<Employee>(list))));

        var chain = new Solution.ChainList<Employee>();
        foreach (string name in Names) chain.Append(new Employee(name));
        Console.WriteLine("Chain list:      " + Show(Solution.PrintEmployees.Print(chain.CreateIterator())));

        var firstTwo = new Solution.PrintNEmployees(list, 2);
        bool finished = firstTwo.Traverse();
        Console.WriteLine("First two:       " + Show(firstTwo.Lines)
            + ", walked to the end: " + (finished ? "true" : "false"));
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
