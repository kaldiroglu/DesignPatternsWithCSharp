namespace dev.kaldiroglu.Command.Account.Solution;

/// <summary>
/// The <b>Command</b>: one request to the bank, as an object.
/// <para>
/// GoF list "Transaction" as one of this pattern's other names (p. 233), and in a bank it is
/// the obvious one. A transaction knows which account it touches and by how much, so
/// <see cref="Execute"/> takes no arguments: everything the request needs was given to it
/// when it was made. That is what lets a teller hold one, put it on a list, run it tonight,
/// or take it back.
/// </para>
/// <para>
/// Redo is not on the interface. To redo a transaction is to execute it again, so the
/// invoker does it with the method that is already here.
/// </para>
/// </summary>
public interface ITransaction
{
    void Execute();

    void Undo();

    /// <summary>One line for the journal.</summary>
    string Description { get; }
}
