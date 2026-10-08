namespace dev.kaldiroglu.Observer.Price.Problem;

/// <summary>A reader of the price: shows only the latest one.</summary>
public sealed class Ticker
{
    private int shown;

    public void Show(int price)
    {
        shown = price;
    }

    public int Shown => shown;
}
