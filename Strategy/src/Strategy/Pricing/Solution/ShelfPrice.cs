using dev.kaldiroglu.Strategy.Pricing.Domain;

namespace dev.kaldiroglu.Strategy.Pricing.Solution;

/// <summary>
/// A <b>ConcreteStrategy</b>: charge what is on the shelf edge.
/// <para>
/// The one every receipt is measured against, and the reason "no campaign" needs no special
/// case anywhere in <see cref="Checkout"/>. GoF call this out as an implementation issue
/// (p. 319): a null strategy forces every client to check for null, so the absence of a
/// campaign is itself a campaign.
/// </para>
/// </summary>
public sealed class ShelfPrice : IPricingRule
{
    public string Name => "NONE";

    public Money PriceFor(Basket basket) => basket.ListTotal;
}
