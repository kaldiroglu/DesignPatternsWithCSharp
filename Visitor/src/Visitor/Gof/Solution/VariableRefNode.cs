namespace dev.kaldiroglu.Visitor.Gof.Solution;

/// <summary>A <b>ConcreteElement</b>: A use of a variable.</summary>
public sealed record VariableRefNode(string Name) : INode
{
    public void Accept(INodeVisitor visitor) => visitor.VisitVariableRef(this);
}
