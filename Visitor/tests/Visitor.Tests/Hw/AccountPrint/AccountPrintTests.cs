using Xunit;
using P = global::dev.kaldiroglu.Visitor.Hw.AccountPrint;

namespace dev.kaldiroglu.Visitor.Tests.Hw.AccountPrint;

/// <summary>Homework one: each printer is a visitor, and it is given the output it writes to.</summary>
public class AccountPrintTests
{
    private static readonly P.IAccount[] Accounts =
    [
        new P.CheckingAccount("Ayse", 1200, 500),
        new P.SavingsAccount("Deniz", 5000, 3),
        new P.LoanAccount("Mert", 20000, 900)
    ];

    private static List<string> PrintedBy(Func<TextWriter, P.IAccountVisitor> printer)
    {
        var written = new StringWriter();
        var visitor = printer(written);
        foreach (var account in Accounts)
        {
            account.Accept(visitor);
        }
        return written.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    [Fact]
    public void TextToAGivenOutput()
    {
        Assert.Equal(new[]
        {
            "Checking Ayse: 1200 (overdraft 500)",
            "Savings  Deniz: 5000 (3% interest)",
            "Loan     Mert: owes 20000, pays 900 a month"
        }, PrintedBy(output => new P.TextPrinter(output)));
    }

    [Fact]
    public void HtmlToAGivenOutput()
    {
        Assert.Equal(new[]
        {
            "<tr><td>Checking</td><td>Ayse</td><td>1200</td></tr>",
            "<tr><td>Savings</td><td>Deniz</td><td>5000</td></tr>",
            "<tr><td>Loan</td><td>Mert</td><td>-20000</td></tr>"
        }, PrintedBy(output => new P.HtmlPrinter(output)));
    }

    [Fact]
    public void MainOutput()
    {
        Assert.Equal(new[]
        {
            "Checking Ayse: 1200 (overdraft 500)",
            "Savings  Deniz: 5000 (3% interest)",
            "Loan     Mert: owes 20000, pays 900 a month",
            "<tr><td>Checking</td><td>Ayse</td><td>1200</td></tr>",
            "<tr><td>Savings</td><td>Deniz</td><td>5000</td></tr>",
            "<tr><td>Loan</td><td>Mert</td><td>-20000</td></tr>"
        }, Printed.By(P.Main.Run));
    }
}
