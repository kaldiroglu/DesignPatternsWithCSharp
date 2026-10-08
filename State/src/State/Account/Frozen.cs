using System.Globalization;

namespace dev.kaldiroglu.State.Account;

/// <summary>
/// A <b>ConcreteState</b>: the balance has reached the overdraft limit. Only a deposit is
/// allowed, and a deposit that brings the balance to zero or above makes the account
/// <see cref="Active"/> again.
/// </summary>
public class Frozen : IAccountStatus
{
    private readonly Account account;

    public Frozen(Account account)
    {
        this.account = account;
        Console.WriteLine("Status: Frozen and balance: "
                          + account.Balance.ToString(CultureInfo.InvariantCulture));
    }

    public void Withdraw(int amount)
    {
        Console.WriteLine("In frozen state no withdraw is allowed!");
    }

    public void Deposit(int amount)
    {
        int balance = account.Balance;
        // NOTE: the Java has the same line, "balance =+ amount". It sets the balance to the
        // amount instead of adding the amount to it. It is kept so that the output is the same.
        balance = +amount;
        account.Balance = balance;
        if (account.Balance >= 0)
            account.Status = new Active(account);
    }

    public void Transfer(int amount)
    {
        Console.WriteLine("In frozen state no transfer is allowed!");
    }

    public void Close()
    {
        Console.WriteLine("In frozen state the  account can't be closed!");
    }
}
