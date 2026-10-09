using System.Globalization;

namespace dev.kaldiroglu.Visitor.Gof.Problem;

/// <summary>A number written in the program.</summary>
public sealed record ConstantNode(int Value) : INode
{
    public void TypeCheck(ISet<string> assigned, IList<string> errors)
    {
    }

    public void GenerateCode(IList<string> code)
    {
        code.Add("PUSH " + Value.ToString(CultureInfo.InvariantCulture));
    }

    public string PrettyPrint() => Value.ToString(CultureInfo.InvariantCulture);
}
