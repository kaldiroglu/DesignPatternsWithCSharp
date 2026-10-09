using dev.kaldiroglu.Strategy.Pricing.Domain;

namespace dev.kaldiroglu.Strategy.Pricing.Solution;

/// <summary>Prices Ceyda's basket with one till and five rules, and prints the best receipt.</summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Strategy.Demo -- pricing-solution</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var basket = Basket.Of(Customer.Student("Ceyda"),
            new Line("java-book", "book", Money.Of("400.00"), 3));
        var today = new CampaignBook(
            new ShelfPrice(),
            PercentageOff.Student(),
            PercentageOff.Staff(),
            TieredPercentageOff.BlackFriday(),
            new CheapestOfEveryThird("book"));

        Console.WriteLine("One till, " + today.Size + " rules, one basket:");
        foreach (var receipt in today.QuoteAll(basket))
        {
            Console.WriteLine("  " + receipt);
        }

        var till = new Checkout(today.BestFor(basket));
        Console.WriteLine("The till takes the best rule. Receipt:");
        Console.WriteLine("  " + till.Ring(basket));

        till.SetRule(PercentageOff.Student());
        Console.WriteLine("The same till, given another rule while it runs:");
        Console.WriteLine("  " + till.Ring(basket));
    }
}
