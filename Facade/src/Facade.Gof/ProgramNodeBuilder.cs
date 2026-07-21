namespace dev.kaldiroglu.Facade.Gof;

/// <summary>
/// Subsystem class (GoF p. 185): builds the parse tree. The <see cref="Parser"/>
/// calls the <c>New*</c> factory methods to create <see cref="ProgramNode"/>s
/// without knowing their concrete classes, and collects top-level statements into
/// a root block returned by <see cref="GetRootNode"/>.
/// </summary>
public class ProgramNodeBuilder
{
    private readonly BlockNode _root = new();

    public ProgramNode NewConstant(int value) => new ConstantNode(value);

    public ProgramNode NewVariable(string name) => new VariableRefNode(name);

    public ProgramNode NewBinaryOperator(char op, ProgramNode left, ProgramNode right) =>
        new BinaryOpNode(op, left, right);

    public ProgramNode NewAssignment(string variable, ProgramNode expression) =>
        new AssignmentNode(variable, expression);

    public ProgramNode NewReturn(ProgramNode expression) => new ReturnNode(expression);

    public void AddStatement(ProgramNode statement) => _root.Add(statement);

    public ProgramNode GetRootNode() => _root;
}
