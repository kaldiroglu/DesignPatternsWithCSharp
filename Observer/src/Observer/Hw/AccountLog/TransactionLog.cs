namespace dev.kaldiroglu.Observer.Hw.AccountLog;

/// <summary>
/// A <b>ConcreteObserver</b>: creates a <see cref="Transaction"/> for every change it hears about.
/// <para>
/// The homework asked that the transaction objects be created by the listener, not by the
/// account. A failed withdrawal throws before any notification, so it is never recorded.
/// </para>
/// </summary>
public sealed class TransactionLog : ITransactionListener
{
    private readonly List<Transaction> transactions = [];

    public void BalanceChanged(Account account, int amount, int newBalance)
    {
        string kind = amount >= 0 ? "deposit" : "withdrawal";
        transactions.Add(new Transaction(account.Owner, kind, Math.Abs(amount), newBalance));
    }

    public IReadOnlyList<Transaction> Transactions => transactions.ToList().AsReadOnly();
}
