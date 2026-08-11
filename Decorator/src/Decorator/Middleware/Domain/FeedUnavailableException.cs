namespace dev.kaldiroglu.Decorator.Middleware.Domain;

/// <summary>The supplier did not answer. Trying again may well work.</summary>
public sealed class FeedUnavailableException : PriceFeedException
{
    public FeedUnavailableException(string message) : base(message)
    {
    }

    public override bool IsRetryable => true;
}
