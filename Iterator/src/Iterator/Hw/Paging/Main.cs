namespace dev.kaldiroglu.Iterator.Hw.Paging;

/// <summary>Searches paged results and stops early, so the pages after the stop are never fetched.</summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Iterator.Demo -- hw-paging</c>. Java's
/// <c>hasNext()</c>/<c>next()</c> loop is a <c>MoveNext()</c>/<c>Current</c> loop here.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        using var results = new PagedIterator<string>(number =>
        {
            Console.WriteLine("  fetching page " + number);
            return number < 5 ? ["p" + number + "a", "p" + number + "b"] : [];
        });

        Console.WriteLine("Looking for p1a in five pages of results:");
        while (results.MoveNext())
        {
            var item = results.Current;
            Console.WriteLine("  read " + item);
            if (item == "p1a")
            {
                break;
            }
        }
        Console.WriteLine("Found p1a. Pages fetched: " + results.PagesFetched + " of 5.");
    }
}
