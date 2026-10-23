namespace dev.kaldiroglu.Strategy.Pricing.Solution;

/// <summary>The campaign book chooses the best rule, and the till uses it.</summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Strategy.Demo -- pricing-best-rule</c>.
/// </remarks>
public static class BestRuleMain
{
    public static void Run()
    {
        var basket = SampleBasket.Build();
        var till = new Checkout(SampleCampaignBook.Build().BestFor(basket));
        Console.WriteLine("The till takes the best rule. Receipt:");
        Console.WriteLine("  " + till.Ring(basket));
    }
}
