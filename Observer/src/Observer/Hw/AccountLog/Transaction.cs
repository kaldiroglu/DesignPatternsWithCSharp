namespace dev.kaldiroglu.Observer.Hw.AccountLog;

/// <summary>One record of a change: who, how much, and the balance after it.</summary>
public sealed record Transaction(string Owner, string Kind, int Amount, int BalanceAfter)
{
    /// <summary>Prints the way a Java record prints: <c>Transaction[owner=Deniz, kind=deposit, …]</c>.</summary>
    public override string ToString() =>
        $"Transaction[owner={Owner}, kind={Kind}, amount={Amount}, balanceAfter={BalanceAfter}]";
}
