namespace dev.kaldiroglu.Visitor.Hw.Expression;

public sealed record Neg(IExpr Operand) : IExpr
{
    public R Accept<R>(IExprVisitor<R> visitor) => visitor.Visit(this);
}
