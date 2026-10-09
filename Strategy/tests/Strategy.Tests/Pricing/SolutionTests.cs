using dev.kaldiroglu.Strategy.Pricing.Domain;
using dev.kaldiroglu.Strategy.Pricing.Solution;
using Xunit;

namespace dev.kaldiroglu.Strategy.Tests.Pricing;

/// <summary>
/// The pattern applied to the same baskets the naive designs priced, and the operations they
/// could not perform. Ported from the Java <c>pricing.SolutionTest</c>.
/// </summary>
public class SolutionTests
{
    private const string Source = "Pricing/Solution/";

    private static Basket StudentBasket() =>
        Basket.Of(Customer.Student("Ceyda"),
            new Line("java-book", "book", Money.Of("400.00"), 3));

    private static CampaignBook Today() =>
        new(new ShelfPrice(),
            PercentageOff.Student(),
            PercentageOff.Staff(),
            TieredPercentageOff.BlackFriday(),
            new CheapestOfEveryThird("book"));

    [Fact(DisplayName = "every rule prices the basket the naive designs priced, to the lira")]
    public void TheSamePrices()
    {
        var basket = StudentBasket();

        Assert.Equal(Money.Of("1200.00"), new ShelfPrice().PriceFor(basket));
        Assert.Equal(Money.Of("960.00"), PercentageOff.Student().PriceFor(basket));
        Assert.Equal(Money.Of("720.00"), TieredPercentageOff.BlackFriday().PriceFor(basket));
        Assert.Equal(Money.Of("800.00"), new CheapestOfEveryThird("book").PriceFor(basket));
    }

    [Fact(DisplayName = "a rule is testable without a till, because it is handed a basket and nothing else")]
    public void ARuleNeedsNoContext()
    {
        var rule = TieredPercentageOff.BlackFriday();

        var small = Basket.Of(Customer.Shopper("Bora"),
            new Line("mug", "kitchen", Money.Of("100.00"), 2));
        var large = Basket.Of(Customer.Shopper("Bora"),
            new Line("kettle", "kitchen", Money.Of("600.00"), 2));

        Assert.Equal(Money.Of("150.00"), rule.PriceFor(small));   // under the threshold: 25%
        Assert.Equal(Money.Of("720.00"), rule.PriceFor(large));   // at or over it: 40%
    }

    [Fact(DisplayName = "the rule can be replaced on a till that already exists")]
    public void TheRuleIsAField()
    {
        var till = new Checkout(new ShelfPrice());
        var basket = StudentBasket();
        Assert.Equal(Money.Of("1200.00"), till.Ring(basket).Paid);

        till.SetRule(PercentageOff.Student());          // the same till, Thursday morning

        Assert.Equal("STUDENT", till.RuleName);
        Assert.Equal(Money.Of("960.00"), till.Ring(basket).Paid);
    }

    [Fact(DisplayName = "the receipt's promise costs nothing: the saving is a subtraction")]
    public void TheReceiptPromise()
    {
        var receipt = new Checkout(TieredPercentageOff.BlackFriday()).Ring(StudentBasket());

        Assert.Equal("BLACK_FRIDAY", receipt.Campaign);
        Assert.Equal(Money.Of("1200.00"), receipt.List);
        Assert.Equal(Money.Of("720.00"), receipt.Paid);
        Assert.Equal(Money.Of("480.00"), receipt.Saved);
    }

    [Fact(DisplayName = "one till prices one basket five ways, and names no campaign class to do it")]
    public void OneTillEveryCampaign()
    {
        var quotes = Today().QuoteAll(StudentBasket());

        Assert.Equal(5, quotes.Count);
        Assert.Equal(["NONE", "STUDENT", "STAFF", "BLACK_FRIDAY", "BUY_TWO_GET_ONE"],
            quotes.Select(q => q.Campaign).ToList());

        // The staff rule does not apply to a student, so it charges shelf price.
        Assert.Equal(Money.Of("1200.00"), quotes.First(q => q.Campaign == "STAFF").Paid);
    }

    [Fact(DisplayName = "the best campaign is chosen at run time, from a list handed in")]
    public void TheBestOfThem()
    {
        Assert.Equal("BLACK_FRIDAY", Today().BestFor(StudentBasket()).Name);

        // Under the threshold, Black Friday drops to 25% and a rule that is not a
        // percentage at all wins: three books at 300 is 900, and one of them comes off.
        var threeBooks = Basket.Of(Customer.Student("Ceyda"),
            new Line("poetry", "book", Money.Of("300.00"), 3));
        Assert.Equal(Money.Of("675.00"), TieredPercentageOff.BlackFriday().PriceFor(threeBooks));
        Assert.Equal(Money.Of("600.00"), new CheapestOfEveryThird("book").PriceFor(threeBooks));
        Assert.Equal("BUY_TWO_GET_ONE", Today().BestFor(threeBooks).Name);
    }

    /// <summary>
    /// The whole of a new campaign. Java writes it as an anonymous class inside the test;
    /// C# has no anonymous classes that implement an interface, so it is a nested class here.
    /// </summary>
    private sealed class NewYear : IPricingRule
    {
        public string Name => "NEW_YEAR";

        public Money PriceFor(Basket basket) => basket.ListTotal.Minus(Money.Of("500.00"));
    }

    [Fact(DisplayName = "a fourth campaign is one class, and nothing already written is touched")]
    public void AddingACampaignCostsOneClass()
    {
        IPricingRule newYear = new NewYear();

        var book = Today().Add(newYear);
        Assert.Equal(6, book.Size);
        Assert.Equal(Money.Of("700.00"), newYear.PriceFor(StudentBasket()));

        // And it wins, by twenty lira over Black Friday's 720.
        Assert.Equal("NEW_YEAR", book.BestFor(StudentBasket()).Name);
    }

    [Fact(DisplayName = "the context asks the rule and never asks which rule it is holding")]
    public void NoBranchInTheContext()
    {
        var body = SourceText.From(SourceText.Read(Source + "Checkout.cs"), "public sealed class");

        // Comments out first. This class documents what it does not do, so a plain search
        // over the file matches its own comments and proves nothing.
        var code = SourceText.StripComments(body);

        // Java looks for `instanceof`; the C# type tests are `is` and `as`.
        Assert.DoesNotContain(" is ", code);
        Assert.DoesNotContain(" as ", code);
        Assert.DoesNotContain("switch", code);
        Assert.DoesNotContain("STUDENT", code);
        Assert.Contains("_rule.PriceFor", code);
    }

    [Fact(DisplayName = "five rules carry the campaigns, and the interface has two methods")]
    public void TheArithmetic()
    {
        List<Type> rules =
            [typeof(ShelfPrice), typeof(PercentageOff), typeof(TieredPercentageOff), typeof(CheapestOfEveryThird)];

        Assert.All(rules, rule => Assert.True(typeof(IPricingRule).IsAssignableFrom(rule)));
        // Java counts two methods, name() and priceFor(). In C# name is the property Name,
        // whose getter get_Name is a method, so the count of methods is the same two.
        Assert.Equal(2, typeof(IPricingRule).GetMethods().Length);

        // Four classes cover five campaigns, because PercentageOff is one class used twice.
        Assert.Equal(4, rules.Count);
        Assert.Equal(5, Today().Size);
    }

    [Fact(DisplayName = "no campaign means the ShelfPrice rule, not a null")]
    public void NullIsNotACampaign()
    {
        Assert.Throws<ArgumentNullException>(() => new Checkout(null!));
        Assert.Equal(Money.Of("1200.00"), new Checkout(new ShelfPrice()).Ring(StudentBasket()).Paid);
    }
}
