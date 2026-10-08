namespace dev.kaldiroglu.State.Order.Problem;

/// <summary>
/// Stage three's order. It forwards every request to its <see cref="OrderStatus"/> constant,
/// and it also holds the data that belongs to one status only: the tracking number and the
/// failed attempts of a shipment.
/// <para>
/// The promise: a courier gets three delivery attempts. After three failures the parcel goes
/// back to the warehouse and can be shipped again — with three new attempts. Here the second
/// shipment starts at three, because nothing reset the count. Its first failure is counted as
/// attempt 4, which is never equal to 3, so the parcel never goes back again: the courier now
/// has no limit at all.
/// </para>
/// </summary>
public sealed class EnumOrder
{
    internal const int MaxAttempts = 3;

    private OrderStatus status = OrderStatus.PLACED;
    private readonly List<string> events = [];

    /// <summary>Means something only while the order is <c>SHIPPED</c>.</summary>
    internal string? TrackingNumber { get; set; }

    /// <summary>Means something only while the order is <c>SHIPPED</c>.</summary>
    internal int FailedAttempts { get; set; }

    public void Pay()
    {
        status = status.Pay(this);
    }

    public void Ship(string trackingNumber)
    {
        status = status.Ship(this, trackingNumber);
    }

    public void FailDelivery()
    {
        status = status.FailDelivery(this);
    }

    public void Deliver()
    {
        status = status.Deliver(this);
    }

    public void Cancel()
    {
        status = status.Cancel(this);
    }

    public OrderStatus Status => status;

    internal void Event(string @event)
    {
        events.Add(@event);
    }

    public IReadOnlyList<string> Events => events.ToList().AsReadOnly();
}
