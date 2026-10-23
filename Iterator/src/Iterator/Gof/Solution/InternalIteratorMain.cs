namespace dev.kaldiroglu.Iterator.Gof.Solution;

/// <summary>
/// An internal iterator: the traverser runs the loop, and <c>PrintNEmployees</c> stops it
/// after two.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Iterator.Demo -- gof-internal-iterator</c>.
/// </remarks>
public static class InternalIteratorMain
{
    public static void Run()
    {
        var firstTwo = new PrintNEmployees(SampleLists.List(), 2);
        bool finished = firstTwo.Traverse();
        Console.WriteLine("First two: " + Show(firstTwo.Lines) + ", walked to the end: "
            + (finished ? "true" : "false"));
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
