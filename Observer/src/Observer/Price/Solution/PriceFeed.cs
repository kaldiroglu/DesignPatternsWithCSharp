namespace dev.kaldiroglu.Observer.Price.Solution;

/// <summary>
/// The <b>Subject</b>: a feed that tells its listeners about every change.
/// <para>
/// It knows its listeners only as <see cref="IPriceListener"/>s — not the chart, the ticker
/// or the alert. Any object can subscribe or unsubscribe while the program runs. And
/// because the feed calls them at the moment the price changes, no change is ever missed
/// and nobody is late.
/// </para>
/// <para>
/// It notifies a copy of the list, so a listener that unsubscribes during a notification
/// does not break the loop.
/// </para>
/// </summary>
public sealed class PriceFeed(string symbol, int price)
{
    private int price = price;
    private readonly List<IPriceListener> listeners = [];

    public void Subscribe(IPriceListener listener)
    {
        listeners.Add(listener);
    }

    /// <summary>
    /// Subscribes a lambda. Java passes a lambda to the method above, because
    /// <c>PriceListener</c> is a functional interface. C# cannot turn a lambda into an
    /// interface, so this overload wraps it in a listener.
    /// </summary>
    /// <returns>The wrapper, so the caller can pass it to <see cref="Unsubscribe"/> later.</returns>
    public IPriceListener Subscribe(Action<PriceChange> listener)
    {
        var wrapper = new ActionListener(listener);
        listeners.Add(wrapper);
        return wrapper;
    }

    public void Unsubscribe(IPriceListener listener)
    {
        listeners.Remove(listener);
    }

    public void SetPrice(int newPrice)
    {
        if (newPrice == price)
        {
            return;                     // no change, no notification
        }
        var change = new PriceChange(symbol, price, newPrice);
        price = newPrice;
        foreach (var listener in listeners.ToList())
        {
            listener.PriceChanged(change);
        }
    }

    public int Price => price;

    /// <summary>An adapter: a listener that calls a delegate.</summary>
    private sealed class ActionListener(Action<PriceChange> action) : IPriceListener
    {
        public void PriceChanged(PriceChange change) => action(change);
    }
}
