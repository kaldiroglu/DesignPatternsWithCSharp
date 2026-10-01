namespace dev.kaldiroglu.Command.Account.Domain;

/// <summary>A withdrawal larger than the balance. Nothing has changed when this is thrown.</summary>
public sealed class InsufficientFundsException : Exception
{
    public InsufficientFundsException(string owner, Money balance, Money requested)
        : base($"{owner} holds {balance}, which is less than {requested}")
    {
    }
}
