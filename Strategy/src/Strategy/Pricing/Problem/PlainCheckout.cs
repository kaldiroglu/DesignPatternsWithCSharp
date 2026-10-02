using dev.kaldiroglu.Strategy.Pricing.Domain;

namespace dev.kaldiroglu.Strategy.Pricing.Problem;

/// <summary>Shelf price, no campaign.</summary>
public sealed class PlainCheckout : Checkout
{
    protected override string CampaignName => "NONE";

    protected override Money PriceFor(Basket basket) => basket.ListTotal;
}
