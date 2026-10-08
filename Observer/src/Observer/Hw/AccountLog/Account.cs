namespace dev.kaldiroglu.Observer.Hw.AccountLog;

/// <summary>
/// Homework 1: the account from the Command deck, as a <b>Subject</b>.
/// <para>
/// The account does banking and tells its listeners after each change. It does not create
/// transaction records; that is the listener's job. The account stays as small as it was in
/// the Command deck's solution.
/// </para>
/// </summary>
public sealed class Account(string owner, int balance)
{
    private int balance = balance;
    private readonly List<ITransactionListener> listeners = [];

    public void AddListener(ITransactionListener listener)
    {
        listeners.Add(listener);
    }

    public string Owner => owner;

    public void Deposit(int amount)
    {
        balance += amount;
        NotifyListeners(amount);
    }

    public void Withdraw(int amount)
    {
        if (amount > balance)
        {
            throw new ArgumentException(owner + " cannot withdraw " + amount);
        }
        balance -= amount;
        NotifyListeners(-amount);
    }

    private void NotifyListeners(int amount)
    {
        foreach (var listener in listeners.ToList())
        {
            listener.BalanceChanged(this, amount, balance);
        }
    }
}
