namespace dev.kaldiroglu.Observer.Hw.Auction;

/// <summary>
/// Homework 2: an auction that tells bidders about every new bid.
/// <para>
/// The homework question: what happens when a bidder decides to stop watching while it is
/// being told about a bid? It calls <see cref="Unwatch"/> from inside its own update. If
/// the auction looped over the real list, that would change the list during the loop and
/// throw an exception — <c>ConcurrentModificationException</c> in Java,
/// <see cref="InvalidOperationException"/> in C#. The auction loops over a copy, so the
/// change takes effect from the next bid.
/// </para>
/// </summary>
public sealed class Auction
{
    private readonly List<IBidListener> watchers = [];
    private int highest;
    private string leader = "nobody";

    public void Watch(IBidListener watcher)
    {
        watchers.Add(watcher);
    }

    public void Unwatch(IBidListener watcher)
    {
        watchers.Remove(watcher);
    }

    public void Bid(string bidder, int amount)
    {
        if (amount <= highest)
        {
            throw new ArgumentException(bidder + " must bid more than " + highest);
        }
        highest = amount;
        leader = bidder;
        foreach (var watcher in watchers.ToList())
        {
            watcher.NewBid(this, bidder, amount);
        }
    }

    public int Highest => highest;

    public string Leader => leader;

    public int WatcherCount => watchers.Count;
}
