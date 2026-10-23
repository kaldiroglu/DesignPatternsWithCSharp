namespace dev.kaldiroglu.Observer.Price.Problem;

/// <summary>
/// Stage two: the screens ask the feed once a second. Most polls find the same price, and the
/// chart draws it again.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Observer.Demo -- price-problem-stage-two</c>.
/// </remarks>
public static class StageTwoMain
{
    public static void Run()
    {
        var feed = new PriceFeed(100);
        var polled = new Chart();
        var polling = new PollingReaders(feed, polled, new Ticker(), new PriceAlert(105));
        polling.Poll();
        polling.Poll();
        feed.SetPrice(102);
        polling.Poll();
        Console.WriteLine("Stage two, the screens poll every second: chart " + Show(polled.Points));
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<int> items) => "[" + string.Join(", ", items) + "]";
}
