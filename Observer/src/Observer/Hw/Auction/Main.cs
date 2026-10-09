namespace dev.kaldiroglu.Observer.Hw.Auction;

/// <summary>
/// Two bidders watch an auction. When a bid passes Deniz's budget, Deniz stops watching
/// from inside the update. The auction loops over a copy of its list, so nothing fails.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Observer.Demo -- hw-auction</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var auction = new Auction();
        var ayse = new Bidder("Ayse", 500);
        var deniz = new Bidder("Deniz", 200);
        auction.Watch(ayse);
        auction.Watch(deniz);
        auction.Bid("Deniz", 150);
        auction.Bid("Ayse", 250);
        auction.Bid("Ayse", 300);
        Console.WriteLine("Ayse heard:  " + Show(ayse.Heard));
        Console.WriteLine("Deniz heard: " + Show(deniz.Heard));
        Console.WriteLine("Leader: " + auction.Leader + " at " + auction.Highest
                + ", watchers left: " + auction.WatcherCount);
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
