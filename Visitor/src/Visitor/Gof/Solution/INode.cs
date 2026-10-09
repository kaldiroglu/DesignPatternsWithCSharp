namespace dev.kaldiroglu.Visitor.Gof.Solution;

/// <summary>The <b>Element</b>: a node only accepts a visitor. It no longer knows any compiler job.</summary>
public interface INode
{
    void Accept(INodeVisitor visitor);
}
