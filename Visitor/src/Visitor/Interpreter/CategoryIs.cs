namespace dev.kaldiroglu.Visitor.Interpreter;

/// <summary>A terminal expression: the product is in this category.</summary>
public sealed record CategoryIs(string Category) : IRule
{
    public bool Interpret(Product product) => product.Category == Category;

    public string Describe() => "category is " + Category;
}
