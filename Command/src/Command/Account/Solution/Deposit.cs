namespace dev.kaldiroglu.Command.Account.Solution;

using dev.kaldiroglu.Command.Account.Domain;

/// <summary>
/// A <b>ConcreteCommand</b>: money into one account.
/// <para>
/// It binds a receiver to an action — this account, a deposit of this much — and knows its
/// own opposite. Compare the <c>Deposit</c> branch in <c>Problem.Teller.Undo()</c>: that
/// knowledge used to live in a switch in somebody else's class.
/// </para>
/// </summary>
public sealed class Deposit : ITransaction
{
    private readonly Account _account;
    private readonly Money _amount;

    public Deposit(Account account, Money amount)
    {
        _account = account;
        _amount = amount;
    }

    public void Execute() => _account.Deposit(_amount);

    public void Undo() => _account.Withdraw(_amount);

    public string Description => "deposit " + _amount + " " + _account.Owner;
}
