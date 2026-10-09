namespace dev.kaldiroglu.Visitor.Checkout.Solution;

/// <summary>
/// Runs one cart through stage two, stage three, the visitors and the switch over records.
/// <para>
/// The cart: a book for 40, food for 100, electronics for 500 — then a gift card for 100.
/// </para>
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Visitor.Demo -- checkout</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        List<Problem.IItem> three =
        [
            new Problem.Book("Novel", 40),
            new Problem.Food("Coffee", 100),
            new Problem.Electronics("Headphones", 500)
        ];
        List<Problem.IItem> four = new(three) { new Problem.GiftCard("Gift card", 100) };

        Console.WriteLine("Three items, correct tax 103");
        Console.WriteLine("  overloads:  tax " + new Problem.OverloadedTax().Total(three));
        Console.WriteLine("  type tests: tax " + new Problem.TypeTestTax().Total(three));

        Console.WriteLine("A gift card is added, correct tax 103, correct shipping 50");
        Console.WriteLine("  type tests: tax " + new Problem.TypeTestTax().Total(four)
                + ", shipping " + new Problem.TypeTestShipping().Total(four));

        var checkout = new Checkout(
        [
            new Book("Novel", 40),
            new Food("Coffee", 100),
            new Electronics("Headphones", 500),
            new GiftCard("Gift card", 100)
        ]);
        Console.WriteLine("  visitors:   tax " + checkout.Total(new TaxVisitor())
                + ", shipping " + checkout.Total(new ShippingVisitor()));

        List<Modern.IItem> records =
        [
            new Modern.Book("Novel", 40),
            new Modern.Food("Coffee", 100),
            new Modern.Electronics("Headphones", 500),
            new Modern.GiftCard("Gift card", 100)
        ];
        Console.WriteLine("  switch:     tax " + Modern.Tax.Total(records));

        Console.WriteLine("Receipt");
        foreach (var line in checkout.Lines(new ReceiptLineVisitor()))
        {
            Console.WriteLine("  " + line);
        }
    }
}
