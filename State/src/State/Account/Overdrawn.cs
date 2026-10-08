using System.Globalization;

namespace dev.kaldiroglu.State.Account;

/// <summary>
/// A <b>ConcreteState</b>: the balance is below zero. Withdrawals are allowed down to the
/// overdraft limit, and reaching the limit moves the account to <see cref="Frozen"/>.
/// Transfers and closing are refused.
/// </summary>
public class Overdrawn : IAccountStatus
{
    private readonly Account account;

    public Overdrawn(Account account)
    {
        this.account = account;
        Console.WriteLine("Status: Overdrawn and balance: " + Text(account.Balance));
    }

    public void Withdraw(int amount)
    {
        int balance = account.Balance;
        int overdraftLimit = account.OverdraftLimit;
        if ((balance + overdraftLimit) >= amount)
        {
            balance -= amount;
            account.Balance = balance;
            Console.WriteLine("Status: Overdrawn and balance: " + Text(account.Balance));
            if (balance == -overdraftLimit)
            {
                account.Status = new Frozen(account);
            }
        }
        else
        {
            Console.WriteLine("Status: Overdrawn and balance: " + Text(account.Balance));
            Console.WriteLine("You can not withdraw money!");
        }
    }

    public void Deposit(int amount)
    {
        int balance = account.Balance;
        balance += amount;
        account.Balance = balance;
        if (balance >= 0)
            account.Status = new Active(account);
        else
            Console.WriteLine("Status: Overdrawn and balance: " + account.Balance.ToString(CultureInfo.InvariantCulture));
    }

    public void Transfer(int amount)
    {
        Console.WriteLine("In overdrawn state no transfer is allowed!");
    }

    public void Close()
    {
        Console.WriteLine("In overdrawn state the  account can't be closed!");
    }

    // A negative balance prints with a plain "-" on every machine, as in Java.
    private static string Text(int value) => value.ToString(CultureInfo.InvariantCulture);
}
