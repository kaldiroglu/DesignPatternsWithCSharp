using dev.kaldiroglu.Strategy.Pricing.Domain;

namespace dev.kaldiroglu.Strategy.Pricing.Problem;

/// <summary>Prices Ceyda's basket with the three stages, and shows what each one costs.</summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Strategy.Demo -- pricing-problem</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var basket = Basket.Of(Customer.Student("Ceyda"),
            new Line("java-book", "book", Money.Of("400.00"), 3));

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

        Console.WriteLine("Stage two, a switch on an enum:");
        Console.WriteLine("  " + new EnumCheckout().Ring(basket, Campaign.STUDENT));

        Console.WriteLine("Stage three, a checkout class per campaign:");
        Console.WriteLine("  " + new StudentCheckout().Ring(basket));
        var till = new Till();
        Console.WriteLine("  Best for Ceyda: " + till.BestFor(basket));
        Console.WriteLine("  To choose, the till names " + till.CampaignsNamedHere
            + " checkout classes. A new campaign is an edit to the till.");
    }
}
