namespace dev.kaldiroglu.Observer.Price.Problem;

/// <summary>
/// Stage one: the feed calls every screen. Every price is seen, and the feed knows every
/// screen by class.
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/Observer.Demo -- price-problem-stage-one</c>.
/// </remarks>
public static class StageOneMain
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
    }

    /// <summary>Prints a list as <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<int> items) => "[" + string.Join(", ", items) + "]";

    /// <summary>Prints a <c>bool</c> as <c>true</c> or <c>false</c>.</summary>
    private static string Show(bool value) => value ? "true" : "false";
}
