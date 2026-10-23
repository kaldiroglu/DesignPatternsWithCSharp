namespace dev.kaldiroglu.Strategy.Pricing.Solution;

/// <summary>The rule is a field, so the same till can be given another rule while it runs.</summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/Strategy.Demo -- pricing-change-rule</c>.
/// </remarks>
public static class ChangeRuleMain
{
    public static void Run()
    {
        var basket = SampleBasket.Build();
        var till = new Checkout(SampleCampaignBook.Build().BestFor(basket));
        till.SetRule(PercentageOff.Student());
        Console.WriteLine("The same till, given another rule while it runs:");
        Console.WriteLine("  " + till.Ring(basket));
    }
}
