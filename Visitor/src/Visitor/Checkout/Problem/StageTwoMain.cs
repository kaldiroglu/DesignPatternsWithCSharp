namespace dev.kaldiroglu.Visitor.Checkout.Problem;

/// <summary>
/// Stage two: one tax class with an overload for each kind. Each item is held as an
/// <see cref="IItem"/>, so <c>TaxOf(IItem)</c> is called and every item is taxed at the
/// standard rate.
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/Visitor.Demo -- checkout-problem-stage-two</c>.
/// </remarks>
public static class StageTwoMain
{
    public static void Run()
    {
        var cart = SampleCart.ThreeItems();
        Console.WriteLine("A book for 40, food for 100, electronics for 500: the correct tax is 2 + 1 + 100 = "
                + (40 * Rates.BookTax / 100 + 100 * Rates.FoodTax / 100 + 500 * Rates.ElectronicsTax / 100));
        Console.WriteLine("Stage two, overloads:   tax " + new OverloadedTax().Total(cart)
                + " (each item is held as an IItem, so TaxOf(IItem) is called)");
    }
}
