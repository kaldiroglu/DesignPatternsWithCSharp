namespace dev.kaldiroglu.Visitor.Gof.Solution;

/// <summary>
/// The <b>Visitor</b>: one operation for each kind of node. GoF name the methods after the
/// class — <c>VisitAssignment</c>, <c>VisitVariableRef</c> — because not every language
/// has overloading.
/// </summary>
public interface INodeVisitor
{
    void VisitAssignment(AssignmentNode node);

    void VisitVariableRef(VariableRefNode node);

    void VisitConstant(ConstantNode node);

    void VisitAdd(AddNode node);
}
