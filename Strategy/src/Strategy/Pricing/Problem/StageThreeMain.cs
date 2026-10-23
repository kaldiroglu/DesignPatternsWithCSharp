namespace dev.kaldiroglu.Strategy.Pricing.Problem;

/// <summary>Stage three: each campaign is a checkout class of its own.</summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Strategy.Demo -- pricing-problem-stage-three</c>.
/// </remarks>
public static class StageThreeMain
{
    public static void Run()
    {
        Console.WriteLine("Stage three, a checkout class per campaign:");
        Console.WriteLine("  " + new StudentCheckout().Ring(SampleBasket.Build()));
    }
}
