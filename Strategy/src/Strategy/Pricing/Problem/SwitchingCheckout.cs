using dev.kaldiroglu.Strategy.Pricing.Domain;

namespace dev.kaldiroglu.Strategy.Pricing.Problem;

/// <summary>
/// Stage one: one method, and a branch per campaign.
/// <para>
/// This is what the till looks like after the third campaign and before anybody has time to
/// think. It works. Every price it produces is correct, and for one or two campaigns it is
/// the clearest thing in the file.
/// </para>
/// <para><b>What it costs:</b></para>
/// <list type="bullet">
///   <item><description><b>Every campaign is a branch, and the branches share a method.</b>
///       Adding the staff discount means editing a method that already works for four other
///       campaigns. The compiler cannot help: the campaign arrives as a <c>string</c>.</description></item>
///   <item><description><b>No rule can be tested on its own.</b> To check the Black Friday tier
///       you must build a basket, call the till, and hope nothing above the branch
///       interfered.</description></item>
///   <item><description><b>The rules leak.</b> Look how many times the 1000-lira threshold
///       appears below, and note that <c>STUDENT</c> silently caps at a different figure that
///       nobody has written down anywhere else.</description></item>
/// </list>
/// </summary>
public sealed class SwitchingCheckout
{
    public Receipt Ring(Basket basket, string campaign)
    {
        var list = basket.ListTotal;
        var paid = campaign switch
        {
            "NONE" => list,
            "STUDENT" => basket.Customer.IsStudent ? list.PercentOff(20) : list,
            "STAFF" => basket.Customer.IsStaff ? list.PercentOff(30) : list,
            "BLACKFRIDAY" => list.IsAtLeast(Money.Of("1000.00"))
                ? list.PercentOff(40)
                : list.PercentOff(25),
            "BUY2GET1" => BuyTwoGetOne(basket, list),
            _ => throw new ArgumentException("unknown campaign: " + campaign, nameof(campaign))
        };
        return new Receipt(campaign, list, paid);
    }

    // Java's switch expression can hold a block that ends in `yield`; C#'s cannot, so the
    // fifth branch's loop lives here. It is still this class's business and nobody else's.
    private static Money BuyTwoGetOne(Basket basket, Money list)
    {
        var discount = Money.Zero;
        foreach (var line in basket.InCategory("book"))
        {
            var free = line.Quantity / 3;
            discount = discount.Plus(line.UnitPrice.Times(free));
        }
        return list.Minus(discount);
    }
}
