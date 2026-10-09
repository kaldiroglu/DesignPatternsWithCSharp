using System.Globalization;

namespace dev.kaldiroglu.Visitor.Interpreter;

/// <summary>
/// Builds one discount rule as a tree and interprets it against four products.
/// <para>
/// The tree is built by hand. Turning the text of a rule into this tree is parsing, and GoF
/// say the pattern does not cover it.
/// </para>
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Visitor.Demo -- interpreter</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        IRule discount = new Or(
            new And(new CategoryIs("books"), new PriceBelow(50)),
            new And(new CategoryIs("food"), new Not(new PriceBelow(20))));
        Console.WriteLine("Rule: " + discount.Describe());

        List<Product> products =
        [
            new Product("Novel", "books", 40),
            new Product("Atlas", "books", 80),
            new Product("Coffee", "food", 30),
            new Product("Tea", "food", 10)
        ];
        foreach (Product product in products)
        {
            Console.WriteLine("  " + product.Name + ", " + product.Category + ", "
                + product.Price.ToString(CultureInfo.InvariantCulture) + ": "
                + (discount.Interpret(product) ? "discount" : "no discount"));
        }
    }
}
