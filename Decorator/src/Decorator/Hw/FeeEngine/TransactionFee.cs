namespace dev.kaldiroglu.Decorator.Hw.FeeEngine;

using System.Globalization;

/// <summary>A fixed fee per transaction.</summary>
public sealed class TransactionFee : Adjustment
{
    private readonly decimal _fee;

    public TransactionFee(ICharge component, string fee) : base(component) =>
        _fee = decimal.Parse(fee, CultureInfo.InvariantCulture);

    protected override decimal Adjust(decimal @base) => @base + _fee;
}
