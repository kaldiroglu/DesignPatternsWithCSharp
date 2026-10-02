namespace dev.kaldiroglu.Strategy.Hw.LateFee;

/// <summary>
/// Nothing for the first few days, then twice the rate.
/// <para>
/// The rule that is not a multiplier, which is why <see cref="IFeeRule"/> is a method rather
/// than a number. A design that had made the fee rule an <c>int RatePerDay</c> could not
/// express it.
/// </para>
/// </summary>
public sealed class GraceThenDouble : IFeeRule
{
    private readonly int _graceDays;

    public GraceThenDouble(int graceDays)
    {
        _graceDays = graceDays;
    }

    public string Name => "GRACE_THEN_DOUBLE";

    public int Charge(Loan loan)
    {
        var chargeable = loan.DaysLate - _graceDays;
        return chargeable <= 0 ? 0 : chargeable * loan.DailyRate * 2;
    }
}
