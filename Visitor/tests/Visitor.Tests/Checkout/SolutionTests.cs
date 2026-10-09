using System.Reflection;
using Xunit;
using M = global::dev.kaldiroglu.Visitor.Checkout.Modern;
using S = global::dev.kaldiroglu.Visitor.Checkout.Solution;

namespace dev.kaldiroglu.Visitor.Tests.Checkout;

/// <summary>
/// The checkout with visitors, and the same check with records and a switch.
/// Ported from the Java SolutionTest.
/// </summary>
public class SolutionTests
{
    private const string Source = "Checkout/";

    private static readonly Type[] Items =
        [typeof(S.Book), typeof(S.Food), typeof(S.Electronics), typeof(S.GiftCard)];

    private static S.Checkout ThreeItems() =>
        new([new S.Book("Novel", 40), new S.Food("Coffee", 100), new S.Electronics("Headphones", 500)]);

    private static S.Checkout WithGiftCard() =>
        new([new S.Book("Novel", 40), new S.Food("Coffee", 100), new S.Electronics("Headphones", 500),
            new S.GiftCard("Gift card", 100)]);

    /// <summary>A visitor that returns the name of the kind of item it visits.</summary>
    private sealed class Names : S.IItemVisitor<string>
    {
        public string Visit(S.Book book) => "book";
        public string Visit(S.Food food) => "food";
        public string Visit(S.Electronics electronics) => "electronics";
        public string Visit(S.GiftCard giftCard) => "gift card";
    }

    /// <summary>A fifth kind of item, which the switch in Modern.Tax does not know.</summary>
    private sealed record Voucher(string Name, int Price) : M.IItem;

    [Fact]
    public void TheTax()
    {
        Assert.Equal(103, ThreeItems().Total(new S.TaxVisitor()));
        Assert.Equal(103, WithGiftCard().Total(new S.TaxVisitor()));
        Assert.Equal(0, new S.GiftCard("Gift card", 100).Accept(new S.TaxVisitor()));
    }

    [Fact]
    public void TheShipping()
    {
        Assert.Equal(50, ThreeItems().Total(new S.ShippingVisitor()));
        Assert.Equal(50, WithGiftCard().Total(new S.ShippingVisitor()));
    }

    [Fact]
    public void AVisitorReturnsWhatItNeeds()
    {
        Assert.Equal(
        [
            "Book        Novel 40",
            "Food        Coffee 100",
            "Electronics Headphones 500",
            "Gift card   Gift card 100 (sent by e-mail)"
        ], WithGiftCard().Lines(new S.ReceiptLineVisitor()));
    }

    [Fact]
    public void DoubleDispatch()
    {
        List<S.IItem> cart =
            [new S.Book("a", 1), new S.Food("b", 1), new S.Electronics("c", 1), new S.GiftCard("d", 1)];

        Assert.Equal(["book", "food", "electronics", "gift card"],
            cart.Select(item => item.Accept(new Names())).ToList());
    }

    [Fact]
    public void OneVisitPerItem()
    {
        var visits = typeof(S.IItemVisitor<>).GetMethods();

        Assert.Equal(4, visits.Length);
        Assert.All(visits, m => Assert.Equal("Visit", m.Name));
        // No default method, so every visitor must write each one.
        Assert.All(visits, m => Assert.True(m.IsAbstract));
        Assert.Equal(Items, visits
            .Select(m => m.GetParameters()[0].ParameterType)
            .OrderBy(t => Array.IndexOf(Items, t))
            .ToArray());
    }

    [Fact]
    public void EveryVisitorIsFinished()
    {
        foreach (var visitor in new[] { typeof(S.TaxVisitor), typeof(S.ShippingVisitor), typeof(S.ReceiptLineVisitor) })
        {
            foreach (var item in Items)
            {
                var visit = visitor.GetMethod("Visit",
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly, [item]);
                Assert.True(visit is not null, visitor.Name + " visits " + item.Name);
            }
        }
    }

    [Fact]
    public void NoTypeTests()
    {
        // Java checks for "instanceof" and "default". The C# type tests are "is" and casts,
        // and the C# fallback arm of a switch is "_ =>" as well as "default".
        foreach (var name in new[] { "TaxVisitor", "ShippingVisitor", "ReceiptLineVisitor", "Checkout" })
        {
            var code = Printed.WithoutStrings(Printed.CodeOf(Source + "Solution/" + name + ".cs"));
            Assert.False(Printed.CountOf(code, @"\bis\b") > 0, name + " has a type test");
            Assert.False(code.Contains("default"), name + " has a default");
            Assert.False(code.Contains("_ =>"), name + " has a discard arm");
        }
    }

    [Fact]
    public void TheSealedItem()
    {
        // Java asserts that Item is sealed and permits exactly four records. C# has no sealed
        // interface: Modern.IItem is open. The closest check is that the four records are the
        // only implementations in the library, and that each record is sealed.
        var item = typeof(M.IItem);
        Assert.True(item.IsInterface);
        Assert.False(item.IsSealed);

        var implementations = item.Assembly.GetTypes()
            .Where(t => item.IsAssignableFrom(t) && t != item)
            .OrderBy(t => Array.IndexOf(
                new[] { typeof(M.Book), typeof(M.Food), typeof(M.Electronics), typeof(M.GiftCard) }, t))
            .ToList();

        Assert.Equal([typeof(M.Book), typeof(M.Food), typeof(M.Electronics), typeof(M.GiftCard)], implementations);
        Assert.All(implementations, t => Assert.True(Printed.IsRecord(t) && t.IsSealed, t.Name));
    }

    [Fact]
    public void TheSwitch()
    {
        // Java: four "case" labels and no "default". The C# switch expression has one arm
        // for each of the four records, and needs a fifth arm, "_", which throws.
        var code = Printed.WithoutStrings(Printed.CodeOf(Source + "Modern/Tax.cs"));
        Assert.Equal(4, Printed.CountOf(code, @"^\s*(Book|Food|Electronics|GiftCard)\b[^\n]*=>"));
        Assert.Equal(1, Printed.CountOf(code, @"^\s*_\s*=>\s*throw\b"));
        Assert.DoesNotContain("default", code);

        List<M.IItem> cart =
        [
            new M.Book("Novel", 40), new M.Food("Coffee", 100),
            new M.Electronics("Headphones", 500), new M.GiftCard("Gift card", 100)
        ];
        Assert.Equal(103, M.Tax.Total(cart));
        Assert.Equal(103, M.Tax.Total(cart.Take(3).ToList()));

        // What the Java compiler rejects, C# compiles and rejects at run time.
        Assert.Throws<InvalidOperationException>(() => M.Tax.Of(new Voucher("Voucher", 10)));
    }

    [Fact]
    public void MainOutput()
    {
        var lines = Printed.By(S.Main.Run);

        Assert.Equal(
        [
            "Three items, correct tax 103",
            "  overloads:  tax 128",
            "  type tests: tax 103",
            "A gift card is added, correct tax 103, correct shipping 50",
            "  type tests: tax 123, shipping 60",
            "  visitors:   tax 103, shipping 50",
            "  switch:     tax 103",
            "Receipt",
            "  Book        Novel 40",
            "  Food        Coffee 100",
            "  Electronics Headphones 500",
            "  Gift card   Gift card 100 (sent by e-mail)"
        ], lines);
    }
}
