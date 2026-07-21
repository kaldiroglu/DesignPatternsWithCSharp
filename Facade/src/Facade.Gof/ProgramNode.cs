namespace dev.kaldiroglu.Facade.Gof;

/// <summary>
/// Subsystem class (GoF p. 185): a node of the parse tree. Each node knows how to
/// <see cref="Traverse"/> itself, driving a <see cref="CodeGenerator"/> to emit
/// code. The concrete node types are <c>internal</c>: only the
/// <see cref="ProgramNodeBuilder"/> creates them, so clients of the
/// <see cref="Compiler"/> facade never see them.
/// </summary>
public abstract class ProgramNode
{
    /// <summary>Emits code for this node (and its children) via the generator.</summary>
    public abstract void Traverse(CodeGenerator generator);
}

/// <summary>Root node: an ordered list of statements.</summary>
internal sealed class BlockNode : ProgramNode
{
    private readonly List<ProgramNode> _statements = new();

    public void Add(ProgramNode statement) => _statements.Add(statement);

    public override void Traverse(CodeGenerator generator)
    {
        foreach (ProgramNode statement in _statements)
        {
            statement.Traverse(generator);
        }
    }
}

/// <summary>Expression: an integer constant.</summary>
internal sealed class ConstantNode(int value) : ProgramNode
{
    public override void Traverse(CodeGenerator generator) => generator.EmitPushConstant(value);
}

/// <summary>Expression: a reference to a variable.</summary>
internal sealed class VariableRefNode(string name) : ProgramNode
{
    public override void Traverse(CodeGenerator generator) => generator.EmitLoad(name);
}

/// <summary>Expression: a binary <c>+</c> or <c>-</c> operation.</summary>
internal sealed class BinaryOpNode(char op, ProgramNode left, ProgramNode right) : ProgramNode
{
    public override void Traverse(CodeGenerator generator)
    {
        left.Traverse(generator);
        right.Traverse(generator);
        if (op == '+') generator.EmitAdd();
        else generator.EmitSubtract();
    }
}

/// <summary>Statement: assign an expression's value to a variable.</summary>
internal sealed class AssignmentNode(string variable, ProgramNode expression) : ProgramNode
{
    public override void Traverse(CodeGenerator generator)
    {
        expression.Traverse(generator);
        generator.EmitStore(variable);
    }
}

/// <summary>Statement: return an expression's value.</summary>
internal sealed class ReturnNode(ProgramNode expression) : ProgramNode
{
    public override void Traverse(CodeGenerator generator)
    {
        expression.Traverse(generator);
        generator.EmitReturn();
    }
}
