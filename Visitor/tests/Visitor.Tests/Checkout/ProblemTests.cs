using System.Reflection;
using Xunit;
using Methods = global::dev.kaldiroglu.Visitor.Checkout.Problem.Methods;
using P = global::dev.kaldiroglu.Visitor.Checkout.Problem;

namespace dev.kaldiroglu.Visitor.Tests.Checkout;

/// <summary>
/// The three designs of Part 1: each item computes its own tax, one class with an overload
/// per kind, and one class with type tests. Ported from the Java ProblemTest.
/// </summary>
public class ProblemTests
{
    private const string Source = "Checkout/Problem/";

    /// <summary>A book for 40, food for 100 and electronics for 500.</summary>
    internal static List<P.IItem> ThreeItems() =>
    [
        new P.Book("Novel", 40), new P.Food("Coffee", 100), new P.Electronics("Headphones", 500)
    ];

    /// <summary>The same cart, and a gift card for 100.</summary>
    internal static List<P.IItem> WithGiftCard() => [.. ThreeItems(), new P.GiftCard("Gift card", 100)];

    [Fact]
    public void TheRates()
    {
        Assert.Equal(5, P.Rates.BookTax);
        Assert.Equal(1, P.Rates.FoodTax);
        Assert.Equal(20, P.Rates.ElectronicsTax);
        Assert.Equal(20, P.Rates.StandardTax);
    }

    [Fact]
    public void EachItemComputesItsOwnTax()
    {
        var book = new Methods.Book("Novel", 40);
        var food = new Methods.Food("Coffee", 100);
        var electronics = new Methods.Electronics("Headphones", 500);

        Assert.Equal(2, book.Tax());
        Assert.Equal(1, food.Tax());
        Assert.Equal(100, electronics.Tax());
        Assert.Equal(103, book.Tax() + food.Tax() + electronics.Tax());
        Assert.Equal(50, book.ShippingCost() + food.ShippingCost() + electronics.ShippingCost());
        Assert.Equal("Novel 40 (tax 2)", book.ReceiptLine());
    }

    [Fact]
    public void EveryOperationOnEveryItem()
    {
        // Property getters (Name, Price) are special-name methods; the operations are not.
        var operations = typeof(Methods.IItem)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Where(m => !m.IsSpecialName)
            .Select(m => m.Name)
            .Order(StringComparer.Ordinal)
            .ToList();

        Assert.Equal(["ReceiptLine", "ShippingCost", "Tax"], operations);
    }

    [Fact]
    public void OverloadsChargeTheStandardRate()
    {
        var tax = new P.OverloadedTax();

        Assert.Equal(128, tax.Total(ThreeItems()));
        // Called with the exact static type, each overload is correct. The loop never does that.
        Assert.Equal(2, tax.TaxOf(new P.Book("Novel", 40)));
        Assert.Equal(1, tax.TaxOf(new P.Food("Coffee", 100)));
        P.IItem book = new P.Book("Novel", 40);
        Assert.Equal(8, tax.TaxOf(book)); // a book held as an IItem is taxed at 20 percent
    }

    [Fact]
    public void TypeTestsAreCorrectForKnownItems()
    {
        Assert.Equal(103, new P.TypeTestTax().Total(ThreeItems()));
        Assert.Equal(50, new P.TypeTestShipping().Total(ThreeItems()));
    }

    [Fact]
    public void TheGiftCardFallsIntoTheElse()
    {
        var tax = new P.TypeTestTax();

        Assert.Equal([2, 1, 100, 20], WithGiftCard().Select(tax.TaxOf).ToList());
        Assert.Equal(123, tax.Total(WithGiftCard()));
        Assert.Equal(60, new P.TypeTestShipping().Total(WithGiftCard()));
    }

    [Fact]
    public void TheItemIsNotSealed()
    {
        // Java asserts that Item is not sealed. A C# interface can never be sealed, so the
        // check here is that IItem is an interface any class may implement.
        Assert.True(typeof(P.IItem).IsInterface);
        Assert.False(typeof(P.IItem).IsSealed);

        // Java counts "instanceof"; the C# type test is "item is Book".
        var code = Printed.WithoutStrings(Printed.CodeOf(Source + "TypeTestTax.cs"));
        Assert.Equal(3, Printed.CountOf(code, @"\bis\s+[A-Z]"));
        Assert.Matches(@"else\s*\{", code);
    }
}
