using dev.kaldiroglu.Decorator.Middleware.Domain;

namespace dev.kaldiroglu.Decorator.Middleware.Problem;

/// <summary>
/// Three concerns, and the class name has to list them <b>and their order</b>.
/// <c>LoggingRetryingCaching…</c> would be a different class again. Covering five concerns
/// in every order would take 325 of these.
/// </summary>
public class CachingRetryingLoggingPriceFeed : RetryingLoggingPriceFeed
{
    private readonly IClock _clock;
    private readonly TimeSpan _ttl;
    private readonly Dictionary<string, Quote> _cache = new();
    private readonly Dictionary<string, DateTimeOffset> _cachedAt = new();

    public CachingRetryingLoggingPriceFeed(IPriceFeed supplier, int maxAttempts, CallLog log,
        IClock clock, TimeSpan ttl) : base(supplier, maxAttempts, log)
    {
        _clock = clock;
        _ttl = ttl;
    }

    public override Quote QuoteFor(string sku)
    {
        if (_cachedAt.TryGetValue(sku, out var at) && _clock.Now() - at < _ttl)
        {
            Log().Record("cache hit for " + sku);
            return _cache[sku];
        }

        var quote = base.QuoteFor(sku);
        _cache[sku] = quote;
        _cachedAt[sku] = _clock.Now();
        return quote;
    }
}
