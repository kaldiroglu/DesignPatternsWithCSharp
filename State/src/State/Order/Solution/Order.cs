namespace dev.kaldiroglu.State.Order.Solution;

/// <summary>
/// The <b>Context</b>: an order that forwards every request to its current state.
/// <para>
/// Compare the three orders in <c>Problem</c>. This one has no flags, no switch and no
/// fields that belong to one status. Each method is one line: ask the state, and keep the
/// state it answers with. To the caller, the order seems to change its class as its status
/// changes — which is how GoF's intent puts it.
/// </para>
/// </summary>
public sealed class Order
{
    private IOrderState state = new Placed();
    private readonly List<string> events = [];

    public void Pay()
    {
        state = state.Pay(this);
    }

    public void Ship(string trackingNumber)
    {
        state = state.Ship(this, trackingNumber);
    }

    public void FailDelivery()
    {
        state = state.FailDelivery(this);
    }

    public void Deliver()
    {
        state = state.Deliver(this);
    }

    public void Cancel()
    {
        state = state.Cancel(this);
    }

    public IOrderState State => state;

    internal void Event(string @event)
    {
        events.Add(@event);
    }

    public IReadOnlyList<string> Events => events.ToList().AsReadOnly();
}
