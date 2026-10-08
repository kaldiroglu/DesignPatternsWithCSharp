using System.Globalization;

namespace dev.kaldiroglu.State.Account;

/// <summary>
/// A <b>ConcreteState</b>: the account is in credit. Every operation is allowed. A withdrawal
/// that takes the balance below zero moves the account to <see cref="Overdrawn"/>, or to
/// <see cref="Frozen"/> when it reaches the overdraft limit exactly.
/// </summary>
public class Active : IAccountStatus
{
    private readonly Account account;

    public Active(Account account)
    {
        this.account = account;
        Console.WriteLine("Status: Active and balance: " + Text(account.Balance));
    }

    public void Withdraw(int amount)
    {
        int balance = account.Balance;
        int overdraftLimit = account.OverdraftLimit;
        if ((balance + overdraftLimit) >= amount)
        {
            balance -= amount;
            account.Balance = balance;
            if (balance >= 0)
            {
                Console.WriteLine("Status: Active and balance: " + Text(account.Balance));
            }
            if (balance == -overdraftLimit)
            {
                account.Status = new Frozen(account);
            }
            else if (balance < 0)
            {
                account.Status = new Overdrawn(account);
            }
        }
    }

    public void Deposit(int amount)
    {
        int balance = account.Balance;
        balance += amount;
        account.Balance = balance;
        Console.WriteLine("Status: Active and balance: " + Text(account.Balance));
    }

    public void Transfer(int amount)
    {
        Withdraw(amount);
    }

    public void Close()
    {
        Withdraw(account.Balance);
        account.Status = new Closed(account);
        Console.WriteLine("Account is closed!");
    }

    // A negative balance prints with a plain "-" on every machine, as in Java.
    private static string Text(int value) => value.ToString(CultureInfo.InvariantCulture);
}
