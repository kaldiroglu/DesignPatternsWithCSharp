using System.Globalization;

namespace dev.kaldiroglu.Command.Account.Domain;

/// <summary>
/// An amount of lira, always to two decimal places.
/// <para>
/// A value, not an entity: two <c>Money</c> objects holding the same amount are equal, and
/// nothing ever changes one in place. Every operation answers a new value.
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

    public bool IsLessThan(Money other) => CompareTo(other) < 0;

    public int CompareTo(Money? other) => other is null ? 1 : Amount.CompareTo(other.Amount);

    /// <summary>
    /// Always two decimal places, in the invariant culture: a Turkish locale would otherwise
    /// print 250,00 where the journal expects 250.00.
    /// </summary>
    public override string ToString() => Amount.ToString("F2", CultureInfo.InvariantCulture);
}
