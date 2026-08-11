using dev.kaldiroglu.Decorator.Middleware.Domain;

namespace dev.kaldiroglu.Decorator.Middleware.Solution.Classic;

/// <summary>
/// A decorator that refuses once a quota is spent.
/// <para>
/// Its position decides what the quota counts. Outside a cache it counts requests, cache
/// hits included; inside one it counts only the calls the supplier actually received —
/// which is what a supplier contract talks about.
/// </para>
/// </summary>
public sealed class RateLimitingPriceFeed : PriceFeedDecorator
{
    private readonly IClock _clock;
    private readonly int _limit;
    private readonly TimeSpan _window;
    private DateTimeOffset _windowStartedAt;

    public RateLimitingPriceFeed(IPriceFeed inner, IClock clock, int limit, TimeSpan window)
        : base(inner)
    {
        _clock = clock;
        _limit = limit;
        _window = window;
        _windowStartedAt = clock.Now();
    }

    public override Quote QuoteFor(string sku)
    {
        if (_clock.Now() - _windowStartedAt >= _window)
        {
            _windowStartedAt = _clock.Now();
            CallsInWindow = 0;
        }

        if (CallsInWindow >= _limit)
        {
            throw new RateLimitExceededException(_limit);
        }

        CallsInWindow++;
        return Inner().QuoteFor(sku);
    }

    public int CallsInWindow { get; private set; }
}
