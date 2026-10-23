namespace dev.kaldiroglu.Strategy.Pricing.Problem;

/// <summary>Stage two: the campaign is an enum, and the switch over it is exhaustive.</summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/Strategy.Demo -- pricing-problem-stage-two</c>.
/// </remarks>
public static class StageTwoMain
{
    public static void Run()
    {
        Console.WriteLine("Stage two, a switch on an enum:");
        Console.WriteLine("  " + new EnumCheckout().Ring(SampleBasket.Build(), Campaign.STUDENT));
    }
}
