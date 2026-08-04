using System.Globalization;

namespace dev.kaldiroglu.Composite.Gof.Equipment;

/// <summary>
/// The <c>Currency</c> value type used by the GoF equipment example (p. 170).
/// </summary>
/// <remarks>
/// A tiny immutable wrapper over <see cref="decimal"/> so that money is never
/// held in a binary floating-point type. It exists only to keep the pattern code
/// readable — it is not part of the pattern.
/// </remarks>
/// <param name="Amount">The monetary amount, rounded to two decimal places.</param>
public readonly record struct Currency(decimal Amount) : IComparable<Currency>
{
    /// <summary>Zero money — the identity used when summing a composite's children.</summary>
    public static readonly Currency Zero = Of(0m);

    /// <summary>Creates an amount, e.g. <c>Currency.Of(24.50m)</c>.</summary>
    public static Currency Of(decimal value) =>
        new(Math.Round(value, 2, MidpointRounding.AwayFromZero));

    /// <summary>Returns the sum of this amount and <paramref name="other"/>.</summary>
    public Currency Plus(Currency other) => Of(Amount + other.Amount);

    /// <summary>Returns this amount multiplied by <paramref name="factor"/>.</summary>
    public Currency Times(decimal factor) => Of(Amount * factor);

    public int CompareTo(Currency other) => Amount.CompareTo(other.Amount);

    // Invariant culture, so the C# and Java demos print byte-identical output
    // regardless of the machine's regional settings.
    public override string ToString() =>
        "$" + Amount.ToString("0.00", CultureInfo.InvariantCulture);
}
