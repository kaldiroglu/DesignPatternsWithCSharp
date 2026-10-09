using System.Globalization;

namespace dev.kaldiroglu.Visitor.Hw.Expression;

/// <summary>Writes the expression with brackets around every sum and product.</summary>
public sealed class Printer : IExprVisitor<string>
{
    public string Visit(Num num) => num.Value.ToString(CultureInfo.InvariantCulture);

    public string Visit(Add add) => "(" + add.Left.Accept(this) + " + " + add.Right.Accept(this) + ")";

    public string Visit(Mul mul) => "(" + mul.Left.Accept(this) + " * " + mul.Right.Accept(this) + ")";

    public string Visit(Neg neg) => "-" + neg.Operand.Accept(this);
}
