namespace dev.kaldiroglu.CreationalIntroduction.Bank;

/// <summary>
/// No setters, and the total is calculated, so no stored value can disagree with the others.
/// </summary>
/// <remarks>
/// A positional record has no constructor body to check its arguments in, so this record
/// declares its constructor. The properties have no <c>init</c> accessor, so a <c>with</c>
/// expression cannot skip the checks.
/// </remarks>
public sealed record LoanRecord
{
    public double Principal { get; }
    public double Rate { get; }
    public int Months { get; }

    public LoanRecord(double principal, double rate, int months)
    {
        if (principal <= 0) throw new ArgumentException("Principal must be positive");
        if (rate < 0) throw new ArgumentException("Rate cannot be negative");
        if (months < 1) throw new ArgumentException("Term must be at least 1 month");

        Principal = principal;
        Rate = rate;
        Months = months;
    }

    public double TotalRepayment => Principal + (Principal * Rate * Months);

    /// <summary>The form the slide shows, not the default record text.</summary>
    public override string ToString() =>
        $"LoanRecord[principal={DoubleText.Of(Principal)}, rate={DoubleText.Of(Rate)}, months={Months}]";
}
