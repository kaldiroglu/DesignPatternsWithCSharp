using System.Globalization;

namespace dev.kaldiroglu.Strategy.Freight;

/// <summary>An amount, held to two places so no example ever argues about rounding.</summary>
public sealed record Money : IComparable<Money>
{
    public static readonly Money Zero = Of("0.00");

    public Money(decimal amount)
    {
        Amount = decimal.Round(amount, 2, MidpointRounding.AwayFromZero);
    }

    public decimal Amount { get; }

    public static Money Of(string amount) =>
        new(decimal.Parse(amount, CultureInfo.InvariantCulture));

    public Money Plus(Money other) => new(Amount + other.Amount);

    /// <summary>
    /// Multiplies by a count of units, rounding half away from zero at two places.
    /// <para>
    /// The <c>double</c> is read through its shortest round-trip text, as Java's
    /// <c>BigDecimal.valueOf(double)</c> reads it, so 0.1 units means exactly 0.1.
    /// </para>
    /// </summary>
    public Money Times(double units)
    {
        var exact = decimal.Parse(units.ToString("R", CultureInfo.InvariantCulture),
            NumberStyles.Float, CultureInfo.InvariantCulture);
        return new Money(Amount * exact);
    }

    public Money PercentMore(int percent) =>
        new(decimal.Round(Amount * (100 + percent) / 100m, 2, MidpointRounding.AwayFromZero));

    public int CompareTo(Money? other) => other is null ? 1 : Amount.CompareTo(other.Amount);

    /// <summary>
    /// Always two decimal places, in the invariant culture: a Turkish locale would otherwise
    /// print 89,90 where the quote expects 89.90.
    /// </summary>
    public override string ToString() => Amount.ToString("F2", CultureInfo.InvariantCulture);
}
