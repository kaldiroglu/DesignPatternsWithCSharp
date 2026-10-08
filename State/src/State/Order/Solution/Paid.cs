namespace dev.kaldiroglu.State.Order.Solution;

/// <summary>A <b>ConcreteState</b>: paid and waiting in the warehouse. It may be shipped or cancelled.</summary>
public sealed record Paid() : IOrderState
{
    public string Name => "paid";

    public IOrderState Ship(Order order, string trackingNumber)
    {
        order.Event("shipped " + trackingNumber);
        return new Shipped(trackingNumber, 0);
    }

    public IOrderState Cancel(Order order)
    {
        order.Event("cancelled, refund issued");
        return new Cancelled();
    }

    /// <summary>Prints the record the way Java prints it.</summary>
    public override string ToString() => "Paid[]";
}
