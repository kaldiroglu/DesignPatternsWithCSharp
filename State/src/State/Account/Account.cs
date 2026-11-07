namespace dev.kaldiroglu.State.Account;

/// <summary>
/// The <b>Context</b>: a bank account that forwards every request to its status. An account
/// may go below zero down to its overdraft limit. Being frozen is a status, so the account
/// keeps no flag for it.
/// </summary>
public class Account
{
    public Account(int balance)
    {
        Balance = balance;
        if (balance >= 0)
            Status = new Active(this);
        else
            throw new Exception("Initial balance can't be negative!");
    }

    public void Deposit(int amount)
    {
        RequirePositive(amount);
        Status.Deposit(amount);
    }

    public void Withdraw(int amount)
    {
        RequirePositive(amount);
        Status.Withdraw(amount);
    }

    public void Transfer(int amount)
    {
        RequirePositive(amount);
        Status.Transfer(amount);
    }

    public IAccountStatus Status { get; set; }

    public int Balance { get; set; }

    public int OverdraftLimit { get; } = 1000;

    public void CloseAccount()
    {
        Status.Close();
    }

    /// <summary>A negative amount would turn a deposit into a withdrawal, so no status accepts one.</summary>
    private static void RequirePositive(int amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), amount, "Amount must be positive.");
    }
}
