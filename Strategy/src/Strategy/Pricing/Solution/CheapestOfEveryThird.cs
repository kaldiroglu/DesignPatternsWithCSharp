using dev.kaldiroglu.Strategy.Pricing.Domain;

namespace dev.kaldiroglu.Strategy.Pricing.Solution;

/// <summary>
/// A <b>ConcreteStrategy</b> that is not a percentage at all: buy two, get the third free.
/// <para>
/// This is the rule that proves the interface is about an <em>algorithm</em> and not about a
/// discount rate. It walks the basket, it groups by category, and it takes off the cheapest
/// of every third item — arithmetic with nothing in common with the rules beside it. Had the
/// interface been <c>int PercentOff()</c>, this campaign could not have been written.
/// </para>
/// </summary>
public sealed class CheapestOfEveryThird : IPricingRule
{
    private readonly string _category;

    public CheapestOfEveryThird(string category)
    {
        _category = category;
    }

    public string Name => "BUY_TWO_GET_ONE";

    public Money PriceFor(Basket basket)
    {
        var off = Money.Zero;
        foreach (var line in basket.InCategory(_category))
        {
            off = off.Plus(line.UnitPrice.Times(line.Quantity / 3));
        }
        return basket.ListTotal.Minus(off);
    }
}
