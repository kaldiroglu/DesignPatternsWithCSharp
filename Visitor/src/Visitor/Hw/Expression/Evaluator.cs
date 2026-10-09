namespace dev.kaldiroglu.Visitor.Hw.Expression;

/// <summary>Computes the value. The visitor walks the tree by calling <c>Accept</c> on the children.</summary>
public sealed class Evaluator : IExprVisitor<int>
{
    public int Visit(Num num) => num.Value;

    public int Visit(Add add) => add.Left.Accept(this) + add.Right.Accept(this);

    public int Visit(Mul mul) => mul.Left.Accept(this) * mul.Right.Accept(this);

    public int Visit(Neg neg) => -neg.Operand.Accept(this);
}
