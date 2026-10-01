namespace dev.kaldiroglu.Command.Account.Domain;

/// <summary>
/// A bank account that does banking and nothing else.
/// <para>
/// This is the class the whole example is trying to protect. It has an owner, a balance and
/// the two operations a balance supports. It does not know what a teller is, what undo
/// means, or that anybody keeps a journal — and in <c>Problem.OneStepAccount</c> and
/// <c>Problem.HistoryAccount</c> you can see what it looks like when it is made to.
/// </para>
/// <para>
/// In the pattern's vocabulary this is the <b>Receiver</b>: the object that knows how to
/// carry out the request.
/// </para>
/// </summary>
public sealed class Account
{
    public Account(string owner, Money opening)
    {
        ArgumentNullException.ThrowIfNull(owner);
        ArgumentNullException.ThrowIfNull(opening);
        Owner = owner;
        Balance = opening;
    }

    public string Owner { get; }

    public Money Balance { get; private set; }

    public void Deposit(Money amount)
    {
        Balance = Balance.Plus(amount);
    }

    public void Withdraw(Money amount)
    {
        if (Balance.IsLessThan(amount))
        {
            throw new InsufficientFundsException(Owner, Balance, amount);
        }

        Balance = Balance.Minus(amount);
    }
}
