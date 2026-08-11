using dev.kaldiroglu.Decorator.Middleware.Domain;

namespace dev.kaldiroglu.Decorator.Middleware.Problem;

/// <summary>
/// Naive design 2: one class that does everything, switched by flags.
/// <para>
/// A real improvement on design 1 — the logic exists once. What it does not fix is
/// anything else: five booleans are 32 configurations, every new concern edits a class the
/// others depend on, and the order is welded into one method. Read
/// <see cref="QuoteFor"/>: the cache is checked before the rate limiter, and retrying
/// happens inside both.
/// </para>
/// </summary>
public sealed class FlaggedPriceFeed : IPriceFeed
{
    private readonly IPriceFeed _supplier;
    private readonly IClock _clock;

    private readonly bool _loggingEnabled;
    private readonly bool _timingEnabled;
    private readonly bool _retryEnabled;
    private readonly bool _cachingEnabled;
    private readonly bool _rateLimitEnabled;

    private readonly CallLog _log;
    private readonly Metrics _metrics;
    private readonly int _maxAttempts;
    private readonly TimeSpan _cacheTtl;
    private readonly int _rateLimit;
    private readonly TimeSpan _rateLimitWindow;

    private readonly Dictionary<string, Quote> _cache = new();
    private readonly Dictionary<string, DateTimeOffset> _cachedAt = new();
    private DateTimeOffset _windowStartedAt;
    private int _callsInWindow;

    public FlaggedPriceFeed(IPriceFeed supplier, IClock clock,
        bool loggingEnabled, bool timingEnabled, bool retryEnabled,
        bool cachingEnabled, bool rateLimitEnabled,
        CallLog log, Metrics metrics, int maxAttempts,
        TimeSpan cacheTtl, int rateLimit, TimeSpan rateLimitWindow)
    {
        _supplier = supplier;
        _clock = clock;
        _loggingEnabled = loggingEnabled;
        _timingEnabled = timingEnabled;
        _retryEnabled = retryEnabled;
        _cachingEnabled = cachingEnabled;
        _rateLimitEnabled = rateLimitEnabled;
        _log = log;
        _metrics = metrics;
        _maxAttempts = maxAttempts;
        _cacheTtl = cacheTtl;
        _rateLimit = rateLimit;
        _rateLimitWindow = rateLimitWindow;
        _windowStartedAt = clock.Now();
    }

    /// <summary>Everything on, with the settings the order system happens to use.</summary>
    public static FlaggedPriceFeed FullyEnabled(
        IPriceFeed supplier, IClock clock, CallLog log, Metrics metrics) =>
        new(supplier, clock, true, true, true, true, true,
            log, metrics, 3, TimeSpan.FromSeconds(60), 10, TimeSpan.FromSeconds(1));

    public Quote QuoteFor(string sku)
    {
        var startedAt = _clock.Now();

        if (_loggingEnabled)
        {
            _log.Record("asked for " + sku);
        }

        if (_cachingEnabled &&
            _cachedAt.TryGetValue(sku, out var at) && _clock.Now() - at < _cacheTtl)
        {
            if (_loggingEnabled)
            {
                _log.Record("cache hit for " + sku);
            }

            if (_timingEnabled)
            {
                _metrics.Record(sku, _clock.Now() - startedAt);
            }

            return _cache[sku];
        }

        if (_rateLimitEnabled)
        {
            if (_clock.Now() - _windowStartedAt >= _rateLimitWindow)
            {
                _windowStartedAt = _clock.Now();
                _callsInWindow = 0;
            }

            if (_callsInWindow >= _rateLimit)
            {
                throw new RateLimitExceededException(_rateLimit);
            }

            _callsInWindow++;   // once per request — the retry loop below never sees it
        }

        var attempts = _retryEnabled ? _maxAttempts : 1;
        FeedUnavailableException? last = null;
        for (var attempt = 1; attempt <= attempts; attempt++)
        {
            try
            {
                var quote = _supplier.QuoteFor(sku);
                if (_cachingEnabled)
                {
                    _cache[sku] = quote;
                    _cachedAt[sku] = _clock.Now();
                }

                if (_timingEnabled)
                {
                    _metrics.Record(sku, _clock.Now() - startedAt);
                }

                if (_loggingEnabled)
                {
                    _log.Record("got " + quote);
                }

                return quote;
            }
            catch (FeedUnavailableException e)
            {
                last = e;
                if (_loggingEnabled)
                {
                    _log.Record($"attempt {attempt} failed for {sku}");
                }
            }
        }

        if (_timingEnabled)
        {
            _metrics.Record(sku, _clock.Now() - startedAt);
        }

        throw last!;
    }
}
