namespace dev.kaldiroglu.Strategy.Pricing.Solution;

/// <summary>One till prices one basket with all five rules.</summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/Strategy.Demo -- pricing-quote-all</c>.
/// </remarks>
public static class QuoteAllMain
{
    public static void Run()
    {
        var basket = SampleBasket.Build();
        var today = SampleCampaignBook.Build();
        Console.WriteLine("One till, " + today.Size + " rules, one basket:");
        foreach (var receipt in today.QuoteAll(basket))
        {
            Console.WriteLine("  " + receipt);
        }
    }
}
