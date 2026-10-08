namespace dev.kaldiroglu.Observer.Hw.Auction;

/// <summary>
/// A <b>ConcreteObserver</b> with a budget. When another bidder goes over its budget, it
/// stops watching — from inside its own update.
/// </summary>
public sealed class Bidder(string name, int budget) : IBidListener
{
    private readonly List<string> heard = [];

    public void NewBid(Auction auction, string bidder, int amount)
    {
        heard.Add(bidder + " " + amount);
        if (!bidder.Equals(name) && amount > budget)
        {
            heard.Add("too much for " + name + ", stops watching");
            auction.Unwatch(this);
        }
    }

    public IReadOnlyList<string> Heard => heard.ToList().AsReadOnly();
}
