using dev.kaldiroglu.Decorator.Middleware.Domain;

namespace dev.kaldiroglu.Decorator.Middleware.Problem;

/// <summary>
/// Both concerns. A class cannot derive from two, so the logging is copied in by hand —
/// character for character, out of <see cref="LoggingPriceFeed"/>.
/// </summary>
public class RetryingLoggingPriceFeed : RetryingPriceFeed
{
    private readonly CallLog _log;

    public RetryingLoggingPriceFeed(IPriceFeed supplier, int maxAttempts, CallLog log)
        : base(supplier, maxAttempts) =>
        _log = log;

    protected CallLog Log() => _log;

    public override Quote QuoteFor(string sku)
    {
        _log.Record("asking for " + sku);   // copied,
        var quote = base.QuoteFor(sku);     // character
        _log.Record("got " + quote);        // for character
        return quote;
    }
}
