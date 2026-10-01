namespace dev.kaldiroglu.Command.Account.Solution;

/// <summary>
/// Requests made during the day and carried out at night.
/// <para>
/// A method call happens when it is made. A transaction is an object, so it can be made at
/// ten in the morning and executed at midnight — and in between it is just something in a
/// queue. This is the "queue requests" half of GoF's intent, and it needs nothing from the
/// transactions that undo did not already need: they were complete when they were built.
/// </para>
/// <para>
/// The night run goes through an ordinary <see cref="Teller"/>, so everything done tonight
/// is in the journal and can be undone tomorrow like anything else.
/// </para>
/// </summary>
public sealed class StandingOrders
{
    private readonly Queue<ITransaction> _tonight = new();

    public void Schedule(ITransaction transaction) => _tonight.Enqueue(transaction);

    public int Pending => _tonight.Count;

    /// <summary>Carry out everything scheduled, in the order it was scheduled.</summary>
    public void RunThrough(Teller teller)
    {
        while (_tonight.Count > 0)
        {
            teller.Perform(_tonight.Dequeue());
        }
    }
}
