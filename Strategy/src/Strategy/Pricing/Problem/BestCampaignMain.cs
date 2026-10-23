namespace dev.kaldiroglu.Strategy.Pricing.Problem;

/// <summary>
/// Marketing asks for the best campaign. At stage three the till needs one checkout per
/// campaign, so it names every checkout class.
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/Strategy.Demo -- pricing-problem-best-campaign</c>.
/// </remarks>
public static class BestCampaignMain
{
    public static void Run()
    {
        var till = new Till();
        Console.WriteLine("  Best for Ceyda: " + till.BestFor(SampleBasket.Build()));
        Console.WriteLine("  To choose, the till names " + till.CampaignsNamedHere
            + " checkout classes. A new campaign is an edit to the till.");
    }
}
