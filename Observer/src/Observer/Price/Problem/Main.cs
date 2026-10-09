namespace dev.kaldiroglu.Observer.Price.Problem;

/// <summary>
/// Runs the three stages. Stage one sees every price but knows every screen; stage three
/// polls ten times a second and misses a spike to 106, so the alert at 105 never fires.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Observer.Demo -- price-problem</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var chart = new Chart();
        var alert = new PriceAlert(105);
        var direct = new DirectPriceFeed(chart, new Ticker(), alert);
        foreach (int price in new[] { 102, 106, 100, 103 })
        {
            direct.SetPrice(price);
        }
        Console.WriteLine("Stage one, the feed calls every screen: chart " + Show(chart.Points)
                + ", alert fired: " + Show(alert.Fired));
        var feed = new PriceFeed(100);
        var polled = new Chart();
        var polling = new PollingReaders(feed, polled, new Ticker(), new PriceAlert(105));
        polling.Poll();
        polling.Poll();
        feed.SetPrice(102);
        polling.Poll();
        Console.WriteLine("Stage two, the screens poll every second: chart " + Show(polled.Points));
        var fast = new PriceFeed(100);
        var changes = new Chart();
        var missed = new PriceAlert(105);
        var readers = new ChangeOnlyReaders(fast, changes, new Ticker(), missed);
        int readsBefore = fast.Reads;
        for (int tenth = 1; tenth <= 100; tenth++)
        {
            if (tenth == 20) fast.SetPrice(102);
            if (tenth == 55) { fast.SetPrice(106); fast.SetPrice(100); }   // a spike between polls
            if (tenth == 80) fast.SetPrice(103);
            readers.Poll();
        }
        Console.WriteLine("Stage three, ten polls a second for ten seconds: chart " + Show(changes.Points)
                + ", alert fired: " + Show(missed.Fired) + ", reads " + (fast.Reads - readsBefore));
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<int> items) => "[" + string.Join(", ", items) + "]";

    /// <summary>Prints a <c>bool</c> the way Java does: <c>true</c> or <c>false</c>.</summary>
    private static string Show(bool value) => value ? "true" : "false";
}
