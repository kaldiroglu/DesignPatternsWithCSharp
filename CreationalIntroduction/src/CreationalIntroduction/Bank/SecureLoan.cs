namespace dev.kaldiroglu.CreationalIntroduction.Bank;

public class SecureLoan
{
    private readonly double principal;
    private readonly double rate;
    private readonly int months;
    private readonly double totalRepayment; // Calculated internally

    private SecureLoan(double principal, double rate, int months)
    {
        // Enforce basic rules via exceptions
        if (principal <= 0) throw new ArgumentException("Principal must be positive");
        if (rate < 0) throw new ArgumentException("Rate cannot be negative");
        if (months < 1) throw new ArgumentException("Term must be at least 1 month");

        this.principal = principal;
        this.rate = rate;
        this.months = months;

        // Enforce the Relationship Invariant automatically
        totalRepayment = principal + (principal * rate * months);
    }

    // Static Factory Method: The only way to create the object
    public static SecureLoan Create(double principal, double rate, int months) =>
        new SecureLoan(principal, rate, months);

    // A getter only - no setters allowed to protect the invariants!
    public double TotalRepayment => totalRepayment;
}
