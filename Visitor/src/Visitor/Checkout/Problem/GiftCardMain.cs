namespace dev.kaldiroglu.Visitor.Checkout.Problem;

/// <summary>
/// Stage three after a gift card is added. The gift card falls into the else branch, so it
/// is taxed and shipped at the standard rates.
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/Visitor.Demo -- checkout-problem-gift-card</c>.
/// </remarks>
public static class GiftCardMain
{
    public static void Run()
    {
        var cart = SampleCart.ThreeItems();
        cart.Add(new GiftCard("Gift card", 100));
        Console.WriteLine("A gift card for 100 is added. It has no tax and is not shipped.");
        Console.WriteLine("Stage three, type tests: tax " + new TypeTestTax().Total(cart)
                + ", shipping " + new TypeTestShipping().Total(cart)
                + " (the else branch charges the standard rates)");
    }
}
