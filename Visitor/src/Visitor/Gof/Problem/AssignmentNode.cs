namespace dev.kaldiroglu.Visitor.Gof.Problem;

/// <summary><c>variable = value</c></summary>
public sealed record AssignmentNode(string Variable, INode Value) : INode
{
    public void TypeCheck(ISet<string> assigned, IList<string> errors)
    {
        Value.TypeCheck(assigned, errors);
        assigned.Add(Variable);
    }

    public void GenerateCode(IList<string> code)
    {
        Value.GenerateCode(code);
        code.Add("STORE " + Variable);
    }

    public string PrettyPrint() => Variable + " = " + Value.PrettyPrint();
}
