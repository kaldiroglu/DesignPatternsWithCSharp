namespace dev.kaldiroglu.Observer.Price.Problem;

/// <summary>
/// Stage two: the screens ask the feed once a second. <see cref="PollingReaders"/> runs a
/// thread, so this class only starts it, changes the price, and stops it. The thread polls at
/// 0, 1 and 2 seconds, and this class prints what every screen has half a second after each
/// poll. Most polls find the same price, and the chart draws it again.
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
        var chart = new Chart();
        var ticker = new Ticker();
        var alert = new PriceAlert(105);
        var polling = new PollingReaders(feed, chart, ticker, alert);
        polling.Start();
        Thread.Sleep(500);
        Print(chart, ticker, alert);
        Thread.Sleep(1_000);
        Print(chart, ticker, alert);
        feed.SetPrice(102);
        Thread.Sleep(1_000);
        Print(chart, ticker, alert);
        polling.Stop();
    }

    private static void Print(Chart chart, Ticker ticker, PriceAlert alert)
    {
        Console.WriteLine("Stage two, the screens poll every second:\nchart " + Show(chart.Points)
                + "\nticker shows " + ticker.Shown + "\nalert fired: " + Show(alert.Fired) + "\n");
    }

    /// <summary>Prints a list as <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<int> items) => "[" + string.Join(", ", items) + "]";

    /// <summary>Prints a <c>bool</c> as <c>true</c> or <c>false</c>.</summary>
    private static string Show(bool value) => value ? "true" : "false";
}
