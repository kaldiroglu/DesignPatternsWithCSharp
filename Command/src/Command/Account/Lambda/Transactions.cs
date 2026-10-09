namespace dev.kaldiroglu.Command.Account.Lambda;

using dev.kaldiroglu.Command.Account.Domain;
using dev.kaldiroglu.Command.Account.Solution;

/// <summary>
/// The four transactions of <c>Account.Solution</c>, written as lambdas.
/// <para>
/// A deposit, a withdrawal and a transfer are easy: each undo is the opposite operation. A
/// close-out must remember how much it took. The Java version shares a one-element array
/// between its lambdas, because a Java lambda cannot change a local variable. A C# lambda
/// can, so here the lambdas share the local <c>taken</c>; the compiler moves it into a hidden
/// class. Either way the state lives in an object — the reason <c>Solution.CloseOut</c> is a
/// class.
/// </para>
/// </summary>
public static class Transactions
{
    public static ITransaction Deposit(Domain.Account account, Money amount) =>
        new LambdaTransaction(
            OnExecute: () => account.Deposit(amount),
            OnUndo: () => account.Withdraw(amount),
            Describe: () => "deposit " + amount + " " + account.Owner);

    public static ITransaction Withdraw(Domain.Account account, Money amount) =>
        new LambdaTransaction(
            OnExecute: () => account.Withdraw(amount),
            OnUndo: () => account.Deposit(amount),
            Describe: () => "withdraw " + amount + " " + account.Owner);

    /// <summary>
    /// The withdrawal runs first. If it fails, it throws before anything has changed, so the
    /// transfer is still all or nothing; a deposit cannot fail.
    /// </summary>
    public static ITransaction Transfer(Domain.Account from, Domain.Account to, Money amount) =>
        new LambdaTransaction(
            OnExecute: () => { from.Withdraw(amount); to.Deposit(amount); },
            OnUndo: () => { to.Withdraw(amount); from.Deposit(amount); },
            Describe: () => "transfer " + amount + " " + from.Owner + " -> " + to.Owner);

    public static ITransaction CloseOut(Domain.Account account)
    {
        Money taken = Money.Zero;               // captured by the three lambdas: the state
        return new LambdaTransaction(
            OnExecute: () => { taken = account.Balance; account.Withdraw(taken); },
            OnUndo: () => account.Deposit(taken),
            Describe: () => "close out " + account.Owner + ", paid " + taken);
    }
}
