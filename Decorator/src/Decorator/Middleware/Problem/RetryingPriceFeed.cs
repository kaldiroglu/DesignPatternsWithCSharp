using dev.kaldiroglu.Decorator.Middleware.Domain;

namespace dev.kaldiroglu.Decorator.Middleware.Problem;

/// <summary>A second concern, a second class — and no way to have both.</summary>
public class RetryingPriceFeed : BasicPriceFeed
{
    private readonly int _maxAttempts;

    public RetryingPriceFeed(IPriceFeed supplier, int maxAttempts) : base(supplier) =>
        _maxAttempts = maxAttempts;

    protected int MaxAttempts() => _maxAttempts;

    public override Quote QuoteFor(string sku)
    {
        FeedUnavailableException? last = null;
        for (var attempt = 1; attempt <= _maxAttempts; attempt++)
        {
            try
            {
                return base.QuoteFor(sku);
            }
            catch (FeedUnavailableException e)
            {
                last = e;
            }
        }

        throw last!;
    }
}
