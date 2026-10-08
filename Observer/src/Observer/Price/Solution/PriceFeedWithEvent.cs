namespace dev.kaldiroglu.Observer.Price.Solution;

/// <summary>
/// The same subject as <see cref="PriceFeed"/>, written the way a C# developer usually
/// writes it: with the <c>event</c> keyword. <b>This class has no Java counterpart.</b>
/// <para>
/// The language keeps the list of listeners. <c>+=</c> subscribes and <c>-=</c>
/// unsubscribes, and only this class can raise the event. A delegate is immutable, so
/// raising the event calls the listeners that were there when it started; a listener that
/// unsubscribes during a notification does not break it. That is the copy
/// <see cref="PriceFeed"/> makes by hand.
/// </para>
/// <para>
/// The roles are the same: this class is the Subject, and each handler is an Observer. The
/// difference is that an Observer here is a method, not an object that implements an
/// interface.
/// </para>
/// </summary>
public sealed class PriceFeedWithEvent(string symbol, int price)
{
    private int price = price;

    /// <summary>Raised after every change of the price, with the old and the new price.</summary>
    public event EventHandler<PriceChange>? PriceChanged;

    public void SetPrice(int newPrice)
    {
        if (newPrice == price)
        {
            return;                     // no change, no notification
        }
        var change = new PriceChange(symbol, price, newPrice);
        price = newPrice;
        PriceChanged?.Invoke(this, change);
    }

    public int Price => price;
}
