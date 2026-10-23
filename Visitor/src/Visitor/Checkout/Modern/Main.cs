namespace dev.kaldiroglu.Visitor.Checkout.Modern;

/// <summary>
/// The tax of the cart with an interface of records and a switch. The gift card has its own
/// case, so it is taxed 0.
/// <para>
/// <see cref="Tax.Of"/> needs a last <c>_</c> arm, and a fifth item kind
/// reaches that arm at run time instead (see <see cref="IItem"/>).
/// </para>
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/Visitor.Demo -- checkout-modern</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        List<IItem> cart =
        [
            new Book("Novel", 40), new Food("Coffee", 100),
            new Electronics("Headphones", 500), new GiftCard("Gift card", 100)
        ];
        foreach (IItem item in cart)
        {
            Console.WriteLine(item.Name + " " + item.Price + ": tax " + Tax.Of(item));
        }
        Console.WriteLine("Total tax: " + Tax.Total(cart));
    }
}
