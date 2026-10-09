namespace dev.kaldiroglu.Visitor.Checkout.Solution;

/// <summary>A <b>ConcreteElement</b>: data, and one line that calls the visitor back.</summary>
public sealed record Electronics(string Name, int Price) : IItem
{
    public R Accept<R>(IItemVisitor<R> visitor) => visitor.Visit(this);
}
