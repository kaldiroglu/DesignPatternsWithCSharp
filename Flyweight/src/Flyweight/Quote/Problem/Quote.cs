namespace dev.kaldiroglu.Flyweight.Quote.Problem;

/// <summary>Every quote carries a full copy of the instrument metadata.</summary>
public class Quote
{
#pragma warning disable IDE0052 // Held on purpose: retaining these fields is the cost being measured.
    private readonly string _symbol;
    private readonly string _exchange;
    private readonly string _currency;
    private readonly string _isin;
    private readonly string _sector;
    private readonly string _companyName;
    private readonly decimal _tickSize;
    private readonly int _lotSize;

    private readonly decimal _bid;
    private readonly decimal _ask;
    private readonly decimal _last;
    private readonly long _volume;
    private readonly DateTimeOffset _timestamp;
#pragma warning restore IDE0052

    public Quote(string symbol, string exchange, string currency, string isin,
                 string sector, string companyName, decimal tickSize, int lotSize,
                 decimal bid, decimal ask, decimal last, long volume, DateTimeOffset timestamp)
    {
        _symbol = symbol;
        _exchange = exchange;
        _currency = currency;
        _isin = isin;
        _sector = sector;
        _companyName = companyName;
        _tickSize = tickSize;
        _lotSize = lotSize;
        _bid = bid;
        _ask = ask;
        _last = last;
        _volume = volume;
        _timestamp = timestamp;
    }

    public string Symbol() => _symbol;

    public decimal Last() => _last;
}
