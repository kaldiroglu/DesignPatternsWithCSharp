using dev.kaldiroglu.Strategy.Pricing.Domain;

namespace dev.kaldiroglu.Strategy.Pricing.Problem;

/// <summary>
/// Stage two: the same branches, over an enum the compiler checks.
/// <para>
/// This is better than stage one and the improvement is real. The switch is exhaustive, so
/// adding a constant to <see cref="Campaign"/> makes this file stop compiling until the new
/// case is written — the compiler now tells you what you forgot, which is exactly what it
/// failed to do before. (In C# a missing case is warning CS8509, which this project's
/// <c>.csproj</c> raises to an error so that the claim holds here as it does in Java.)
/// </para>
/// <para><b>What it still costs:</b></para>
/// <list type="bullet">
///   <item><description><b>A campaign is still two edits in two files.</b> One in the enum, one
///       here, and they are only kept in step by the compiler noticing.</description></item>
///   <item><description><b>The pricing logic and the campaign list are the same class's business.</b>
///       The till knows every rule the company has ever run, which is why this file is the
///       one that changes every Thursday.</description></item>
///   <item><description><b>Nothing outside this file can add a campaign.</b> Marketing cannot, a
///       test cannot, and a regional store cannot. A rule the company invents is a
///       release.</description></item>
/// </list>
/// </summary>
public sealed class EnumCheckout
{
    public Receipt Ring(Basket basket, Campaign campaign)
    {
        var list = basket.ListTotal;
        // A C# enum can hold a value no constant names, so a switch with a case for every
        // constant still draws warning CS8524. A default case would silence it, and would
        // also silence CS8509, the warning that names a forgotten constant. So there is no
        // default, and only CS8524 is suppressed.
#pragma warning disable CS8524
        var paid = campaign switch
        {
            Campaign.NONE => list,
            Campaign.STUDENT => basket.Customer.IsStudent ? list.PercentOff(20) : list,
            Campaign.STAFF => basket.Customer.IsStaff ? list.PercentOff(30) : list,
            Campaign.BLACK_FRIDAY => list.IsAtLeast(Money.Of("1000.00"))
                ? list.PercentOff(40)
                : list.PercentOff(25),
            Campaign.BUY_TWO_GET_ONE => BuyTwoGetOne(basket, list)
        };
#pragma warning restore CS8524
        return new Receipt(campaign.ToString(), list, paid);
    }

    // Java's switch expression can hold a block that ends in `yield`; C#'s cannot.
    private static Money BuyTwoGetOne(Basket basket, Money list)
    {
        var discount = Money.Zero;
        foreach (var line in basket.InCategory("book"))
        {
            discount = discount.Plus(line.UnitPrice.Times(line.Quantity / 3));
        }
        return list.Minus(discount);
    }
}
