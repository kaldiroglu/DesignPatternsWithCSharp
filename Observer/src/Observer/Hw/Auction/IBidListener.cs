namespace dev.kaldiroglu.Observer.Hw.Auction;

/// <summary>The <b>Observer</b>: a bidder who wants to know about new bids.</summary>
public interface IBidListener
{
    void NewBid(Auction auction, string bidder, int amount);
}
