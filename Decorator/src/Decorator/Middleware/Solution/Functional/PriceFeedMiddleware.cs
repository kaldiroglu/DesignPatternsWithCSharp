using dev.kaldiroglu.Decorator.Middleware.Domain;

namespace dev.kaldiroglu.Decorator.Middleware.Solution.Functional;

/// <summary>
/// A decorator as a <b>function from component to component</b>.
/// <para>
/// This works only because <see cref="IPriceFeed"/> has one method: a lambda can satisfy
/// it, so there is no decorator class at all — abstract or concrete. GoF point this way in
/// implementation issue 2 (p. 179), which permits merging the abstract Decorator's
/// forwarding into the concrete one; a lambda takes that to its limit.
/// </para>
/// </summary>
public delegate IPriceFeed PriceFeedMiddleware(IPriceFeed next);

/// <summary>The concerns, as functions, and the fold that applies them.</summary>
public static class Middleware
{
    /// <summary>Applies middleware to a base feed, outermost first.</summary>
    public static IPriceFeed Apply(IPriceFeed baseFeed, IReadOnlyList<PriceFeedMiddleware> middleware)
    {
        var feed = baseFeed;
        for (var i = middleware.Count - 1; i >= 0; i--)
        {
            feed = middleware[i](feed);
        }

        return feed;
    }

    public static IPriceFeed Apply(IPriceFeed baseFeed, params PriceFeedMiddleware[] middleware) =>
        Apply(baseFeed, (IReadOnlyList<PriceFeedMiddleware>)middleware);

    public static PriceFeedMiddleware Logging(CallLog log, string name) =>
        next => new Lambda(sku =>
        {
            log.Record($"{name}: asking for {sku}");
            try
            {
                var quote = next.QuoteFor(sku);
                log.Record($"{name}: got {quote}");
                return quote;
            }
            catch (PriceFeedException e)
            {
                log.Record($"{name}: failed for {sku} — {e.Message}");
                throw;
            }
        });

    public static PriceFeedMiddleware Timing(IClock clock, Metrics metrics) =>
        next => new Lambda(sku =>
        {
            var startedAt = clock.Now();
            try
            {
                return next.QuoteFor(sku);
            }
            finally
            {
                metrics.Record(sku, clock.Now() - startedAt);
            }
        });

    public static PriceFeedMiddleware Retrying(int maxAttempts) =>
        next => new Lambda(sku =>
        {
            PriceFeedException? last = null;
            for (var attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    return next.QuoteFor(sku);
                }
                catch (PriceFeedException e)
                {
                    if (!e.IsRetryable)
                    {
                        throw;
                    }

                    last = e;
                }
            }

            throw last!;
        });

    public static PriceFeedMiddleware Caching(IClock clock, TimeSpan ttl)
    {
        // The state lives in the closure. One dictionary per pipeline built, which is the
        // same lifetime a CachingPriceFeed instance would have had.
        var quotes = new Dictionary<string, Quote>();
        var storedAt = new Dictionary<string, DateTimeOffset>();

        return next => new Lambda(sku =>
        {
            if (storedAt.TryGetValue(sku, out var at) && clock.Now() - at < ttl)
            {
                return quotes[sku];
            }

            var quote = next.QuoteFor(sku);
            quotes[sku] = quote;
            storedAt[sku] = clock.Now();
            return quote;
        });
    }

    /// <summary>
    /// C# cannot implement an interface with a bare lambda the way Java can, so this
    /// one-line adapter stands in for it. It is the only difference from the Java port.
    /// </summary>
    public sealed class Lambda : IPriceFeed
    {
        private readonly Func<string, Quote> _quoteFor;

        public Lambda(Func<string, Quote> quoteFor) => _quoteFor = quoteFor;

        public Quote QuoteFor(string sku) => _quoteFor(sku);
    }
}
