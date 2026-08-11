namespace dev.kaldiroglu.Decorator.Middleware.Domain;

/// <summary>There is no such item. Trying again will produce the same answer.</summary>
public sealed class UnknownSkuException : PriceFeedException
{
    public UnknownSkuException(string sku) : base("unknown sku: " + sku)
    {
    }

    public override bool IsRetryable => false;
}
