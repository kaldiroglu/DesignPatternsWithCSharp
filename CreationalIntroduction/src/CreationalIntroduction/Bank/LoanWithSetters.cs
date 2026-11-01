namespace dev.kaldiroglu.CreationalIntroduction.Bank;

public class LoanWithSetters
{
    public double Principal { get; set; }
    public double Rate { get; set; }
    public int Months { get; set; }
    public double TotalRepayment { get; set; }

    /// <summary>What the total must be: principal + principal * rate * months.</summary>
    public double CorrectTotal() => Principal + (Principal * Rate * Months);
}
