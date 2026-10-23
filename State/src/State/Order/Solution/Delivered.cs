namespace dev.kaldiroglu.State.Order.Solution;

/// <summary>A <b>ConcreteState</b>: the end. Every operation is refused.</summary>
public sealed record Delivered() : IOrderState
{
    public string Name => "delivered";

    /// <summary>Prints the record as <c>Delivered[]</c>.</summary>
    public override string ToString() => "Delivered[]";
}
