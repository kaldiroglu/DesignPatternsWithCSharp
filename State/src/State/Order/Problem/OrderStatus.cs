namespace dev.kaldiroglu.State.Order.Problem;

/// <summary>
/// Stage three: <b>each status is an enum constant with its own behavior.</b>
/// <para>
/// The best of the three. The rules of one status are in one place: read <c>SHIPPED</c> to
/// see what a shipped order may do. Each constant returns the next status, so the
/// transitions are in the statuses. A new status is one new constant.
/// </para>
/// <para>
/// The limit: an enum constant is one object shared by every order. It cannot hold one
/// order's data. A shipped order now has a tracking number and a count of failed delivery
/// attempts, and both have to live in <see cref="EnumOrder"/> — fields that mean something
/// only while the order is shipped, and that every transition must remember to set or reset.
/// <c>PAID.Ship</c> forgets to reset the count.
/// </para>
/// <para>
/// A C# enum cannot have methods, so this is a class with a private constructor and five
/// shared instances, one per constant. Each instance is a private nested class that overrides
/// the operations it allows — the same shape as a Java enum constant with its own body. The
/// constants keep the Java names, because <c>Main</c> prints the status.
/// </para>
/// </summary>
public abstract class OrderStatus
{
    public static readonly OrderStatus PLACED = new PlacedStatus();
    public static readonly OrderStatus PAID = new PaidStatus();
    public static readonly OrderStatus SHIPPED = new ShippedStatus();
    public static readonly OrderStatus DELIVERED = new DeliveredStatus();
    public static readonly OrderStatus CANCELLED = new CancelledStatus();

    private OrderStatus(string name)
    {
        Name = name;
    }

    /// <summary>The constant's name, as Java's <c>name()</c> returns it.</summary>
    public string Name { get; }

    public override string ToString() => Name;

    internal virtual OrderStatus Pay(EnumOrder order) => throw Reject("pay");

    internal virtual OrderStatus Ship(EnumOrder order, string trackingNumber) => throw Reject("ship");

    internal virtual OrderStatus FailDelivery(EnumOrder order) => throw Reject("record a failed delivery for");

    internal virtual OrderStatus Deliver(EnumOrder order) => throw Reject("deliver");

    internal virtual OrderStatus Cancel(EnumOrder order) => throw Reject("cancel");

    private InvalidOperationException Reject(string action) =>
        new("cannot " + action + " a " + Name.ToLowerInvariant() + " order");

    private sealed class PlacedStatus() : OrderStatus("PLACED")
    {
        internal override OrderStatus Pay(EnumOrder order)
        {
            order.Event("paid");
            return PAID;
        }

        internal override OrderStatus Cancel(EnumOrder order)
        {
            order.Event("cancelled");
            return CANCELLED;
        }
    }

    private sealed class PaidStatus() : OrderStatus("PAID")
    {
        internal override OrderStatus Ship(EnumOrder order, string trackingNumber)
        {
            order.TrackingNumber = trackingNumber;   // the attempt count is not reset
            order.Event("shipped " + trackingNumber);
            return SHIPPED;
        }

        internal override OrderStatus Cancel(EnumOrder order)
        {
            order.Event("cancelled, refund issued");
            return CANCELLED;
        }
    }

    private sealed class ShippedStatus() : OrderStatus("SHIPPED")
    {
        internal override OrderStatus FailDelivery(EnumOrder order)
        {
            order.FailedAttempts++;
            order.Event("delivery attempt " + order.FailedAttempts + " failed");
            if (order.FailedAttempts == EnumOrder.MaxAttempts)
            {
                order.Event("back to the warehouse");
                return PAID;
            }
            return SHIPPED;
        }

        internal override OrderStatus Deliver(EnumOrder order)
        {
            order.Event("delivered");
            return DELIVERED;
        }
    }

    private sealed class DeliveredStatus() : OrderStatus("DELIVERED");

    private sealed class CancelledStatus() : OrderStatus("CANCELLED");
}
