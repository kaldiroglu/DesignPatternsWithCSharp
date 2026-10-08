namespace dev.kaldiroglu.Observer.Price.Problem;

/// <summary>
/// Stage one: <b>the feed calls every reader itself.</b>
/// <para>
/// When the price changes, the feed tells the chart, the ticker and the alert at once. It
/// works, and no change is ever missed. What it costs:
/// </para>
/// <list type="bullet">
///   <item>The feed knows every reader by class. A fourth reader is an edit to the feed.</item>
///   <item>A reader cannot stop reading while the program runs; it is a field of the feed.</item>
///   <item>The feed cannot be reused, or tested, without all three readers.</item>
/// </list>
/// </summary>
public sealed class DirectPriceFeed(Chart chart, Ticker ticker, PriceAlert alert)
{
    private int price;

    public void SetPrice(int price)
    {
        this.price = price;
        chart.Add(price);
        ticker.Show(price);
        alert.Check(price);
    }

    public int Price => price;
}
