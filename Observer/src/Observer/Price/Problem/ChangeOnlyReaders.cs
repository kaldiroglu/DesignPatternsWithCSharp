namespace dev.kaldiroglu.Observer.Price.Problem;

/// <summary>
/// Stage three: <b>the readers poll more often, and act only when the price changed.</b>
/// <para>
/// The best of the three. The feed still knows nobody. The readers poll more often, so they
/// are less late, and they remember the last price, so an unchanged price costs one read
/// and no work: the chart no longer draws the same point again and again.
/// </para>
/// <para>
/// What it cannot fix: a reader sees the price only at the moment it asks. If the price
/// goes from 100 to 106 and back to 100 between two polls, no reader ever sees 106, and the
/// alert at 105 never fires. Polling more often makes this rarer and wastes more reads; it
/// never makes it impossible.
/// </para>
/// </summary>
public sealed class ChangeOnlyReaders
{
    private readonly PriceFeed feed;
    private readonly Chart chart;
    private readonly Ticker ticker;
    private readonly PriceAlert alert;
    private int lastSeen;

    public ChangeOnlyReaders(PriceFeed feed, Chart chart, Ticker ticker, PriceAlert alert)
    {
        this.feed = feed;
        this.chart = chart;
        this.ticker = ticker;
        this.alert = alert;
        lastSeen = feed.Price();
    }

    /// <summary>Called by a timer, ten times a second.</summary>
    public void Poll()
    {
        int price = feed.Price();
        if (price == lastSeen)
        {
            return;                     // nothing new: one read, no work
        }
        lastSeen = price;
        chart.Add(price);
        ticker.Show(price);
        alert.Check(price);
    }
}
