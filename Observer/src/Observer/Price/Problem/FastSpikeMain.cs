namespace dev.kaldiroglu.Observer.Price.Problem;

/// <summary>
/// Stage three meets a fast spike. The screens poll ten times a second and act only on a
/// change, but the price goes to 106 and back to 100 between two polls, so the alert at 105
/// never fires. This class calls Poll itself, without the thread, so that the spike always
/// falls between the same two polls.
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/Observer.Demo -- price-problem-fast-spike</c>.
/// </remarks>
public static class FastSpikeMain
{
    public static void Run()
    {
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

    /// <summary>Prints a list as <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<int> items) => "[" + string.Join(", ", items) + "]";

    /// <summary>Prints a <c>bool</c> as <c>true</c> or <c>false</c>.</summary>
    private static string Show(bool value) => value ? "true" : "false";
}
