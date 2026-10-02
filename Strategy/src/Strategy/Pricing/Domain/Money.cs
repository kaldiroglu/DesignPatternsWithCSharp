using System.Globalization;

namespace dev.kaldiroglu.Strategy.Pricing.Domain;

/// <summary>
/// An amount in minor-unit-safe decimal, so the examples never argue about rounding.
/// <para>
/// Every amount is held to two places and rounded half away from zero (Java's
/// <c>HALF_UP</c>), which is what a till does. The type exists so a pricing rule can be read
/// as arithmetic about money rather than as arithmetic about <c>double</c>.
/// </para>
/// </summary>
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

    public Money Minus(Money other) => new(Amount - other.Amount);

    public Money Times(int count) => new(Amount * count);

    /// <summary>Takes a percentage off, so <c>PercentOff(20)</c> leaves 80% of the amount.</summary>
    public Money PercentOff(int percent)
    {
        var kept = decimal.Round((100 - percent) / 100m, 4, MidpointRounding.AwayFromZero);
        return new Money(Amount * kept);
    }

    public bool IsAtLeast(Money other) => Amount >= other.Amount;

    public int CompareTo(Money? other) => other is null ? 1 : Amount.CompareTo(other.Amount);

    /// <summary>
    /// Always two decimal places, in the invariant culture: a Turkish locale would otherwise
    /// print 1200,00 where the receipt expects 1200.00.
    /// </summary>
    public override string ToString() => Amount.ToString("F2", CultureInfo.InvariantCulture);
}
