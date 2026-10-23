namespace dev.kaldiroglu.Strategy.Pricing.Solution;

/// <summary>Today's five campaigns. This is the one place that names the concrete rules.</summary>
internal static class SampleCampaignBook
{
    internal static CampaignBook Build() =>
        new(new ShelfPrice(),
            PercentageOff.Student(),
            PercentageOff.Staff(),
            TieredPercentageOff.BlackFriday(),
            new CheapestOfEveryThird("book"));
}
