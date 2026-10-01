namespace dev.kaldiroglu.Command.Account.Solution;

using dev.kaldiroglu.Command.Account.Domain;

/// <summary>
/// A <b>ConcreteCommand</b>: money out of one account.
/// <para>
/// If the account cannot cover it, <c>Execute()</c> throws and nothing has changed — so a
/// failed withdrawal never reaches the teller's history, and can never be "undone" into
/// money the account did not have.
/// </para>
/// </summary>
public sealed class Withdraw : ITransaction
{
    private readonly Account _account;
    private readonly Money _amount;

    public Withdraw(Account account, Money amount)
    {
        _account = account;
        _amount = amount;
    }

    public void Execute() => _account.Withdraw(_amount);

    public void Undo() => _account.Deposit(_amount);

    public string Description => "withdraw " + _amount + " " + _account.Owner;
}
