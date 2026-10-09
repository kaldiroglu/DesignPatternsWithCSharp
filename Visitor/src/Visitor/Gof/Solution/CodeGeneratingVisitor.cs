using System.Globalization;

namespace dev.kaldiroglu.Visitor.Gof.Solution;

/// <summary>A <b>ConcreteVisitor</b>: code for a stack machine. Children first, then the node.</summary>
public sealed class CodeGeneratingVisitor : INodeVisitor
{
    private readonly List<string> code = new();

    public void VisitAssignment(AssignmentNode node)
    {
        node.Value.Accept(this);
        code.Add("STORE " + node.Variable);
    }

    public void VisitVariableRef(VariableRefNode node)
    {
        code.Add("LOAD " + node.Name);
    }

    public void VisitConstant(ConstantNode node)
    {
        code.Add("PUSH " + node.Value.ToString(CultureInfo.InvariantCulture));
    }

    public void VisitAdd(AddNode node)
    {
        node.Left.Accept(this);
        node.Right.Accept(this);
        code.Add("ADD");
    }

    public IReadOnlyList<string> Code => code.ToList();
}
