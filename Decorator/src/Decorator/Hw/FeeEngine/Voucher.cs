namespace dev.kaldiroglu.Decorator.Hw.FeeEngine;

using System.Globalization;

/// <summary>
/// A fixed amount off, never below zero.
/// <para>
/// This is the one that makes the exercise have an answer: a voucher is a subtraction, and
/// subtraction does not commute with the VAT multiplication. Put VAT outside the voucher
/// and the discount reduces the taxable base — which is what tax law says. Put it inside
/// and the company pays VAT on the voucher itself.
/// </para>
/// </summary>
public sealed class Voucher : Adjustment
{
    private readonly decimal _value;

    public Voucher(ICharge component, string value) : base(component) =>
        _value = decimal.Parse(value, CultureInfo.InvariantCulture);

    protected override decimal Adjust(decimal @base) => Math.Max(@base - _value, 0m);
}
