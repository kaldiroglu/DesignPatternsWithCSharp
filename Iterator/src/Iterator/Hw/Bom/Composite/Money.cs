using System.Globalization;

namespace dev.kaldiroglu.Iterator.Hw.Bom.Composite;

/// <summary>
/// An amount of money, held in <see cref="decimal"/> so that costs never drift the way
/// binary floating-point values do.
/// </summary>
/// <remarks>
/// <para>
/// Copied from the Composite port (<c>dev.kaldiroglu.Composite.Bom.Domain.Money</c>). The
/// pattern folders in this repository do not reference each other, so the type is copied
/// rather than shared.
/// </para>
/// </remarks>
/// <param name="Amount">The amount, rounded to two decimal places.</param>
public readonly record struct Money(decimal Amount)
{
    /// <summary>No money at all.</summary>
    public static readonly Money Zero = Of(0m);

    /// <summary>Creates an amount, e.g. <c>Money.Of(24.50m)</c>.</summary>
    public static Money Of(decimal value) =>
        new(Math.Round(value, 2, MidpointRounding.AwayFromZero));

    public override string ToString() =>
        "$" + Amount.ToString("0.00", CultureInfo.InvariantCulture);
}
