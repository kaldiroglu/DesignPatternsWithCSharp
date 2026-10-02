using dev.kaldiroglu.Strategy.Pricing.Domain;

namespace dev.kaldiroglu.Strategy.Pricing.Problem;

/// <summary>Twenty-five percent off, or forty over a thousand lira.</summary>
public sealed class BlackFridayCheckout : Checkout
{
    protected override string CampaignName => "BLACK_FRIDAY";

    protected override Money PriceFor(Basket basket)
    {
        var list = basket.ListTotal;
        return list.IsAtLeast(Money.Of("1000.00")) ? list.PercentOff(40) : list.PercentOff(25);
    }
}
