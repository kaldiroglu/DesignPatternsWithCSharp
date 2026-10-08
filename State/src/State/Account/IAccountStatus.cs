namespace dev.kaldiroglu.State.Account;

/// <summary>The <b>State</b>: what an account does in one status.</summary>
public interface IAccountStatus
{
    void Withdraw(int amount);

    void Deposit(int amount);

    void Transfer(int amount);

    void Close();
}
