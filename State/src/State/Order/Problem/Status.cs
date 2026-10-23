namespace dev.kaldiroglu.State.Order.Problem;

/// <summary>
/// Stage two's statuses. One field, so an order has exactly one of them.
/// <para>
/// The constants are named in capitals, because <see cref="SwitchingOrder"/> prints a
/// constant's name in its error message.
/// </para>
/// </summary>
public enum Status
{
    PLACED,
    PAID,
    SHIPPED,
    DELIVERED,
    CANCELLED
}
