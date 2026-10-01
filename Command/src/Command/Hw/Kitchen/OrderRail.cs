namespace dev.kaldiroglu.Command.Hw.Kitchen;

/// <summary>
/// The <b>Invoker</b>: the rail of tickets above the kitchen pass.
/// <para>
/// The homework's question is who knows whether an order can still be cancelled. The answer
/// is this class, and nobody else: an order on the rail has not started, and an order off
/// it has. Cancelling is removing a ticket that has not run — the order needs no undo,
/// because nothing has happened yet that would need reversing. A dish already cooked is not
/// undone; it is a refund, which is a different request.
/// </para>
/// </summary>
public sealed class OrderRail
{
    private readonly LinkedList<Order> _waiting = new();

    public void Place(Order order) => _waiting.AddLast(order);

    /// <summary>Answers false when the kitchen has already started it.</summary>
    public bool Cancel(Order order) => _waiting.Remove(order);

    public int Waiting => _waiting.Count;

    /// <summary>The kitchen takes the oldest ticket.</summary>
    public void CookNext()
    {
        var next = _waiting.First;
        if (next is not null)
        {
            _waiting.RemoveFirst();
            next.Value.Execute();
        }
    }
}
