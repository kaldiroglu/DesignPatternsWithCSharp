namespace dev.kaldiroglu.State.Order.Problem;

/// <summary>
/// Stage one: <b>the order's status is four boolean flags.</b>
/// <para>
/// Every method checks the flags it needs. It works, and every rule of the order can be
/// found in these four methods. What it costs:
/// </para>
/// <list type="bullet">
///   <item>Four flags make sixteen combinations, and only five of them are real statuses.
///   Nothing stops a bug from making an order that is both cancelled and shipped.</item>
///   <item>Each method needs its own chain of <c>if</c>s, and each chain must list every flag
///   that matters to it.</item>
/// </list>
/// </summary>
public sealed class FlagOrder
{
    private bool paid;
    private bool shipped;
    private bool delivered;
    private bool cancelled;
    private readonly List<string> events = [];

    public void Pay()
    {
        if (cancelled || paid)
        {
            throw new InvalidOperationException("cannot pay this order");
        }
        paid = true;
        events.Add("paid");
    }

    public void Ship(string trackingNumber)
    {
        if (!paid || shipped || cancelled)
        {
            throw new InvalidOperationException("cannot ship this order");
        }
        shipped = true;
        events.Add("shipped " + trackingNumber);
    }

    public void Deliver()
    {
        if (!shipped || delivered)
        {
            throw new InvalidOperationException("cannot deliver this order");
        }
        delivered = true;
        events.Add("delivered");
    }

    public void Cancel()
    {
        if (shipped || cancelled)
        {
            throw new InvalidOperationException("cannot cancel this order");
        }
        cancelled = true;
        events.Add(paid ? "cancelled, refund issued" : "cancelled");
    }

    public IReadOnlyList<string> Events => events.ToList().AsReadOnly();
}
