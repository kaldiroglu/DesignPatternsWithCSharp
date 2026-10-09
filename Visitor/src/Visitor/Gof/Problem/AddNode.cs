namespace dev.kaldiroglu.Visitor.Gof.Problem;

/// <summary><c>left + right</c></summary>
public sealed record AddNode(INode Left, INode Right) : INode
{
    public void TypeCheck(ISet<string> assigned, IList<string> errors)
    {
        Left.TypeCheck(assigned, errors);
        Right.TypeCheck(assigned, errors);
    }

    public void GenerateCode(IList<string> code)
    {
        Left.GenerateCode(code);
        Right.GenerateCode(code);
        code.Add("ADD");
    }

    public string PrettyPrint() => Left.PrettyPrint() + " + " + Right.PrettyPrint();
}
