namespace dev.kaldiroglu.Strategy.Pricing.Problem;

/// <summary>Stage one: the campaign arrives as a string, and a typo fails only at run time.</summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Strategy.Demo -- pricing-problem-stage-one</c>.
/// </remarks>
public static class StageOneMain
{
    public static void Run()
    {
        var basket = SampleBasket.Build();
        var switching = new SwitchingCheckout();
        Console.WriteLine("Stage one, a switch on a string:");
        foreach (var campaign in new[] { "NONE", "STUDENT", "BLACKFRIDAY", "BUY2GET1" })
        {
            Console.WriteLine("  " + switching.Ring(basket, campaign));
        }
        try
        {
            switching.Ring(basket, "BLACK_FRIDAY");
        }
        catch (ArgumentException e)
        {
            // .NET adds " (Parameter 'campaign')" to the message; Java's message has no such suffix.
            Console.WriteLine("  A typo compiles and fails at run time: " + e.Message.Split(" (")[0]);
        }
    }
}
