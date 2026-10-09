namespace dev.kaldiroglu.Visitor.Gof.Problem;

/// <summary>A use of a variable.</summary>
public sealed record VariableRefNode(string Name) : INode
{
    public void TypeCheck(ISet<string> assigned, IList<string> errors)
    {
        if (!assigned.Contains(Name))
        {
            errors.Add(Name + " is used before it is assigned");
        }
    }

    public void GenerateCode(IList<string> code)
    {
        code.Add("LOAD " + Name);
    }

    public string PrettyPrint() => Name;
}
