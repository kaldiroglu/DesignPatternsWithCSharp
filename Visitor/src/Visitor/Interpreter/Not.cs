namespace dev.kaldiroglu.Visitor.Interpreter;

/// <summary>A nonterminal expression: the rule does not hold.</summary>
public sealed record Not(IRule Rule) : IRule
{
    public bool Interpret(Product product) => !Rule.Interpret(product);

    public string Describe() => "not " + Rule.Describe();
}
