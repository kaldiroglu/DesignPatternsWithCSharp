using System.Globalization;

namespace dev.kaldiroglu.State.Account;

/// <summary>A <b>ConcreteState</b>: the account is closed. Every operation is refused.</summary>
public class Closed : IAccountStatus
{
    private readonly Account account;

    public Closed(Account account)
    {
        this.account = account;
        Console.WriteLine("Status: Closed and balance: "
                          + account.Balance.ToString(CultureInfo.InvariantCulture));
    }

    public void Withdraw(int amount)
    {
        Console.WriteLine("Account closed!");
    }

    public void Deposit(int amount)
    {
        Console.WriteLine("Account closed!");
    }

    public void Transfer(int amount)
    {
        Console.WriteLine("Account closed!");
    }

    public void Close()
    {
        // TODO Auto-generated method stub
    }
}
