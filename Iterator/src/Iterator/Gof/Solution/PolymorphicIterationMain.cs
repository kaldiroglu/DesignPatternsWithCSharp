namespace dev.kaldiroglu.Iterator.Gof.Solution;

/// <summary>
/// One client, <c>PrintEmployees</c>, walks a list backwards and walks a chain list. It knows
/// only the iterator interface.
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/Iterator.Demo -- gof-polymorphic-iteration</c>.
/// </remarks>
public static class PolymorphicIterationMain
{
    public static void Run()
    {
        Console.WriteLine("Backward: "
            + Show(PrintEmployees.Print(new ReverseListIterator<Employee>(SampleLists.List()))));
        Console.WriteLine("Chain list: "
            + Show(PrintEmployees.Print(SampleLists.Chain().CreateIterator())));
    }

    /// <summary>Prints a list as <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
