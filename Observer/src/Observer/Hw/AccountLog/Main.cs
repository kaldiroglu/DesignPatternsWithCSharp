namespace dev.kaldiroglu.Observer.Hw.AccountLog;

/// <summary>
/// A transaction log listens to an account. Two changes are recorded; a withdrawal that is
/// too large throws before any listener is told, so it is not recorded.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Observer.Demo -- hw-accountlog</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var account = new Account("Ayse", 100);
        var log = new TransactionLog();
        account.AddListener(log);
        account.Deposit(50);
        account.Withdraw(30);
        try
        {
            account.Withdraw(500);
        }
        catch (ArgumentException refused)
        {
            Console.WriteLine("Refused: " + refused.Message);
        }
        foreach (Transaction transaction in log.Transactions)
        {
            Console.WriteLine(transaction);
        }
    }
}
