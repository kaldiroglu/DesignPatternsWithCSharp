namespace dev.kaldiroglu.Strategy.Hw.LateFee;

/// <summary>The daily rate, but never more than the item is worth replacing.</summary>
public sealed class CappedFee : IFeeRule
{
    private readonly int _cap;

    public CappedFee(int cap)
    {
        _cap = cap;
    }

    public string Name => "CAPPED";

    public int Charge(Loan loan) => Math.Min(_cap, loan.DaysLate * loan.DailyRate);
}
