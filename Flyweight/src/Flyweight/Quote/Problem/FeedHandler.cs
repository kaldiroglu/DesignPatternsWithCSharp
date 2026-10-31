namespace dev.kaldiroglu.Flyweight.Quote.Problem;

/// <summary>
/// Ingests ticks and keeps them. Every argument that describes the instrument is copied onto
/// every event and then held for as long as the event is held.
/// </summary>
public class FeedHandler
{
    private readonly List<Quote> _quotes = [];

    public void OnTick(string symbol, string exchange, string currency, string isin,
                       string sector, string companyName, decimal tickSize, int lotSize,
                       decimal bid, decimal ask, decimal last, long volume)
    {
        _quotes.Add(new Quote(symbol, exchange, currency, isin, sector, companyName,
            tickSize, lotSize, bid, ask, last, volume, DateTimeOffset.UtcNow));
    }

    public int Total() => _quotes.Count;

    public IReadOnlyList<Quote> Quotes() => _quotes.ToList();
}
