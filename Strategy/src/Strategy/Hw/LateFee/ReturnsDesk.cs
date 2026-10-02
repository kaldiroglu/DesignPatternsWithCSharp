namespace dev.kaldiroglu.Strategy.Hw.LateFee;

/// <summary>The Context: charges a returned loan under whichever rule the member's class carries.</summary>
public sealed class ReturnsDesk
{
    private IFeeRule _rule;

    public ReturnsDesk(IFeeRule rule)
    {
        ArgumentNullException.ThrowIfNull(rule);
        _rule = rule;
    }

    public void SetRule(IFeeRule rule)
    {
        ArgumentNullException.ThrowIfNull(rule);
        _rule = rule;
    }

    public string RuleName => _rule.Name;

    public int Charge(Loan loan) => _rule.Charge(loan);
}
