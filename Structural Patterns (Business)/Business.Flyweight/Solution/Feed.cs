using System.Collections.Concurrent;

namespace DevKaldiroglu.DP.Structural.Flyweight.Solution;

/// <summary>Intrinsic, immutable, shareable. Constructor is internal so only
/// <see cref="InstrumentRegistry"/> can mint instances — guarantees identity equality.</summary>
public sealed class Instrument
{
    public string Symbol { get; }
    public string Exchange { get; }
    public string Currency { get; }
    public string Isin { get; }
    public string Sector { get; }
    public string CompanyName { get; }
    public decimal TickSize { get; }
    public int LotSize { get; }
    public TimeOnly SessionOpen { get; }
    public TimeOnly SessionClose { get; }

    internal Instrument(string symbol, string exchange, string currency, string isin,
        string sector, string companyName, decimal tickSize, int lotSize,
        TimeOnly sessionOpen, TimeOnly sessionClose)
    {
        Symbol = symbol; Exchange = exchange; Currency = currency; Isin = isin;
        Sector = sector; CompanyName = companyName; TickSize = tickSize; LotSize = lotSize;
        SessionOpen = sessionOpen; SessionClose = sessionClose;
    }
}

public class InstrumentRegistry
{
    private readonly ConcurrentDictionary<string, Instrument> _cache = new();

    public Instrument Get(string symbol, string exchange, string currency, string isin,
        string sector, string companyName, decimal tickSize, int lotSize,
        TimeOnly sessionOpen, TimeOnly sessionClose)
    {
        var key = $"{symbol}@{exchange}";
        return _cache.GetOrAdd(key, _ => new Instrument(symbol, exchange, currency, isin,
            sector, companyName, tickSize, lotSize, sessionOpen, sessionClose));
    }

    public int Size => _cache.Count;
}

public sealed record Quote(
    Instrument Instrument,
    decimal Bid,
    decimal Ask,
    decimal Last,
    long Volume,
    DateTimeOffset Timestamp);

public class FeedHandler
{
    private readonly List<Quote> _quotes = new();
    private readonly InstrumentRegistry _registry;

    public FeedHandler(InstrumentRegistry registry) => _registry = registry;

    public void OnTick(Instrument instrument, decimal bid, decimal ask, decimal last, long volume) =>
        _quotes.Add(new Quote(instrument, bid, ask, last, volume, DateTimeOffset.UtcNow));

    public int Total => _quotes.Count;
    public int DistinctInstruments => _registry.Size;
}

public static class SolutionDemo
{
    public static void Run()
    {
        var registry = new InstrumentRegistry();
        var feed = new FeedHandler(registry);

        var aapl = registry.Get("AAPL", "NASDAQ", "USD", "US0378331005", "Technology",
            "Apple Inc.", 0.01m, 100, new TimeOnly(9, 30), new TimeOnly(16, 0));
        var msft = registry.Get("MSFT", "NASDAQ", "USD", "US5949181045", "Technology",
            "Microsoft Corporation", 0.01m, 100, new TimeOnly(9, 30), new TimeOnly(16, 0));
        var bmw = registry.Get("BMW.DE", "XETRA", "EUR", "DE0005190003", "Automotive",
            "Bayerische Motoren Werke AG", 0.01m, 1, new TimeOnly(9, 0), new TimeOnly(17, 30));
        var toyota = registry.Get("7203.T", "TSE", "JPY", "JP3633400001", "Automotive",
            "Toyota Motor Corporation", 0.5m, 100, new TimeOnly(9, 0), new TimeOnly(15, 0));

        for (int i = 0; i < 250_000; i++)
        {
            feed.OnTick(aapl,   190.00m, 190.05m, 190.02m, 100);
            feed.OnTick(msft,   420.10m, 420.15m, 420.12m, 50);
            feed.OnTick(bmw,    90.50m,  90.55m,  90.52m,  200);
            feed.OnTick(toyota, 3000m,   3001m,   3000m,   1000);
        }

        Console.WriteLine($"Quotes ingested:        {feed.Total}");
        Console.WriteLine($"Distinct Instruments:   {feed.DistinctInstruments}");

        var aaplAgain = registry.Get("AAPL", "NASDAQ", "USD", "US0378331005", "Technology",
            "Apple Inc.", 0.01m, 100, new TimeOnly(9, 30), new TimeOnly(16, 0));
        Console.WriteLine($"Identity reuse (AAPL):  {ReferenceEquals(aapl, aaplAgain)}");
    }
}
