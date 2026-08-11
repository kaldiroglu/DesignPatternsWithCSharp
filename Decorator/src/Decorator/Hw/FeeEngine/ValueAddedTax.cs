namespace dev.kaldiroglu.Decorator.Hw.FeeEngine;

using System.Globalization;

/// <summary>VAT. Where it sits relative to a discount decides the company's tax position.</summary>
public sealed class ValueAddedTax : Adjustment
{
    private readonly decimal _rate;

    public ValueAddedTax(ICharge component, string percent) : base(component) =>
        _rate = decimal.Parse(percent, CultureInfo.InvariantCulture) / 100m;

    protected override decimal Adjust(decimal @base) => @base + @base * _rate;
}
