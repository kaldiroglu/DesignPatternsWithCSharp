namespace dev.kaldiroglu.Command.Account.Problem;

using dev.kaldiroglu.Command.Account.Domain;

/// <summary>
/// Stage two: <b>the account keeps a history.</b>
/// <para>
/// A real improvement on stage one. The account keeps every operation it has performed, so
/// a teller can undo as far back as they like, redo what they undid, and hand the auditors
/// a journal of the day. Nothing is lost, and the names are an enum, so a typo no longer
/// compiles.
/// </para>
/// <para>
/// What it costs is the account. It now has eight public operations and three of them are
/// banking; the other five are a family of helper methods around them — undo, redo, the
/// journal, and the questions a screen asks before it enables a button. And every operation
/// lives in three places: the method that performs it, a branch in <see cref="Undo"/> and a
/// branch in <see cref="Redo"/>. A third operation is an edit to all three.
/// </para>
/// </summary>
public sealed class HistoryAccount
{
    private sealed record Entry(Kind Kind, Money Amount);

    private readonly string _owner;
    private readonly Stack<Entry> _done = new();
    private readonly Stack<Entry> _undone = new();
    private readonly List<string> _journal = [];

    public HistoryAccount(string owner, Money opening)
    {
        _owner = owner;
        Balance = opening;
    }

    public Money Balance { get; private set; }

    public void Deposit(Money amount)
    {
        Balance = Balance.Plus(amount);
        Record(new Entry(Kind.Deposit, amount));
    }

    public void Withdraw(Money amount)
    {
        if (Balance.IsLessThan(amount))
        {
            throw new InsufficientFundsException(_owner, Balance, amount);
        }

        Balance = Balance.Minus(amount);
        Record(new Entry(Kind.Withdraw, amount));
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
                Balance = Balance.Minus(last.Amount);
                break;
            case Kind.Withdraw:
                Balance = Balance.Plus(last.Amount);
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
                Balance = Balance.Plus(next.Amount);
                break;
            case Kind.Withdraw:
                Balance = Balance.Minus(next.Amount);
                break;
        }

        _done.Push(next);
        _journal.Add("redo " + Describe(next));
    }

    public bool CanUndo => _done.Count > 0;

    public bool CanRedo => _undone.Count > 0;

    public IReadOnlyList<string> Journal() => _journal.ToList().AsReadOnly();

    private void Record(Entry entry)
    {
        _done.Push(entry);
        _undone.Clear();
        _journal.Add(Describe(entry));
    }

    private static string Describe(Entry entry) =>
        entry.Kind.ToString().ToLowerInvariant() + " " + entry.Amount;
}
