namespace dev.kaldiroglu.State.Account;

/// <summary>
/// Deposits, transfers and withdraws until the account is overdrawn, then deposits and
/// closes it. The Java original's <c>main</c>; its commented-out lines are not ported.
/// </summary>
public static class Test
{
    public static void Run()
    {
        Account account = new Account(1000, false);
        account.Deposit(1000);
        account.Transfer(200);
        account.Withdraw(2500);
        account.Withdraw(200);
        account.Withdraw(100);
        account.Transfer(500);
        account.Deposit(2000);
        account.CloseAccount();
    }
}
