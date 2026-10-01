namespace dev.kaldiroglu.Command.Account.Solution;

/// <summary>
/// The <b>Invoker</b>: performs transactions, keeps them, and takes them back.
/// <para>
/// Compare <c>Problem.Teller</c>. That teller had a method per operation and a switch per
/// direction, and it remembered names and amounts that it had to interpret again whenever
/// it looked back. This one has no operation of its own. It is handed a transaction, runs
/// it, and keeps the object — so undo is "take the last one off the pile and ask it", and
/// redo is "ask it again".
/// </para>
/// <para>
/// Nothing in this class names an operation. Deposits, withdrawals, transfers, close-outs,
/// and whatever the bank invents next are all one type to it.
/// </para>
/// </summary>
public sealed class Teller
{
    private readonly Stack<ITransaction> _done = new();
    private readonly Stack<ITransaction> _undone = new();
    private readonly List<string> _journal = [];

    public void Perform(ITransaction transaction)
    {
        transaction.Execute();
        _done.Push(transaction);
        _undone.Clear();
        _journal.Add(transaction.Description);
    }

    public void Undo()
    {
        if (_done.Count == 0)
        {
            return;
        }

        var last = _done.Pop();
        last.Undo();
        _undone.Push(last);
        _journal.Add("undo " + last.Description);
    }

    public void Redo()
    {
        if (_undone.Count == 0)
        {
            return;
        }

        var next = _undone.Pop();
        next.Execute();
        _done.Push(next);
        _journal.Add("redo " + next.Description);
    }

    public bool CanUndo => _done.Count > 0;

    public bool CanRedo => _undone.Count > 0;

    public IReadOnlyList<string> Journal() => _journal.ToList().AsReadOnly();
}
