using dev.kaldiroglu.Decorator.Middleware.Domain;

namespace dev.kaldiroglu.Decorator.Middleware.Solution.Classic;

/// <summary>A decorator that forwards <b>exactly once</b>: the familiar case.</summary>
public sealed class LoggingPriceFeed : PriceFeedDecorator
{
    private readonly CallLog _log;
    private readonly string _name;

    public LoggingPriceFeed(IPriceFeed inner, CallLog log) : this(inner, log, "feed")
    {
    }

    public LoggingPriceFeed(IPriceFeed inner, CallLog log, string name) : base(inner)
    {
        _log = log;
        _name = name;
    }

    public override Quote QuoteFor(string sku)
    {
        _log.Record($"{_name}: asking for {sku}");
        try
        {
            var quote = Inner().QuoteFor(sku);
            _log.Record($"{_name}: got {quote}");
            return quote;
        }
        catch (PriceFeedException e)
        {
            _log.Record($"{_name}: failed for {sku} — {e.Message}");
            throw; // a decorator observes; it does not swallow
        }
    }
}
