namespace dev.kaldiroglu.Command.Account.Lambda;

using dev.kaldiroglu.Command.Account.Solution;

/// <summary>
/// A transaction made of three functions instead of a class.
/// <para>
/// <see cref="ITransaction"/> has three members, so a single lambda cannot implement it. This
/// record holds one function for each. The description is a <see cref="Func{TResult}"/>, not
/// a string, because a close-out only knows what it paid after it has run.
/// </para>
/// </summary>
public sealed record LambdaTransaction(Action OnExecute, Action OnUndo, Func<string> Describe)
    : ITransaction
{
    public void Execute() => OnExecute();

    public void Undo() => OnUndo();

    public string Description => Describe();
}
