using dev.kaldiroglu.Decorator.Middleware.Domain;

namespace dev.kaldiroglu.Decorator.Middleware.Problem;

/// <summary>
/// Naive design 1: the cross-cutting code written where each call is made.
/// <para>
/// The two methods began as one. Nothing keeps two copies in step, so every edit since has
/// landed in one of them: different retry counts, one path that never logs its failures,
/// and a reorder cache that never writes a timestamp and therefore never hits.
/// </para>
/// </summary>
public sealed class CopyPasteOrderService
{
    private static readonly TimeSpan CacheTtl = TimeSpan.FromSeconds(60);

    private readonly IPriceFeed _feed;
    private readonly IClock _clock;
    private readonly CallLog _log;
    private readonly Dictionary<string, Quote> _cache = new();
    private readonly Dictionary<string, DateTimeOffset> _cachedAt = new();

    public CopyPasteOrderService(IPriceFeed feed, IClock clock, CallLog log)
    {
        _feed = feed;
        _clock = clock;
        _log = log;
    }

    public Quote PriceForOrder(string sku)
    {
        if (_cachedAt.TryGetValue(sku, out var cachedTime) && _clock.Now() - cachedTime < CacheTtl)
        {
            _log.Record("order: cache hit for " + sku);
            return _cache[sku];
        }

        FeedUnavailableException? last = null;
        for (var attempt = 1; attempt <= 3; attempt++)
        {
            try
            {
                _log.Record($"order: calling supplier for {sku}, attempt {attempt}");
                var quote = _feed.QuoteFor(sku);
                _cache[sku] = quote;
                _cachedAt[sku] = _clock.Now();
                return quote;
            }
            catch (FeedUnavailableException e)
            {
                last = e;
                _log.Record($"order: attempt {attempt} failed for {sku}");
            }
        }

        throw last!;
    }

    public Quote PriceForReorder(string sku)
    {
        // <= rather than <, three drifted differences below, and no timestamp written.
        if (_cachedAt.TryGetValue(sku, out var cachedTime) && _clock.Now() - cachedTime <= CacheTtl)
        {
            _log.Record("reorder: cache hit for " + sku);
            return _cache[sku];
        }

        FeedUnavailableException? last = null;
        for (var attempt = 1; attempt <= 2; attempt++) // two attempts, not three
        {
            try
            {
                _log.Record($"reorder: calling supplier for {sku}, attempt {attempt}");
                var quote = _feed.QuoteFor(sku);
                _cache[sku] = quote;    // and no cachedAt, so this cache never hits
                return quote;
            }
            catch (FeedUnavailableException e)
            {
                last = e;               // and the failure is never logged
            }
        }

        throw last!;
    }
}
