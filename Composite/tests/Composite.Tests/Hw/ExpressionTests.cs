using dev.kaldiroglu.Composite.Hw.Expression;
using Xunit;

namespace dev.kaldiroglu.Composite.Tests.Hw;

/// <summary>Homework 3: the tree is the arithmetic, and the recursion is the evaluation.</summary>
public class ExpressionTests
{
    // (3 + 4) * (10 - 2) / 4
    private readonly IExpression _expression = new Divide(
        new Multiply(
            new Add(new Number(3), new Number(4)),
            new Subtract(new Number(10), new Number(2))),
        new Number(4));

    [Fact(DisplayName = "A leaf evaluates to itself")]
    public void ALeafEvaluatesToItself()
    {
        Assert.Equal(3, new Number(3).Evaluate());
        Assert.Equal(1, new Number(3).NodeCount());
        Assert.Equal("3", new Number(3).ToText());
    }

    [Fact(DisplayName = "The tree evaluates by recursion")]
    public void TheTreeEvaluates()
    {
        Assert.Equal(14, _expression.Evaluate());
        Assert.Equal(9, _expression.NodeCount());
        Assert.Equal("(((3 + 4) * (10 - 2)) / 4)", _expression.ToText());
    }

    [Fact(DisplayName = "A sub-expression can be swapped for a number and nothing notices")]
    public void ALeafSubstitutesForAComposite()
    {
        IExpression simplified = new Divide(new Number(56), new Number(4));

        Assert.Equal(_expression.Evaluate(), simplified.Evaluate());
        Assert.Equal(3, simplified.NodeCount());
    }

    [Fact(DisplayName = "A node that cannot answer says so")]
    public void DivisionByZeroThrows()
    {
        var failure = Assert.Throws<DivideByZeroException>(
            () => new Divide(new Number(1), new Subtract(new Number(5), new Number(5))).Evaluate());
        Assert.Contains("division by zero", failure.Message);
    }
}
