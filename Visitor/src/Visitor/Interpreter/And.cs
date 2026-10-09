namespace dev.kaldiroglu.Visitor.Interpreter;

/// <summary>A nonterminal expression: both rules hold.</summary>
public sealed record And(IRule Left, IRule Right) : IRule
{
    public bool Interpret(Product product) => Left.Interpret(product) && Right.Interpret(product);

    public string Describe() => "(" + Left.Describe() + " and " + Right.Describe() + ")";
}
