using System.Globalization;

namespace dev.kaldiroglu.Visitor.Hw.AccountPrint;

/// <summary>A <b>ConcreteVisitor</b> that writes one plain line per account to the output it is given.</summary>
public sealed class TextPrinter : IAccountVisitor
{
    private readonly TextWriter output;

    public TextPrinter(TextWriter output)
    {
        this.output = output;
    }

    public void Visit(CheckingAccount account)
    {
        output.WriteLine(string.Create(CultureInfo.InvariantCulture,
                $"Checking {account.Owner}: {account.Balance} (overdraft {account.OverdraftLimit})"));
    }

    public void Visit(SavingsAccount account)
    {
        output.WriteLine(string.Create(CultureInfo.InvariantCulture,
                $"Savings  {account.Owner}: {account.Balance} ({account.InterestPercent}% interest)"));
    }

    public void Visit(LoanAccount account)
    {
        output.WriteLine(string.Create(CultureInfo.InvariantCulture,
                $"Loan     {account.Owner}: owes {account.Debt}, pays {account.MonthlyPayment} a month"));
    }
}
