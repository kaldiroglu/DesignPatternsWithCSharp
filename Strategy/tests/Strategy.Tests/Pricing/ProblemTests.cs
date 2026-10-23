using System.Text.RegularExpressions;
using dev.kaldiroglu.Strategy.Pricing.Domain;
using dev.kaldiroglu.Strategy.Pricing.Problem;
using Xunit;

namespace dev.kaldiroglu.Strategy.Tests.Pricing;

/// <summary>
/// The three naive designs, and what each one costs. All three price correctly, so every
/// number the slides quote about them is measured here.
/// </summary>
public class ProblemTests
{
    private const string Source = "Pricing/Problem/";

    private static Basket StudentBasket() =>
        Basket.Of(Customer.Student("Ceyda"),
            new Line("java-book", "book", Money.Of("400.00"), 3));

    // --------------------------------------------------- stage one: a branch per campaign

    [Fact(DisplayName = "switch: it prices correctly, which is why nobody rewrites it")]
    public void TheSwitchWorks()
    {
        var till = new SwitchingCheckout();
        var basket = StudentBasket();

        Assert.Equal(Money.Of("1200.00"), till.Ring(basket, "NONE").Paid);
        Assert.Equal(Money.Of("960.00"), till.Ring(basket, "STUDENT").Paid);
        Assert.Equal(Money.Of("720.00"), till.Ring(basket, "BLACKFRIDAY").Paid);
        Assert.Equal(Money.Of("800.00"), till.Ring(basket, "BUY2GET1").Paid);
    }

    [Fact(DisplayName = "switch: the campaign is a string, so a typo is a run-time failure")]
    public void TheCampaignIsUnchecked()
    {
        Assert.Throws<ArgumentException>(
            () => new SwitchingCheckout().Ring(StudentBasket(), "BLACK_FRIDAY"));
    }

    [Fact(DisplayName = "switch: five campaigns, one method, and the threshold written where it is used")]
    public void EveryCampaignIsABranch()
    {
        var body = SourceText.From(SourceText.Read(Source + "SwitchingCheckout.cs"), "public sealed class");
        var code = SourceText.StripComments(body);

        // A switch expression has no `case` keyword; each campaign is an arm that starts with a string literal and an arrow, such as `"NONE" =>`.
        Assert.Equal(5, Regex.Matches(code, "\"[A-Z0-9_]+\"\\s*=>").Count);
        Assert.Equal(1, SourceText.CountOf(code, "Ring("));
    }

    // ------------------------------------------------------ stage two: the compiler helps

    [Fact(DisplayName = "enum: the same prices, and now the compiler checks the campaign")]
    public void TheEnumWorks()
    {
        var till = new EnumCheckout();
        var basket = StudentBasket();

        Assert.Equal(Money.Of("960.00"), till.Ring(basket, Campaign.STUDENT).Paid);
        Assert.Equal(Money.Of("720.00"), till.Ring(basket, Campaign.BLACK_FRIDAY).Paid);
        Assert.Equal(5, Enum.GetValues<Campaign>().Length);
    }

    [Fact(DisplayName = "enum: a campaign is still two edits, in two files that only the compiler joins")]
    public void ACampaignIsTwoEdits()
    {
        var body = SourceText.From(SourceText.Read(Source + "EnumCheckout.cs"), "public sealed class");
        var code = SourceText.StripComments(body);

        // One arm per constant, and no discard arm: adding a constant breaks this file
        // until somebody writes the branch. The arms are `Campaign.X =>` and the discard arm
        // would be `_ =>`.
        Assert.Equal(Enum.GetValues<Campaign>().Length,
            Regex.Matches(code, @"Campaign\.[A-Z_]+\s*=>").Count);
        Assert.Empty(Regex.Matches(code, @"\b_\s*=>"));
    }

    // --------------------------------------------- stage three: a class per campaign

