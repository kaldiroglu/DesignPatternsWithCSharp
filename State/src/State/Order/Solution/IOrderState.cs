namespace dev.kaldiroglu.State.Order.Solution;

/// <summary>
/// The <b>State</b>: what an order may do in one status.
/// <para>
/// Each operation returns the order's next state, so the states decide the transitions. By
/// default every operation is refused; a state overrides only the operations it allows. GoF
/// implementation issue 1 (who defines the state transitions?): here, the states.
/// </para>
/// <para>
/// In Java the interface is <c>sealed</c>: these five states are all there are, and the
/// compiler knows it. C# has no sealed interface. The closest form is used here: the five
/// states are <c>sealed record</c>s, so none of them can be extended, but the compiler does
/// not stop another class from implementing this interface.
/// </para>
/// </summary>
public interface IOrderState
{
    string Name { get; }

    IOrderState Pay(Order order) => throw Reject("pay");

    IOrderState Ship(Order order, string trackingNumber) => throw Reject("ship");

    IOrderState FailDelivery(Order order) => throw Reject("record a failed delivery for");

    IOrderState Deliver(Order order) => throw Reject("deliver");

    IOrderState Cancel(Order order) => throw Reject("cancel");

    private InvalidOperationException Reject(string action) =>
        new("cannot " + action + " a " + Name + " order");
}
