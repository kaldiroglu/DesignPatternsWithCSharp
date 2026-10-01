namespace dev.kaldiroglu.Command.Account.Problem;

using dev.kaldiroglu.Command.Account.Domain;

/// <summary>
/// Stage three: <b>the history moves out of the account.</b>
/// <para>
/// The best of the three, and where a careful team lands. The account is back to doing
/// banking and nothing else — <c>Domain.Account</c> has no idea it can be undone. The
/// teller performs each operation, writes down what it did, and branches on what it wrote
/// down to reverse it. Undo, redo and the journal all work, across any number of accounts.
/// </para>
/// <para>
/// Then the bank adds transfers. A transfer is a withdrawal and a deposit, both already
/// written and both already undoable, so <see cref="Transfer"/> reuses them — which is
/// exactly what a good developer should do. It records two entries. The teller presses Undo
/// once, and Undo takes back the deposit and leaves the withdrawal: the money has left one
/// account and arrived nowhere.
/// </para>
/// <para>
/// The fix inside this design is a third <see cref="Kind"/>, an entry field that only
/// transfers use, and a branch in both switches. The request was never a thing the teller
/// could hold; it was a name and an amount that the teller has to interpret again every
/// time it looks back.
/// </para>
/// </summary>
public sealed class Teller
{
    private sealed record Entry(Kind Kind, Account Account, Money Amount);

    private readonly Stack<Entry> _done = new();
    private readonly Stack<Entry> _undone = new();
    private readonly List<string> _journal = [];

    public void Deposit(Account account, Money amount)
    {
        account.Deposit(amount);
        Record(new Entry(Kind.Deposit, account, amount));
    }

    public void Withdraw(Account account, Money amount)
    {
        account.Withdraw(amount);
        Record(new Entry(Kind.Withdraw, account, amount));
    }

    /// <summary>Added later, by reusing the two operations that already work.</summary>
    public void Transfer(Account from, Account to, Money amount)
    {
        Withdraw(from, amount);
        Deposit(to, amount);
    }

    public void Undo()
    {
        if (_done.Count == 0)
        {
            return;
        }

        var last = _done.Pop();
        switch (last.Kind)
        {
            case Kind.Deposit:
                last.Account.Withdraw(last.Amount);
                break;
            case Kind.Withdraw:
                last.Account.Deposit(last.Amount);
                break;
        }

        _undone.Push(last);
        _journal.Add("undo " + Describe(last));
    }

    public void Redo()
    {
        if (_undone.Count == 0)
        {
            return;
        }

        var next = _undone.Pop();
        switch (next.Kind)
        {
            case Kind.Deposit:
                next.Account.Deposit(next.Amount);
                break;
            case Kind.Withdraw:
                next.Account.Withdraw(next.Amount);
                break;
        }

        _done.Push(next);
        _journal.Add("redo " + Describe(next));
    }

    public IReadOnlyList<string> Journal() => _journal.ToList().AsReadOnly();

    private void Record(Entry entry)
    {
        _done.Push(entry);
        _undone.Clear();
        _journal.Add(Describe(entry));
    }

    private static string Describe(Entry entry) =>
        entry.Kind.ToString().ToLowerInvariant() + " " + entry.Amount + " " + entry.Account.Owner;
}
