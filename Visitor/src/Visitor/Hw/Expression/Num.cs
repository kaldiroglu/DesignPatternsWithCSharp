namespace dev.kaldiroglu.Visitor.Hw.Expression;

public sealed record Num(int Value) : IExpr
{
    public R Accept<R>(IExprVisitor<R> visitor) => visitor.Visit(this);
}
