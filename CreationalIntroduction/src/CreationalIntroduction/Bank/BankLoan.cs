namespace dev.kaldiroglu.CreationalIntroduction.Bank;

public class BankLoan
{
    private double principal;
    private double rate;
    private int months;
    private double totalRepayment;

    // If we use a standard constructor, the user has to calculate 'totalRepayment' manually.
    // This is "Easy to create" (just a constructor) but "Hard to use" (user must know math).
    public BankLoan(double principal, double rate, int months, double totalRepayment)
    {
        // A tolerance, because two double values that should be equal often differ slightly.
        if (Math.Abs(totalRepayment - (principal + (principal * rate * months))) > 0.005)
        {
            throw new ArgumentException("Math doesn't add up!");
        }
        this.principal = principal;
        this.rate = rate;
        this.months = months;
        this.totalRepayment = totalRepayment;
    }

    public double TotalRepayment => totalRepayment;
}
