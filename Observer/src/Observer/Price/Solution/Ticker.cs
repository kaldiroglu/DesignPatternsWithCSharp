namespace dev.kaldiroglu.Observer.Price.Solution;

/// <summary>A <b>ConcreteObserver</b>: shows the latest price and whether it went up or down.</summary>
public sealed class Ticker : IPriceListener
{
    private string shown = "";

    public void PriceChanged(PriceChange change)
    {
        string arrow = change.NewPrice > change.OldPrice ? "up" : "down";
        shown = change.Symbol + " " + change.NewPrice + " " + arrow;
    }

    public string Shown => shown;
}
