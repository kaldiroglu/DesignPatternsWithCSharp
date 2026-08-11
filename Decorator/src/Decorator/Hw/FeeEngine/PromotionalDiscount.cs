namespace dev.kaldiroglu.Decorator.Hw.FeeEngine;

using System.Globalization;

/// <summary>A percentage off — and two percentages commute, so its position changes nothing.</summary>
public sealed class PromotionalDiscount : Adjustment
{
    private readonly decimal _rate;

    public PromotionalDiscount(ICharge component, string percent) : base(component) =>
        _rate = decimal.Parse(percent, CultureInfo.InvariantCulture) / 100m;

    protected override decimal Adjust(decimal @base) => @base - @base * _rate;
}
