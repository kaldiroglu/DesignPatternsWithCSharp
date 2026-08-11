namespace dev.kaldiroglu.Decorator.Hw.FeeEngine;

using System.Globalization;

/// <summary>A percentage the platform takes.</summary>
public sealed class PlatformFee : Adjustment
{
    private readonly decimal _rate;

    public PlatformFee(ICharge component, string percent) : base(component) =>
        _rate = decimal.Parse(percent, CultureInfo.InvariantCulture) / 100m;

    protected override decimal Adjust(decimal @base) => @base + @base * _rate;
}
