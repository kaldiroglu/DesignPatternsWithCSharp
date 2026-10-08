namespace dev.kaldiroglu.State.Order.Solution;

/// <summary>
/// A <b>ConcreteState</b> with its own data: the tracking number and the failed delivery
/// attempts of this one shipment.
/// <para>
/// Compare <c>Problem.OrderStatus.SHIPPED</c>, whose data had to live in the order. Here a
/// new shipment is a new <c>Shipped</c> object, so its count starts at zero — nothing has to
/// remember to reset it. When the shipment ends, its data ends with it.
/// </para>
/// </summary>
public sealed record Shipped(string TrackingNumber, int FailedAttempts) : IOrderState
{
    public const int MaxAttempts = 3;

    public string Name => "shipped";

    public IOrderState FailDelivery(Order order)
    {
        int attempts = FailedAttempts + 1;
        order.Event("delivery attempt " + attempts + " failed");
        if (attempts == MaxAttempts)
        {
            order.Event("back to the warehouse");
            return new Paid();
        }
        return new Shipped(TrackingNumber, attempts);
    }

    public IOrderState Deliver(Order order)
    {
        order.Event("delivered");
        return new Delivered();
    }

    /// <summary>
    /// Prints the record the way Java prints it: <c>Shipped[trackingNumber=TR-2, failedAttempts=1]</c>.
    /// A C# record would print <c>Shipped { TrackingNumber = TR-2, ... }</c>.
    /// </summary>
    public override string ToString() =>
        "Shipped[trackingNumber=" + TrackingNumber + ", failedAttempts=" + FailedAttempts + "]";
}
