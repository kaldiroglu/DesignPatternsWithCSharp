using dev.kaldiroglu.Strategy.Pricing.Domain;

namespace dev.kaldiroglu.Strategy.Pricing.Solution;

/// <summary>
/// A <b>ConcreteStrategy</b> with a threshold in it: more off once the basket is big enough.
/// <para>
/// The 1000-lira figure appeared three times across the naive designs and in three different
/// branches. Here it exists once, in the object that means it, and a test can read it back.
/// </para>
/// </summary>
public sealed class TieredPercentageOff : IPricingRule
{
    private readonly int _belowPercent;
    private readonly int _atOrAbovePercent;

    public TieredPercentageOff(string name, Money threshold, int belowPercent, int atOrAbovePercent)
    {
        Name = name;
        Threshold = threshold;
        _belowPercent = belowPercent;
        _atOrAbovePercent = atOrAbovePercent;
    }

    public static IPricingRule BlackFriday() =>
        new TieredPercentageOff("BLACK_FRIDAY", Money.Of("1000.00"), 25, 40);

    public Money Threshold { get; }

    public string Name { get; }

    public Money PriceFor(Basket basket)
    {
        var list = basket.ListTotal;
        return list.IsAtLeast(Threshold)
            ? list.PercentOff(_atOrAbovePercent)
            : list.PercentOff(_belowPercent);
    }
}
