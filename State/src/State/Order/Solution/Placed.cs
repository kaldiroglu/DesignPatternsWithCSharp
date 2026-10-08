namespace dev.kaldiroglu.State.Order.Solution;

/// <summary>A <b>ConcreteState</b>: the order exists and is not paid. It may be paid or cancelled.</summary>
public sealed record Placed() : IOrderState
{
    public string Name => "placed";

    public IOrderState Pay(Order order)
    {
        order.Event("paid");
        return new Paid();
    }

    public IOrderState Cancel(Order order)
    {
        order.Event("cancelled");
        return new Cancelled();
    }

    /// <summary>Prints the record the way Java prints it.</summary>
    public override string ToString() => "Placed[]";
}
