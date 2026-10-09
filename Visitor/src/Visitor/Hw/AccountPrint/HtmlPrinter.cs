using System.Globalization;

namespace dev.kaldiroglu.Visitor.Hw.AccountPrint;

/// <summary>A <b>ConcreteVisitor</b> that writes one HTML table row per account.</summary>
public sealed class HtmlPrinter : IAccountVisitor
{
    private readonly TextWriter output;

    public HtmlPrinter(TextWriter output)
    {
        this.output = output;
    }

    public void Visit(CheckingAccount account)
    {
        Row("Checking", account.Owner, account.Balance.ToString(CultureInfo.InvariantCulture));
    }

    public void Visit(SavingsAccount account)
    {
        Row("Savings", account.Owner, account.Balance.ToString(CultureInfo.InvariantCulture));
    }

    public void Visit(LoanAccount account)
    {
        Row("Loan", account.Owner, (-account.Debt).ToString(CultureInfo.InvariantCulture));
    }

    private void Row(string kind, string owner, string amount)
    {
        output.WriteLine("<tr><td>" + kind + "</td><td>" + owner + "</td><td>" + amount + "</td></tr>");
    }
}
