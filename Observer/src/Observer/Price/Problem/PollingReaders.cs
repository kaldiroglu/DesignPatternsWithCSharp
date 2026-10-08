namespace dev.kaldiroglu.Observer.Price.Problem;

/// <summary>
/// Stage two: <b>the readers poll the feed.</b>
/// <para>
/// A real improvement on stage one: the feed knows nobody, and a new reader needs no change
/// to the feed. Every second, each reader asks for the price.
/// </para>
/// <para>
/// What it costs: a change is seen only at the next poll, so readers are late; most polls
/// find the same price; and a change that is undone before the next poll is never seen.
/// </para>
/// </summary>
public sealed class PollingReaders(PriceFeed feed, Chart chart, Ticker ticker, PriceAlert alert)
{
    /// <summary>Called by a timer, once a second.</summary>
    public void Poll()
    {
        chart.Add(feed.Price());
        ticker.Show(feed.Price());
        alert.Check(feed.Price());
    }
}
