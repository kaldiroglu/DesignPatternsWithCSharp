namespace dev.kaldiroglu.Observer.Price.Solution;

/// <summary>
/// The same ten seconds of prices, read by stage three's polling readers and by listeners.
/// The price spikes from 100 to 106 and back to 100 between two polls.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Observer.Demo -- price</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        // Stage three: readers poll ten times a second (100 polls in ten seconds).
        var feed = new Problem.PriceFeed(100);
        var chart = new Problem.Chart();
        var ticker = new Problem.Ticker();
        var alert = new Problem.PriceAlert(105);
        var readers = new Problem.ChangeOnlyReaders(feed, chart, ticker, alert);
        int readsBefore = feed.Reads;
        for (int tenth = 1; tenth <= 100; tenth++)
        {
            if (tenth == 20) feed.SetPrice(102);
            if (tenth == 55) { feed.SetPrice(106); feed.SetPrice(100); }   // a spike between polls
            if (tenth == 80) feed.SetPrice(103);
            readers.Poll();
        }
        Console.WriteLine("Polling:   chart " + Show(chart.Points) + ", alert fired: " + Show(alert.Fired)
                + ", reads " + (feed.Reads - readsBefore));

        // The Observer pattern: the feed tells its listeners.
        var observed = new PriceFeed("ACME", 100);
        var observedChart = new Chart();
        var observedTicker = new Ticker();
        var observedAlert = new PriceAlert(105);
        observed.Subscribe(observedChart);
        observed.Subscribe(observedTicker);
        observed.Subscribe(observedAlert);
        var log = new List<string>();
        observed.Subscribe(change => log.Add(change.OldPrice + "->" + change.NewPrice));

        observed.SetPrice(102);
        observed.SetPrice(106);
        observed.SetPrice(100);
        observed.SetPrice(103);
        Console.WriteLine("Listeners: chart " + Show(observedChart.Points) + ", alert fired: "
                + Show(observedAlert.Fired) + ", ticker " + observedTicker.Shown + ", lambda " + Show(log));
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    internal static string Show<T>(IEnumerable<T> items) => "[" + string.Join(", ", items) + "]";

    /// <summary>Prints a <c>bool</c> the way Java does: <c>true</c> or <c>false</c>.</summary>
    internal static string Show(bool value) => value ? "true" : "false";
}
