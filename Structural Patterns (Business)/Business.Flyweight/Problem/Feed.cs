namespace DevKaldiroglu.DP.Structural.Flyweight.Problem;

public class Quote
{
    public string Symbol { get; }
    public string Exchange { get; }
    public string Currency { get; }
    public string Isin { get; }
    public string Sector { get; }
    public string CompanyName { get; }
    public decimal TickSize { get; }
    public int LotSize { get; }

    public decimal Bid { get; }
    public decimal Ask { get; }
    public decimal Last { get; }
    public long Volume { get; }
    public DateTimeOffset Timestamp { get; }

    public Quote(string symbol, string exchange, string currency, string isin,
                 string sector, string companyName, decimal tickSize, int lotSize,
                 decimal bid, decimal ask, decimal last, long volume, DateTimeOffset timestamp)
    {
        Symbol = symbol; Exchange = exchange; Currency = currency; Isin = isin;
        Sector = sector; CompanyName = companyName; TickSize = tickSize; LotSize = lotSize;
        Bid = bid; Ask = ask; Last = last; Volume = volume; Timestamp = timestamp;
    }
}

public class FeedHandler
{
    private readonly List<Quote> _quotes = new();

    public void OnTick(string symbol, string exchange, string currency, string isin,
                       string sector, string companyName, decimal tickSize, int lotSize,
                       decimal bid, decimal ask, decimal last, long volume) =>
        _quotes.Add(new Quote(symbol, exchange, currency, isin, sector, companyName,
            tickSize, lotSize, bid, ask, last, volume, DateTimeOffset.UtcNow));

    public int Total => _quotes.Count;
}

public static class ProblemDemo
{
    public static void Run()
    {
        var feed = new FeedHandler();
        for (int i = 0; i < 250_000; i++)
        {
            feed.OnTick("AAPL", "NASDAQ", "USD", "US0378331005", "Technology",
                "Apple Inc.", 0.01m, 100, 190.00m, 190.05m, 190.02m, 100);
            feed.OnTick("MSFT", "NASDAQ", "USD", "US5949181045", "Technology",
                "Microsoft Corporation", 0.01m, 100, 420.10m, 420.15m, 420.12m, 50);
            feed.OnTick("BMW.DE", "XETRA", "EUR", "DE0005190003", "Automotive",
                "Bayerische Motoren Werke AG", 0.01m, 1, 90.50m, 90.55m, 90.52m, 200);
            feed.OnTick("7203.T", "TSE", "JPY", "JP3633400001", "Automotive",
                "Toyota Motor Corporation", 0.5m, 100, 3000m, 3001m, 3000m, 1000);
        }
        Console.WriteLine($"Quotes ingested: {feed.Total}");
    }
}
