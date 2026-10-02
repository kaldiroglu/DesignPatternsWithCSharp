using dev.kaldiroglu.Strategy.Pricing.Domain;

namespace dev.kaldiroglu.Strategy.Pricing.Problem;

/// <summary>
/// Stage three: one class per campaign, joined by inheritance.
/// <para>
/// This is the best of the three and the improvement is real. Each rule is its own class,
/// each can be read on its own page and tested on its own, and a new campaign is a new file
/// rather than an edit to a method that already works for five others. Nothing here is
/// stupid; this is where a careful team lands.
/// </para>
/// <para>
/// What it decides quietly is that <b>a till is its campaign</b>. The rule is the object's
/// class, and an object cannot change its class.
/// </para>
/// </summary>
public abstract class Checkout
{
    /// <summary>What to print above the total.</summary>
    protected abstract string CampaignName { get; }

    /// <summary>What this campaign charges for the basket.</summary>
    protected abstract Money PriceFor(Basket basket);

    public Receipt Ring(Basket basket) =>
        new(CampaignName, basket.ListTotal, PriceFor(basket));
}
