using dev.kaldiroglu.Decorator.Middleware.Domain;

namespace dev.kaldiroglu.Decorator.Middleware.Solution.Classic;

/// <summary>
/// A decorator that forwards <b>zero times</b> on a hit. Everything wrapped inside it —
/// the retry, the rate limiter, the supplier — simply does not run.
/// </summary>
public sealed class CachingPriceFeed : PriceFeedDecorator
{
    private sealed record Entry(Quote Quote, DateTimeOffset StoredAt);

    private readonly IClock _clock;
    private readonly TimeSpan _ttl;
    private readonly Dictionary<string, Entry> _entries = new();

    public CachingPriceFeed(IPriceFeed inner, IClock clock, TimeSpan ttl) : base(inner)
    {
        _clock = clock;
        _ttl = ttl;
    }

    public override Quote QuoteFor(string sku)
    {
        if (_entries.TryGetValue(sku, out var entry) && _clock.Now() - entry.StoredAt < _ttl)
        {
            Hits++;
            return entry.Quote;
        }

        Misses++;
        var quote = Inner().QuoteFor(sku);
        _entries[sku] = new Entry(quote, _clock.Now());
        return quote;

        // A failure is deliberately not cached: the next caller should get a fresh attempt.
    }

    public int Hits { get; private set; }

    public int Misses { get; private set; }
}
