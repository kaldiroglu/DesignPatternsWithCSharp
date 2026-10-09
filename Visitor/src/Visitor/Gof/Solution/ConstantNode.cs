namespace dev.kaldiroglu.Visitor.Gof.Solution;

/// <summary>A <b>ConcreteElement</b>: A number written in the program.</summary>
public sealed record ConstantNode(int Value) : INode
{
    public void Accept(INodeVisitor visitor) => visitor.VisitConstant(this);
}
