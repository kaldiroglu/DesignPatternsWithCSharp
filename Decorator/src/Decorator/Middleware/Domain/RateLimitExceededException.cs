namespace dev.kaldiroglu.Decorator.Middleware.Domain;

/// <summary>Our own quota for this supplier is used up for now.</summary>
public sealed class RateLimitExceededException : PriceFeedException
{
    public RateLimitExceededException(int limit)
        : base($"rate limit of {limit} calls per window exceeded")
    {
    }

    /// <summary>
    /// Retrying immediately would only burn the quota further. Whether to wait and try
    /// later is a decision for the caller, not for a retry loop with no sense of time.
    /// </summary>
    public override bool IsRetryable => false;
}
