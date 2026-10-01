namespace dev.kaldiroglu.Command.Account.Problem;

using dev.kaldiroglu.Command.Account.Domain;

/// <summary>
/// Stage one: <b>the account remembers its last move.</b>
/// <para>
/// The teller screen gets an Undo button, and the quickest place to put undo is on the
/// account itself. So the account writes down what it last did and how much, and
/// <see cref="Undo"/> branches on that to do the opposite.
/// </para>
/// <para>
/// It works, and for a single slip it is all a teller needs. What it costs:
/// </para>
/// <list type="bullet">
///   <item>One step only. A second undo finds nothing to reverse, because the account
///       remembers one operation and the first undo erased it.</item>
///   <item>Two of the account's three fields are not about money. They are bookkeeping for a
///       screen the account should never have heard of.</item>
///   <item>Every new operation is an edit to <c>Undo()</c> as well as a new method.</item>
/// </list>
/// </summary>
public sealed class OneStepAccount
{
    private readonly string _owner;

    private string _lastKind = "NONE";          // what the last operation was
    private Money _lastAmount = Money.Zero;     // and how much it moved

    public OneStepAccount(string owner, Money opening)
    {
        _owner = owner;
        Balance = opening;
    }

    public Money Balance { get; private set; }

    public void Deposit(Money amount)
    {
        Balance = Balance.Plus(amount);
        _lastKind = "DEPOSIT";
        _lastAmount = amount;
    }

    public void Withdraw(Money amount)
    {
        if (Balance.IsLessThan(amount))
        {
            throw new InsufficientFundsException(_owner, Balance, amount);
        }

        Balance = Balance.Minus(amount);
        _lastKind = "WITHDRAW";
        _lastAmount = amount;
    }

    public void Undo()
    {
        switch (_lastKind)
        {
            case "DEPOSIT":
                Balance = Balance.Minus(_lastAmount);
                break;
            case "WITHDRAW":
                Balance = Balance.Plus(_lastAmount);
                break;
            default:                            // nothing to undo
                break;
        }

        _lastKind = "NONE";
    }
}
