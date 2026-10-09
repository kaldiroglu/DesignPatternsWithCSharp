using Xunit;
using I = global::dev.kaldiroglu.Visitor.Interpreter;

namespace dev.kaldiroglu.Visitor.Tests.Interpreter;

/// <summary>The shop's rule language, taught as the Interpreter section of the Visitor deck.</summary>
public class RuleTests
{
    /// <summary>(category is books and price below 50) or (category is food and not price below 20)</summary>
    private static I.IRule Discount() =>
        new I.Or(
            new I.And(new I.CategoryIs("books"), new I.PriceBelow(50)),
            new I.And(new I.CategoryIs("food"), new I.Not(new I.PriceBelow(20))));

    [Fact]
    public void FourProducts()
    {
        var rule = Discount();

        Assert.True(rule.Interpret(new I.Product("Novel", "books", 40)));
        Assert.False(rule.Interpret(new I.Product("Atlas", "books", 80)));
        Assert.True(rule.Interpret(new I.Product("Coffee", "food", 30)));
        Assert.False(rule.Interpret(new I.Product("Tea", "food", 10)));
    }

    [Fact]
    public void TheLimits()
    {
        var rule = Discount();

        Assert.False(rule.Interpret(new I.Product("Book", "books", 50)), "50 is not below 50");
        Assert.True(rule.Interpret(new I.Product("Rice", "food", 20)), "food at 20 or more");
        Assert.False(rule.Interpret(new I.Product("Lamp", "home", 5)), "another category");
    }

    [Fact]
    public void Describe()
    {
        Assert.Equal("((category is books and price below 50) or "
                     + "(category is food and not price below 20))", Discount().Describe());
    }

    [Fact]
    public void FiveRuleClasses()
    {
        // Java asserts that Rule is sealed and permits exactly five records. C# has no sealed
        // interface, so IRule is open; the check is that the library has exactly these five
        // implementations, each a sealed record.
        var rule = typeof(I.IRule);
        Assert.True(rule.IsInterface);
        Type[] expected = [typeof(I.CategoryIs), typeof(I.PriceBelow), typeof(I.And), typeof(I.Or), typeof(I.Not)];

        var implementations = rule.Assembly.GetTypes()
            .Where(t => rule.IsAssignableFrom(t) && t != rule)
            .OrderBy(t => Array.IndexOf(expected, t))
            .ToArray();

        Assert.Equal(expected, implementations);
        Assert.All(implementations, t => Assert.True(Printed.IsRecord(t) && t.IsSealed, t.Name));
    }

    [Fact]
    public void MainOutput()
    {
        var lines = Printed.By(I.Main.Run);

        Assert.Equal(
        [
            "Rule: ((category is books and price below 50) or (category is food and not price below 20))",
            "  Novel, books, 40: discount",
            "  Atlas, books, 80: no discount",
            "  Coffee, food, 30: discount",
            "  Tea, food, 10: no discount"
        ], lines);
    }
}
