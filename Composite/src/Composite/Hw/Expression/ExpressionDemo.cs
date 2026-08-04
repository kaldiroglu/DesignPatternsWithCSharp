namespace dev.kaldiroglu.Composite.Hw.Expression;

/// <summary>
/// Homework 3 — the expression tree: <c>(3 + 4) * (10 - 2) / 4</c>, built as objects and
/// evaluated by recursion.
/// </summary>
public static class ExpressionDemo
{
    public static void Run()
    {
        IExpression expression = new Divide(
            new Multiply(
                new Add(new Number(3), new Number(4)),
                new Subtract(new Number(10), new Number(2))),
            new Number(4));

        Console.WriteLine($"expression : {expression.ToText()}");
        Console.WriteLine($"value      : {expression.Evaluate()}");
        Console.WriteLine($"nodes      : {expression.NodeCount()}");

        Console.WriteLine();
        Console.WriteLine("A leaf and an operation are the same type to the client, so a");
        Console.WriteLine("sub-expression can be swapped for a number and nothing above it");
        Console.WriteLine("notices:");

        IExpression simplified = new Divide(new Number(56), new Number(4));
        Console.WriteLine($"  {simplified.ToText()} = {simplified.Evaluate()}");
        Console.WriteLine($"  same answer, 3 nodes instead of {expression.NodeCount()}");

        Console.WriteLine();
        Console.WriteLine("And a node that cannot answer says so, loudly:");
        try
        {
            new Divide(new Number(1), new Subtract(new Number(5), new Number(5))).Evaluate();
        }
        catch (DivideByZeroException e)
        {
            Console.WriteLine($"  {e.Message}");
        }
    }
}
