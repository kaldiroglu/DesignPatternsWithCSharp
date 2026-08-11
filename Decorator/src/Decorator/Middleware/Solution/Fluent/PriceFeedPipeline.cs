using dev.kaldiroglu.Decorator.Middleware.Domain;
using dev.kaldiroglu.Decorator.Middleware.Solution.Classic;

namespace dev.kaldiroglu.Decorator.Middleware.Solution.Fluent;

/// <summary>
/// The same classes as the classic chain, listed in the order a request travels.
/// <para>
/// A hand-written chain reads inside-out: the outermost decorator is written first and the
/// supplier is buried deepest. This builder fixes the reading order and changes nothing
/// else — <see cref="Build"/> simply applies the layers backwards, producing the identical
/// objects in the identical order.
/// </para>
/// <para>
/// Not in GoF, in any form. It is a readability decision, not a design one.
/// </para>
/// </summary>
public sealed class PriceFeedPipeline
{
    private readonly IPriceFeed _base;
    private readonly List<Func<IPriceFeed, IPriceFeed>> _layers = [];

    private PriceFeedPipeline(IPriceFeed baseFeed) => _base = baseFeed;

    public static PriceFeedPipeline Around(IPriceFeed baseFeed) => new(baseFeed);

    public PriceFeedPipeline WithLogging(CallLog log, string name)
    {
        _layers.Add(inner => new LoggingPriceFeed(inner, log, name));
        return this;
    }

    public PriceFeedPipeline WithTiming(IClock clock, Metrics metrics)
    {
        _layers.Add(inner => new TimingPriceFeed(inner, clock, metrics));
        return this;
    }

    public PriceFeedPipeline WithRetry(int maxAttempts)
    {
        _layers.Add(inner => new RetryingPriceFeed(inner, maxAttempts));
        return this;
    }

    public PriceFeedPipeline WithCache(IClock clock, TimeSpan ttl)
    {
        _layers.Add(inner => new CachingPriceFeed(inner, clock, ttl));
        return this;
    }

    public PriceFeedPipeline WithRateLimit(IClock clock, int limit, TimeSpan window)
    {
        _layers.Add(inner => new RateLimitingPriceFeed(inner, clock, limit, window));
        return this;
    }

    public IPriceFeed Build()
    {
        var feed = _base;
        for (var i = _layers.Count - 1; i >= 0; i--)
        {
            feed = _layers[i](feed);
        }

        return feed;
    }
}
