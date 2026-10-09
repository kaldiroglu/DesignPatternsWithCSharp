namespace dev.kaldiroglu.Strategy.Freight;

/// <summary>
/// Quotes a pillow and a box of books with four carriers whose rate cards have nothing in
/// common.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Strategy.Demo -- freight</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var pillow = new Shipment("TR", "TR", 900, 50, 40, 30);
        var books = new Shipment("TR", "TR", 4200, 25, 20, 10);

        var board = new CarrierBoard(
            new ByDesi("Yurtici", Money.Of("38.00"), 1),
            new ByWeightBand("Aras",
            [
                new ByWeightBand.Band(1000, Money.Of("45.00")),
                new ByWeightBand.Band(5000, Money.Of("70.00")),
                new ByWeightBand.Band(10000, Money.Of("110.00"))
            ], Money.Of("190.00")),
            new ByZone("UPS",
                new Dictionary<string, Money> { ["TR"] = Money.Of("60.00"), ["DE"] = Money.Of("240.00") },
                Money.Of("12.00"), 15),
            new FlatRate("Marketplace", Money.Of("89.90")));

        Console.WriteLine("The pillow: 900 g, but charged as " + pillow.ChargeableGrams + " g by volume");
        foreach (var quote in board.QuoteAll(pillow)) Console.WriteLine("  " + quote);
        Console.WriteLine("  Cheapest: " + board.CheapestFor(pillow).Carrier);

        Console.WriteLine("The books: " + books.ChargeableGrams + " g");
        foreach (var quote in board.QuoteAll(books)) Console.WriteLine("  " + quote);
        Console.WriteLine("  Cheapest: " + board.CheapestFor(books).Carrier);
    }
}
