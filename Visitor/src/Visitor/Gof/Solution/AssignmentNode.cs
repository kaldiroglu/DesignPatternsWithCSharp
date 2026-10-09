namespace dev.kaldiroglu.Visitor.Gof.Solution;

/// <summary>A <b>ConcreteElement</b>: <c>variable = value</c></summary>
public sealed record AssignmentNode(string Variable, INode Value) : INode
{
    public void Accept(INodeVisitor visitor) => visitor.VisitAssignment(this);
}
