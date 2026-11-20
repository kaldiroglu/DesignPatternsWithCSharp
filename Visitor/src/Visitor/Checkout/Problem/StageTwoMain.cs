namespace dev.kaldiroglu.Visitor.Checkout.Problem;

/// <summary>
/// Stage two: one tax class with an overload for each kind. First each item is held as its own
/// type, so its own <c>TaxOf</c> is called and the tax is correct. Then the same items are held
/// as <see cref="IItem"/>s, as the cart holds them, so <c>TaxOf(IItem)</c> is called for every
/// item and every item is taxed at the standard rate.
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/Visitor.Demo -- checkout-problem-stage-two</c>.
/// </remarks>
public static class StageTwoMain
{
    public static void Run()
    {
        var tax = new OverloadedTax();
        Book book = new("Novel", 40);
        Food food = new("Coffee", 100);
        Electronics electronics = new("Headphones", 500);

        Console.WriteLine("Each item held as its own type, so TaxOf(Book), TaxOf(Food) and TaxOf(Electronics) are called:");
        Print(book, tax.TaxOf(book));
        Print(food, tax.TaxOf(food));
        Print(electronics, tax.TaxOf(electronics));
        Console.WriteLine("  total tax " + (tax.TaxOf(book) + tax.TaxOf(food) + tax.TaxOf(electronics)));

        List<IItem> cart = [book, food, electronics];
        Console.WriteLine("Each item held as an IItem, as in the cart, so TaxOf(IItem) is called:");
        foreach (IItem item in cart)
        {
            Print(item, tax.TaxOf(item));
        }
        Console.WriteLine("  total tax " + tax.Total(cart));
    }

    private static void Print(IItem item, int tax)
    {
        Console.WriteLine($"  {item.Name,-10} {item.Price,3}  tax {tax,3}");
    }
}
