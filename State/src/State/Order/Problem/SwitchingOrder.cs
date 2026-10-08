namespace dev.kaldiroglu.State.Order.Problem;

/// <summary>
/// Stage two: <b>one status field, and a switch on it in every method.</b>
/// <para>
/// A real improvement on stage one. The order has exactly one status, so an order that is
/// both cancelled and shipped cannot exist. The switches have no default case, so a new
/// status that is missing from a switch does not compile. (In C# a missing constant is
/// warning CS8509, and <c>State.csproj</c> makes that warning an error.)
/// </para>
/// <para>
/// What it costs: the rules of one status are spread over every method. To read what a paid
/// order may do, you read five switches. A new status is an edit to every one of them.
/// </para>
/// </summary>
public sealed class SwitchingOrder
{
    private Status status = Status.PLACED;
    private readonly List<string> events = [];

    // A C# enum can hold a value that no constant names, so a switch with a case for every
    // constant still draws warning CS8524. A default case would silence it, and would also
    // silence CS8509, the warning that names a forgotten constant. So there is no default
    // case, and only CS8524 is suppressed.
#pragma warning disable CS8524
    public void Pay()
    {
        status = status switch
        {
            Status.PLACED => Record(Status.PAID, "paid"),
            Status.PAID or Status.SHIPPED or Status.DELIVERED or Status.CANCELLED => Reject("pay")
        };
    }

    public void Ship(string trackingNumber)
    {
        status = status switch
        {
            Status.PAID => Record(Status.SHIPPED, "shipped " + trackingNumber),
            Status.PLACED or Status.SHIPPED or Status.DELIVERED or Status.CANCELLED => Reject("ship")
        };
    }

    public void Deliver()
    {
        status = status switch
        {
            Status.SHIPPED => Record(Status.DELIVERED, "delivered"),
            Status.PLACED or Status.PAID or Status.DELIVERED or Status.CANCELLED => Reject("deliver")
        };
    }

    public void Cancel()
    {
        status = status switch
        {
            Status.PLACED => Record(Status.CANCELLED, "cancelled"),
            Status.PAID => Record(Status.CANCELLED, "cancelled, refund issued"),
            Status.SHIPPED or Status.DELIVERED or Status.CANCELLED => Reject("cancel")
        };
    }
#pragma warning restore CS8524

    public Status Status => status;

    public IReadOnlyList<string> Events => events.ToList().AsReadOnly();

    private Status Record(Status next, string @event)
    {
        events.Add(@event);
        return next;
    }

    private Status Reject(string action) =>
        throw new InvalidOperationException(
            "cannot " + action + " a " + status.ToString().ToLowerInvariant() + " order");
}
