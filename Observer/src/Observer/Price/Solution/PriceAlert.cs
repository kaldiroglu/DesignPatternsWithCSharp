namespace dev.kaldiroglu.Observer.Price.Solution;

/// <summary>A <b>ConcreteObserver</b>: fires once when the price reaches its limit.</summary>
public sealed class PriceAlert(int limit) : IPriceListener
{
    private bool fired;

    public void PriceChanged(PriceChange change)
    {
        if (change.NewPrice >= limit)
        {
            fired = true;
        }
    }

    public bool Fired => fired;
}
