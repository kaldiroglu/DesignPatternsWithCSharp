namespace dev.kaldiroglu.Observer.Price.Problem;

/// <summary>
/// Stage two: the screens ask the feed once a second. <see cref="PollingReaders"/> runs a
/// thread, so this class only starts it, changes the price, and stops it. The thread polls at
/// 0, 1 and 2 seconds. Most polls find the same price, and the chart draws it again.
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/Observer.Demo -- price-problem-stage-two</c>.
/// </remarks>
public static class StageTwoMain
{
    public static void Run()
    {
        var feed = new PriceFeed(100);
        var polled = new Chart();
        var polling = new PollingReaders(feed, polled, new Ticker(), new PriceAlert(105));
        polling.Start();
        Thread.Sleep(1_500);
        feed.SetPrice(102);
        Thread.Sleep(1_000);
        polling.Stop();
        Console.WriteLine("Stage two, the screens poll every second: chart " + Show(polled.Points));
    }

    /// <summary>Prints a list as <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<int> items) => "[" + string.Join(", ", items) + "]";
}
