namespace dev.kaldiroglu.Observer.Price.Solution;

/// <summary>
/// The <b>Observer</b>: anything that wants to hear about price changes.
/// <para>
/// One method, so a lambda can be a listener too. In Java a lambda can implement this
/// interface directly. In C# a lambda can only become a delegate, so
/// <see cref="PriceFeed.Subscribe(Action{PriceChange})"/> takes an
/// <see cref="Action{T}"/> and wraps it in a listener.
/// </para>
/// </summary>
public interface IPriceListener
{
    void PriceChanged(PriceChange change);
}
