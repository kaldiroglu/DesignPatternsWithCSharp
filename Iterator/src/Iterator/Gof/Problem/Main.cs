namespace dev.kaldiroglu.Iterator.Gof.Problem;

/// <summary>
/// Pairs every employee with every other one using a list that keeps its own cursor, and shows
/// the walk breaking.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Iterator.Demo -- gof-problem</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var list = new CursorList<Employee>();
        foreach (var name in new[] { "Ayse", "Deniz", "Elif" })
        {
            list.Append(new Employee(name));
        }

        var pairs = new List<string>();
        for (list.First(); !list.IsDone(); list.Next())
        {
            Employee a = list.CurrentItem();
            for (list.First(); !list.IsDone(); list.Next())
            {
                pairs.Add(a + "-" + list.CurrentItem());
            }
        }
        Console.WriteLine("Expected " + list.Count * list.Count + " pairs, got " + pairs.Count
            + ": [" + string.Join(", ", pairs) + "]");
        Console.WriteLine("The inner loop moved the only cursor, so the outer loop ended after Ayse.");
    }
}
