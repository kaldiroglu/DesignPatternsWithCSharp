namespace dev.kaldiroglu.Observer.Price.Problem;

/// <summary>A reader of the price: shows only the latest one.</summary>
public sealed class Ticker
{
    private volatile int shown;     // set by the polling thread, read by the caller

    public void Show(int price)
    {
        shown = price;
    }

    public int Shown => shown;
}
