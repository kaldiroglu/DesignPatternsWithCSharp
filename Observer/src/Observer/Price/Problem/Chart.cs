namespace dev.kaldiroglu.Observer.Price.Problem;

/// <summary>A reader of the price: draws every price it is given.</summary>
public sealed class Chart
{
    private readonly List<int> points = [];

    public void Add(int price)
    {
        points.Add(price);
    }

    public IReadOnlyList<int> Points => points.ToList().AsReadOnly();
}
