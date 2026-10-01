namespace dev.kaldiroglu.Command.Account.Solution;

using dev.kaldiroglu.Command.Account.Domain;

/// <summary>
/// A <b>ConcreteCommand</b> whose amount is not known until it runs: pay out the whole
/// balance.
/// <para>
/// The request carries no amount — "pay out whatever is there" — so undo cannot be computed
/// from the request. The command has to remember what it actually took. This is GoF
/// implementation issue 2 (supporting undo and redo): a command may need to store "any
/// original values in the receiver that can change as a result of handling the request".
/// </para>
/// <para>
/// In <c>Problem.Teller</c> this would be a third kind, an entry field that only one kind
/// fills in, and a branch in two switches. Here it is one field, in the only class that
/// needs it.
/// </para>
/// </summary>
public sealed class CloseOut : ITransaction
{
    private readonly Account _account;
    private Money _taken = Money.Zero;       // learned when it runs, needed to undo

    public CloseOut(Account account)
    {
        _account = account;
    }

    public void Execute()
    {
        _taken = _account.Balance;
        _account.Withdraw(_taken);
    }

    public void Undo() => _account.Deposit(_taken);

    public string Description => "close out " + _account.Owner + ", paid " + _taken;
}
