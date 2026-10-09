using System.Globalization;
using System.Text;

namespace dev.kaldiroglu.Visitor.Gof.Solution;

/// <summary>A <b>ConcreteVisitor</b>: prints the program back as text.</summary>
public sealed class PrettyPrintingVisitor : INodeVisitor
{
    private readonly StringBuilder text = new();

    public void VisitAssignment(AssignmentNode node)
    {
        text.Append(node.Variable).Append(" = ");
        node.Value.Accept(this);
    }

    public void VisitVariableRef(VariableRefNode node)
    {
        text.Append(node.Name);
    }

    public void VisitConstant(ConstantNode node)
    {
        text.Append(node.Value.ToString(CultureInfo.InvariantCulture));
    }

    public void VisitAdd(AddNode node)
    {
        node.Left.Accept(this);
        text.Append(" + ");
        node.Right.Accept(this);
    }

    public string Text => text.ToString();
}
