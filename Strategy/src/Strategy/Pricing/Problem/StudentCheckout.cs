using dev.kaldiroglu.Strategy.Pricing.Domain;

namespace dev.kaldiroglu.Strategy.Pricing.Problem;

/// <summary>Twenty percent off, for anybody who showed a student card.</summary>
public sealed class StudentCheckout : Checkout
{
    protected override string CampaignName => "STUDENT";

    protected override Money PriceFor(Basket basket) =>
        basket.Customer.IsStudent
            ? basket.ListTotal.PercentOff(20)
            : basket.ListTotal;
}
