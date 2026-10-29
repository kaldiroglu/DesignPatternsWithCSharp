using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using dev.kaldiroglu.Introduction.Problems.Pricing;
using dev.kaldiroglu.Introduction.Problems.Pricing.Domain;
using Xunit;

namespace dev.kaldiroglu.Introduction.Tests.Problems.Pricing;

/// <summary>
/// The one-method checkout the Introduction deck shows as a long switch: it prices
/// correctly, every campaign is a branch of one method, and a typo in a campaign name
/// fails only at run time.
/// </summary>
public class SwitchingCheckoutTests
{
    private static Basket StudentBasket() =>
        Basket.Of(Customer.Student("Ceyda"),
            new Line("java-book", "book", Money.Of("400.00"), 3));

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
        var text = File.ReadAllText(SourceFile());
        var code = text[text.IndexOf("public sealed class", StringComparison.Ordinal)..];
        code = Regex.Replace(code, @"/\*.*?\*/", "", RegexOptions.Singleline);
        code = Regex.Replace(code, @"//[^\n]*", "");

        // A switch expression has no `case` keyword; each campaign is an arm that starts with a string literal and an arrow, such as `"NONE" =>`.
        Assert.Equal(5, Regex.Matches(code, "\"[A-Z0-9_]+\"\\s*=>").Count);
        Assert.Single(Regex.Matches(code, @"Ring\("));
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
        }, PrintedBy(StageOneMain.Run));
    }

    /// <summary>Runs the action and returns what it printed, one line per element.</summary>
    private static string[] PrintedBy(Action action)
    {
        var original = Console.Out;
        var buffer = new StringWriter();
        Console.SetOut(buffer);
        try
        {
            action();
        }
        finally
        {
            Console.SetOut(original);
        }
        return buffer.ToString().Replace("\r\n", "\n").TrimEnd('\n').Split('\n');
    }

    /// <summary>
    /// <c>CallerFilePath</c> is filled in at the call site, so it is captured here, in this
    /// file, and the library's source is found from it.
    /// </summary>
    private static string SourceFile() =>
        Path.GetFullPath(Path.Combine(Path.GetDirectoryName(ThisFile())!,
            "..", "..", "..", "..", "src", "Introduction", "Problems", "Pricing",
            "SwitchingCheckout.cs"));

    private static string ThisFile([CallerFilePath] string path = "") => path;
}
