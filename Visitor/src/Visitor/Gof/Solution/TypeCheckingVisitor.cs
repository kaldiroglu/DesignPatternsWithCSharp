namespace dev.kaldiroglu.Visitor.Gof.Solution;

/// <summary>
/// A <b>ConcreteVisitor</b>: reports a variable used before it is assigned.
/// <para>
/// The visitor walks the tree itself — it calls <c>Accept</c> on the children. This is one
/// answer to GoF implementation issue 2 (who is responsible for traversing the object
/// structure?). The visitor also keeps state between nodes: the variables assigned so far.
/// </para>
/// </summary>
public sealed class TypeCheckingVisitor : INodeVisitor
{
    private readonly HashSet<string> assigned = new();
    private readonly List<string> errors = new();

    public void VisitAssignment(AssignmentNode node)
    {
        node.Value.Accept(this);
        assigned.Add(node.Variable);
    }

    public void VisitVariableRef(VariableRefNode node)
    {
        if (!assigned.Contains(node.Name))
        {
            errors.Add(node.Name + " is used before it is assigned");
        }
    }

    public void VisitConstant(ConstantNode node)
    {
    }

    public void VisitAdd(AddNode node)
    {
        node.Left.Accept(this);
        node.Right.Accept(this);
    }

    public IReadOnlyList<string> Errors => errors.ToList();
}
