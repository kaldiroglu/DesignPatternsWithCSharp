using dev.kaldiroglu.Decorator.Middleware.Domain;

namespace dev.kaldiroglu.Decorator.Middleware.Problem;

/// <summary>One concern, one class. So far so good.</summary>
public class LoggingPriceFeed : BasicPriceFeed
{
    private readonly CallLog _log;

    public LoggingPriceFeed(IPriceFeed supplier, CallLog log) : base(supplier) => _log = log;

    protected CallLog Log() => _log;

    public override Quote QuoteFor(string sku)
    {
        _log.Record("asking for " + sku);
        var quote = base.QuoteFor(sku);
        _log.Record("got " + quote);
        return quote;
    }
}
