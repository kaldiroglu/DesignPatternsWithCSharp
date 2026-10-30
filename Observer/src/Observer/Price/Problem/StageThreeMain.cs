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
        var changes = new Chart();
        var readers = new ChangeOnlyReaders(feed, changes, new Ticker(), new PriceAlert(105));
        readers.Start();
        Thread.Sleep(250);
        feed.SetPrice(102);
        Thread.Sleep(300);
        feed.SetPrice(103);
        Thread.Sleep(250);
        readers.Stop();
        Console.WriteLine("Stage three, the screens poll ten times a second: chart " + Show(changes.Points));
    }

    /// <summary>Prints a list as <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<int> items) => "[" + string.Join(", ", items) + "]";
}
