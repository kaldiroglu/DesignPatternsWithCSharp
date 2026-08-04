using System.Globalization;

namespace dev.kaldiroglu.Composite.Bom.Domain;

/// <summary>
/// An amount of money, held in <see cref="decimal"/> so that costs never drift
/// the way binary floating-point values do.
/// </summary>
/// <remarks>A supporting value type, not part of the Composite pattern.</remarks>
/// <param name="Amount">The amount, rounded to two decimal places.</param>
public readonly record struct Money(decimal Amount) : IComparable<Money>
{
    /// <summary>The identity for summation — the cost of an assembly with no children.</summary>
    public static readonly Money Zero = Of(0m);

    /// <summary>Creates an amount, e.g. <c>Money.Of(24.50m)</c>.</summary>
    public static Money Of(decimal value) =>
        new(Math.Round(value, 2, MidpointRounding.AwayFromZero));

    /// <summary>Returns the sum of this amount and <paramref name="other"/>.</summary>
    public Money Plus(Money other) => Of(Amount + other.Amount);

    /// <summary>Returns this amount repeated <paramref name="quantity"/> times.</summary>
    public Money Times(int quantity) => Of(Amount * quantity);

    public int CompareTo(Money other) => Amount.CompareTo(other.Amount);

    // Invariant culture, so the C# and Java demos print identical output
    // regardless of the machine's regional settings.
    public override string ToString() =>
        "$" + Amount.ToString("0.00", CultureInfo.InvariantCulture);
}
