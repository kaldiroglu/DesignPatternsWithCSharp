namespace dev.kaldiroglu.State.Order.Solution;

/// <summary>A <b>ConcreteState</b>: the other end. Every operation is refused.</summary>
public sealed record Cancelled() : IOrderState
{
    public string Name => "cancelled";

    /// <summary>Prints the record as <c>Cancelled[]</c>.</summary>
    public override string ToString() => "Cancelled[]";
}
