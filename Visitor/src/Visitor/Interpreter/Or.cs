namespace dev.kaldiroglu.Visitor.Interpreter;

/// <summary>A nonterminal expression: at least one rule holds.</summary>
public sealed record Or(IRule Left, IRule Right) : IRule
{
    public bool Interpret(Product product) => Left.Interpret(product) || Right.Interpret(product);

    public string Describe() => "(" + Left.Describe() + " or " + Right.Describe() + ")";
}
