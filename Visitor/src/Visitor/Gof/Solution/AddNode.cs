namespace dev.kaldiroglu.Visitor.Gof.Solution;

/// <summary>A <b>ConcreteElement</b>: <c>left + right</c></summary>
public sealed record AddNode(INode Left, INode Right) : INode
{
    public void Accept(INodeVisitor visitor) => visitor.VisitAdd(this);
}
