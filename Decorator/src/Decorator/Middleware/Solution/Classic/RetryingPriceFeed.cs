using dev.kaldiroglu.Decorator.Middleware.Domain;

namespace dev.kaldiroglu.Decorator.Middleware.Solution.Classic;

/// <summary>
/// A decorator that forwards <b>many times</b> — and refuses to retry a failure that
/// cannot succeed.
/// </summary>
public sealed class RetryingPriceFeed : PriceFeedDecorator
{
    private readonly int _maxAttempts;

    public RetryingPriceFeed(IPriceFeed inner, int maxAttempts) : base(inner)
    {
        if (maxAttempts < 1)
        {
            throw new ArgumentException("maxAttempts must be at least 1", nameof(maxAttempts));
        }

        _maxAttempts = maxAttempts;
    }

    public override Quote QuoteFor(string sku)
    {
        PriceFeedException? last = null;
        for (var attempt = 1; attempt <= _maxAttempts; attempt++)
        {
            try
            {
                return Inner().QuoteFor(sku);
            }
            catch (PriceFeedException e)
            {
                if (!e.IsRetryable)
                {
                    throw; // an unknown SKU will still be unknown on the third try
                }

                last = e;
            }
        }

        throw last!;
    }
}
