namespace dev.kaldiroglu.Observer.Hw.AccountLog;

/// <summary>The <b>Observer</b>: told about every change to an account's balance.</summary>
public interface ITransactionListener
{
    void BalanceChanged(Account account, int amount, int newBalance);
}
