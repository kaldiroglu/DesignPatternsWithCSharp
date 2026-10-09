using System.Linq.Expressions;
using System.Reflection;
using Xunit;

namespace dev.kaldiroglu.Visitor.Tests;

/// <summary>
/// The first row of the deck's .NET known-uses table, checked against the running runtime.
/// The Java KnownUsesTest checks JDK types (FileVisitor, ElementVisitor, TreeVisitor and the
/// class-file API), which have no place in a .NET test. Roslyn's CSharpSyntaxWalker, the
/// table's second row, is left out because it needs an extra package.
/// </summary>
public class KnownUsesTests
{
    /// <summary>Doubles every integer constant in an expression tree.</summary>
    private sealed class DoubleConstants : ExpressionVisitor
    {
        protected override Expression VisitConstant(ConstantExpression node) =>
            node.Value is int value ? Expression.Constant(value * 2) : node;
    }

    [Fact]
    public void ExpressionVisitorHasAVisitMethodPerNodeKind()
    {
        var names = typeof(ExpressionVisitor)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Select(m => m.Name)
            .ToList();

        Assert.Contains("Visit", names);
        Assert.Contains("VisitBinary", names);
        Assert.Contains("VisitConstant", names);
        Assert.Contains("VisitParameter", names);
        Assert.Contains("VisitLambda", names);
    }

    [Fact]
    public void ExpressionVisitorVisitsAndRewritesATree()
    {
        Expression<Func<int, int>> plusOneTimesTwo = x => (x + 1) * 2;

        var rewritten = (Expression<Func<int, int>>)new DoubleConstants().Visit(plusOneTimesTwo);

        // (x + 2) * 4 for x = 3 is 20; the original tree is not changed and still gives 8.
        Assert.Equal(20, rewritten.Compile()(3));
        Assert.Equal(8, plusOneTimesTwo.Compile()(3));
    }
}
