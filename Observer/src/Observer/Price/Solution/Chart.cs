namespace dev.kaldiroglu.Observer.Price.Solution;

/// <summary>A <b>ConcreteObserver</b>: draws every new price.</summary>
public sealed class Chart : IPriceListener
{
    private readonly List<int> points = [];

    public void PriceChanged(PriceChange change)
    {
        points.Add(change.NewPrice);
    }

    public IReadOnlyList<int> Points => points.ToList().AsReadOnly();
}
