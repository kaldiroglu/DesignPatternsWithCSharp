using System.Globalization;

namespace dev.kaldiroglu.Flyweight.Quote.Problem;

/// <summary>
/// Ingests a morning of ticks for four instruments and counts what is left in memory. Four
/// instruments. The count of distinct symbol objects retained is not four.
/// </summary>
public static class QuoteDemo
{
    public const int TicksPerInstrument = 25_000;

    public static void Run()
    {
        Wire.Reset();
        var feed = new FeedHandler();

        for (int i = 0; i < TicksPerInstrument; i++)
        {
            Tick(feed, "AAPL", "NASDAQ", "USD", "US0378331005", "Technology", "Apple Inc.",
                "0.01", 100, "190.00", "190.05", "190.02", 100);
            Tick(feed, "MSFT", "NASDAQ", "USD", "US5949181045", "Technology",
                "Microsoft Corporation", "0.01", 100, "420.10", "420.15", "420.12", 50);
            Tick(feed, "BMW.DE", "XETRA", "EUR", "DE0005190003", "Automotive",
                "Bayerische Motoren Werke AG", "0.01", 1, "90.50", "90.55", "90.52", 200);
            Tick(feed, "7203.T", "TSE", "JPY", "JP3633400001", "Automotive",
                "Toyota Motor Corporation", "0.5", 100, "3000", "3001", "3000", 1000);
        }

        var distinctSymbolObjects = new HashSet<string>(ReferenceEqualityComparer.Instance);
        foreach (Quote quote in feed.Quotes())
        {
            distinctSymbolObjects.Add(quote.Symbol());
        }

        Console.WriteLine($"Quotes ingested                     : {Grouped(feed.Total())}");
        Console.WriteLine("Instruments in the universe         : 4");
        Console.WriteLine($"Distinct symbol objects retained    : {Grouped(distinctSymbolObjects.Count)}");
        Console.WriteLine($"Strings decoded off the wire        : {Grouped(Wire.DecodeCount())}");
        Console.WriteLine();
        Console.WriteLine("Four instruments. One symbol object per tick, held forever.");
    }

    private static string Grouped(int count) => count.ToString("N0", CultureInfo.InvariantCulture);

    /// <summary>One tick, with every field arriving off the wire as a new object.</summary>
    private static void Tick(FeedHandler feed, string symbol, string exchange, string currency,
                             string isin, string sector, string companyName, string tickSize,
                             int lotSize, string bid, string ask, string last, long volume)
    {
        feed.OnTick(Wire.Decode(symbol), Wire.Decode(exchange), Wire.Decode(currency),
            Wire.Decode(isin), Wire.Decode(sector), Wire.Decode(companyName),
            Parse(tickSize), lotSize, Parse(bid), Parse(ask), Parse(last), volume);
    }

    private static decimal Parse(string value) => decimal.Parse(value, CultureInfo.InvariantCulture);
}
