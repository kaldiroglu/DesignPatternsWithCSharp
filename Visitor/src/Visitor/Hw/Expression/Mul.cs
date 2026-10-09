namespace dev.kaldiroglu.Visitor.Hw.Expression;

public sealed record Mul(IExpr Left, IExpr Right) : IExpr
{
    public R Accept<R>(IExprVisitor<R> visitor) => visitor.Visit(this);
}
