namespace dev.kaldiroglu.Visitor.Checkout.Problem;

/// <summary>
/// Stages two and three on one cart. The overloads tax every item at the standard rate;
/// the type tests are right until a gift card is added, which falls into the else branch.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Visitor.Demo -- checkout-problem</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        List<IItem> cart = [new Book("Novel", 40), new Food("Coffee", 100), new Electronics("Headphones", 500)];
        Console.WriteLine("A book for 40, food for 100, electronics for 500: the correct tax is 2 + 1 + 100 = "
                + (40 * Rates.BookTax / 100 + 100 * Rates.FoodTax / 100 + 500 * Rates.ElectronicsTax / 100));
        Console.WriteLine("Stage two, overloads:   tax " + new OverloadedTax().Total(cart)
                + " (each item is held as an IItem, so TaxOf(IItem) is called)");
        Console.WriteLine("Stage three, type tests: tax " + new TypeTestTax().Total(cart)
                + ", shipping " + new TypeTestShipping().Total(cart));
        cart.Add(new GiftCard("Gift card", 100));
        Console.WriteLine("A gift card for 100 is added. It has no tax and is not shipped.");
        Console.WriteLine("Stage three, type tests: tax " + new TypeTestTax().Total(cart)
                + ", shipping " + new TypeTestShipping().Total(cart)
                + " (the else branch charges the standard rates)");
    }
}
