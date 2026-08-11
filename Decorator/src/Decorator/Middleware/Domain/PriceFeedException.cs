namespace dev.kaldiroglu.Decorator.Middleware.Domain;

/// <summary>
/// Every failure the feed can produce, with the one question a retry decorator needs to
/// ask of it.
/// </summary>
public abstract class PriceFeedException : Exception
{
    protected PriceFeedException(string message) : base(message)
    {
    }

    public abstract bool IsRetryable { get; }
}
