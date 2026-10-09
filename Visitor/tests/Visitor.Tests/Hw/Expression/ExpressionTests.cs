using Xunit;
using E = global::dev.kaldiroglu.Visitor.Hw.Expression;

namespace dev.kaldiroglu.Visitor.Tests.Hw.Expression;

/// <summary>Homework three: the expression (2 + 3) * -4, evaluated, printed and measured.</summary>
public class ExpressionTests
{
    private static E.IExpr TheExpression() =>
        new E.Mul(new E.Add(new E.Num(2), new E.Num(3)), new E.Neg(new E.Num(4)));

    [Fact]
    public void Evaluate()
    {
        Assert.Equal(-20, TheExpression().Accept(new E.Evaluator()));
    }

    [Fact]
    public void Depth()
    {
        Assert.Equal(3, TheExpression().Accept(new E.DepthCounter()));
    }

    [Fact]
    public void Print()
    {
        Assert.Equal("((2 + 3) * -4)", TheExpression().Accept(new E.Printer()));
    }

    [Fact]
    public void FourVisitMethods()
    {
        Assert.Equal(4, typeof(E.IExprVisitor<>).GetMethods().Length);
    }
}
