namespace dev.kaldiroglu.Visitor.Hw.AccountPrint;

/// <summary>
/// Prints three accounts twice, as text and as HTML. Each format is a visitor that is
/// given its output; the account classes do not print.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Visitor.Demo -- hw-accountprint</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        List<IAccount> accounts =
        [
            new CheckingAccount("Ayse", 1200, 500),
            new SavingsAccount("Deniz", 5000, 3),
            new LoanAccount("Mert", 20000, 900)
        ];
        TextWriter console = Console.Out;
        IAccountVisitor text = new TextPrinter(console);
        accounts.ForEach(account => account.Accept(text));
        IAccountVisitor html = new HtmlPrinter(console);
        accounts.ForEach(account => account.Accept(html));
        console.Flush();
    }
}
