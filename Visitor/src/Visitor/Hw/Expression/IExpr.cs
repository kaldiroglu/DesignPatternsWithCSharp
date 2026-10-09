namespace dev.kaldiroglu.Visitor.Hw.Expression;

/// <summary>Homework 3: an arithmetic expression as a tree.</summary>
public interface IExpr
{
    R Accept<R>(IExprVisitor<R> visitor);
}
