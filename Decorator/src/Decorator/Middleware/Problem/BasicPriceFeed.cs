using dev.kaldiroglu.Decorator.Middleware.Domain;

namespace dev.kaldiroglu.Decorator.Middleware.Problem;

/// <summary>Naive design 3, the root: the call, with nothing around it.</summary>
public class BasicPriceFeed : IPriceFeed
{
    private readonly IPriceFeed _supplier;

    public BasicPriceFeed(IPriceFeed supplier) => _supplier = supplier;

    public virtual Quote QuoteFor(string sku) => _supplier.QuoteFor(sku);
}
