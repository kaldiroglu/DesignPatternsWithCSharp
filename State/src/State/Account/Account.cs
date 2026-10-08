namespace dev.kaldiroglu.State.Account;

/// <summary>
/// The <b>Context</b>: a bank account that forwards every request to its status. An account
/// may go below zero down to its overdraft limit.
/// </summary>
public class Account
{
    public Account(int balance, bool frozen)
    {
        Balance = balance;
        IsFrozen = frozen;
        if (balance >= 0)
            Status = new Active(this);
        else
            throw new Exception("Initial balance can't be negative!");
    }

    public void Deposit(int amount)
    {
        Status.Deposit(amount);
    }

    public void Withdraw(int amount)
    {
        Status.Withdraw(amount);
    }

    public void Transfer(int amount)
    {
        Status.Transfer(amount);
    }

    public IAccountStatus Status { get; set; }

    public int Balance { get; set; }

    public bool IsFrozen { get; set; }

    public int OverdraftLimit { get; } = 1000;

    public void CloseAccount()
    {
        Status.Close();
    }
}
