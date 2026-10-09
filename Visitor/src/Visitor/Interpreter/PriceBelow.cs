using System.Globalization;

namespace dev.kaldiroglu.Visitor.Interpreter;

/// <summary>A terminal expression: the product costs less than this.</summary>
public sealed record PriceBelow(int Limit) : IRule
{
    public bool Interpret(Product product) => product.Price < Limit;

    public string Describe() => "price below " + Limit.ToString(CultureInfo.InvariantCulture);
}
