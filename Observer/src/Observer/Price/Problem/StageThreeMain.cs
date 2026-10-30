namespace dev.kaldiroglu.Observer.Price.Problem;

/// <summary>
/// Stage three: the screens ask the feed ten times a second and act only on a change.
/// <see cref="ChangeOnlyReaders"/> runs a thread, so this class only starts it, changes the
/// price twice, and stops it. Only the two changes reach the chart.
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/Observer.Demo -- price-problem-stage-three</c>.
/// </remarks>
public static class StageThreeMain
{
    public static void Run()
    {
        var feed = new PriceFeed(100);
        var chart = new Chart();
        var ticker = new Ticker();
        var alert = new PriceAlert(105);
        var readers = new ChangeOnlyReaders(feed, chart, ticker, alert);
        readers.Start();
        Thread.Sleep(250);
        feed.SetPrice(102);
        Thread.Sleep(300);
        Print(chart, ticker, alert);
        feed.SetPrice(103);
        Thread.Sleep(300);
        Print(chart, ticker, alert);
        readers.Stop();
    }

    private static void Print(Chart chart, Ticker ticker, PriceAlert alert)
    {
        Console.WriteLine("Stage three, the screens poll ten times a second:\nchart " + Show(chart.Points)
                + "\nticker shows " + ticker.Shown + "\nalert fired: " + Show(alert.Fired) + "\n");
    }

    /// <summary>Prints a list as <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<int> items) => "[" + string.Join(", ", items) + "]";

    /// <summary>Prints a <c>bool</c> as <c>true</c> or <c>false</c>.</summary>
    private static string Show(bool value) => value ? "true" : "false";
}