    [Fact(DisplayName = "subclass: each rule is its own class, readable and testable on its own")]
    public void AClassPerCampaign()
    {
        var basket = StudentBasket();

        Assert.Equal(Money.Of("960.00"), new StudentCheckout().Ring(basket).Paid);
        Assert.Equal(Money.Of("720.00"), new BlackFridayCheckout().Ring(basket).Paid);
        Assert.True(typeof(Checkout).IsAbstract);
    }

    [Fact(DisplayName = "subclass: the till IS its campaign, so choosing costs the caller every class name")]
    public void TheReversal()
    {
        var till = new Till();
        var best = till.BestFor(StudentBasket());

        // It works: the customer is given the better of the campaigns.
        Assert.Equal("BLACK_FRIDAY", best.Campaign);
        Assert.Equal(Money.Of("720.00"), best.Paid);
        Assert.Equal(Money.Of("480.00"), best.Saved);

        // And this is what it cost. Adding a campaign edits Till.
        Assert.Equal(3, till.CampaignsNamedHere);

        // There is no operation that changes a checkout's campaign, because the campaign is
        // the object's class.
        Assert.DoesNotContain(typeof(Checkout).GetMethods(),
            m => m.Name.ToLowerInvariant().Contains("setcampaign")
                 || m.Name.ToLowerInvariant().Contains("setrule"));
        Assert.NotEqual(new StudentCheckout().GetType(), new BlackFridayCheckout().GetType());
    }

    [Fact(DisplayName = "all three designs put the same price on the same basket")]
    public void AllThreeAgree()
    {
        var basket = StudentBasket();
        List<Money> studentPrice =
        [
            new SwitchingCheckout().Ring(basket, "STUDENT").Paid,
            new EnumCheckout().Ring(basket, Campaign.STUDENT).Paid,
            new StudentCheckout().Ring(basket).Paid
        ];

        // They differ in design, not in what the customer pays.
        Assert.Single(studentPrice.Distinct());
    }

    [Fact(DisplayName = "StageOneMain prints the four campaigns and the typo that fails at run time")]
    public void StageOneMainPrintsTheStringSwitch()
    {
        Assert.Equal(new[]
        {
            "Stage one, a switch on a string:",
            "  NONE                   list  1200.00   paid  1200.00   saved     0.00",
            "  STUDENT                list  1200.00   paid   960.00   saved   240.00",
            "  BLACKFRIDAY            list  1200.00   paid   720.00   saved   480.00",
            "  BUY2GET1               list  1200.00   paid   800.00   saved   400.00",
            "  A typo compiles and fails at run time: unknown campaign: BLACK_FRIDAY"
        }, Printed.By(StageOneMain.Run));
    }

    [Fact(DisplayName = "StageTwoMain prints the student price from the enum switch")]
    public void StageTwoMainPrintsTheEnumSwitch()
    {
        Assert.Equal(new[]
        {
            "Stage two, a switch on an enum:",
            "  STUDENT                list  1200.00   paid   960.00   saved   240.00"
        }, Printed.By(StageTwoMain.Run));
    }

    [Fact(DisplayName = "StageThreeMain prints the student price from its own checkout class")]
    public void StageThreeMainPrintsOneCheckoutClass()
    {
        Assert.Equal(new[]
        {
            "Stage three, a checkout class per campaign:",
            "  STUDENT                list  1200.00   paid   960.00   saved   240.00"
        }, Printed.By(StageThreeMain.Run));
    }

    [Fact(DisplayName = "BestCampaignMain prints the best campaign and the three classes the till names")]
    public void BestCampaignMainPrintsTheTillsChoice()
    {
        Assert.Equal(new[]
        {
            "  Best for Ceyda: BLACK_FRIDAY           list  1200.00   paid   720.00   saved   480.00",
            "  To choose, the till names 3 checkout classes. A new campaign is an edit to the till."
        }, Printed.By(BestCampaignMain.Run));
    }
}
