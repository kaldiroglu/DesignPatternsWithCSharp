namespace dev.kaldiroglu.Visitor.Hw.Expression;

public sealed record Add(IExpr Left, IExpr Right) : IExpr
{
    public R Accept<R>(IExprVisitor<R> visitor) => visitor.Visit(this);
}
