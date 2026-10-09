namespace dev.kaldiroglu.Visitor.Checkout.Problem.Methods;

/// <summary>
/// Stage one: each item computes its own tax, shipping cost and receipt line. The figures
/// are right; the cost is that every new operation is an edit to every item class.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Visitor.Demo -- checkout-methods</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        List<IItem> cart = [new Book("Novel", 40), new Food("Coffee", 100), new Electronics("Headphones", 500)];
        int tax = 0;
        int shipping = 0;
        foreach (IItem item in cart)
        {
            Console.WriteLine(item.ReceiptLine());
            tax += item.Tax();
            shipping += item.ShippingCost();
        }
        Console.WriteLine("Tax " + tax + ", shipping " + shipping);
        Console.WriteLine("Three operations, written in each of the three item classes.");
    }
}
