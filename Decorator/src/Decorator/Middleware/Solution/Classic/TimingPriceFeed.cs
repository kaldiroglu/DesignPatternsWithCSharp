using dev.kaldiroglu.Decorator.Middleware.Domain;

namespace dev.kaldiroglu.Decorator.Middleware.Solution.Classic;

/// <summary>
/// A decorator that forwards <b>exactly once</b> and measures how long it took.
/// <para>
/// Where it sits decides what it measures: outside a cache it records what the caller
/// waited for, inside one it records what the supplier itself cost.
/// </para>
/// </summary>
public sealed class TimingPriceFeed : PriceFeedDecorator
{
    private readonly IClock _clock;
    private readonly Metrics _metrics;

    public TimingPriceFeed(IPriceFeed inner, IClock clock, Metrics metrics) : base(inner)
    {
        _clock = clock;
        _metrics = metrics;
    }

    public override Quote QuoteFor(string sku)
    {
        var startedAt = _clock.Now();
        try
        {
            return Inner().QuoteFor(sku);
        }
        finally
        {
            _metrics.Record(sku, _clock.Now() - startedAt);
        }
    }
}
