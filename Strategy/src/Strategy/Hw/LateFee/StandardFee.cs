namespace dev.kaldiroglu.Strategy.Hw.LateFee;

/// <summary>The daily rate, every day, with no ceiling.</summary>
public sealed class StandardFee : IFeeRule
{
    public string Name => "STANDARD";

    public int Charge(Loan loan) => loan.DaysLate * loan.DailyRate;
}
