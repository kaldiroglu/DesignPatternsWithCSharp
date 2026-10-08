namespace dev.kaldiroglu.State.Order.Solution;

/// <summary>A <b>ConcreteState</b>: the other end. Every operation is refused.</summary>
public sealed record Cancelled() : IOrderState
{
    public string Name => "cancelled";

    /// <summary>Prints the record the way Java prints it.</summary>
    public override string ToString() => "Cancelled[]";
}
