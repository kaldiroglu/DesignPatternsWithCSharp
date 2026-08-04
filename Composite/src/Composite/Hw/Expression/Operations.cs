namespace dev.kaldiroglu.Composite.Hw.Expression;

/// <summary>A ConcreteComposite.</summary>
public sealed class Add(IExpression left, IExpression right) : BinaryOperation(left, right)
{
    protected override string Symbol => "+";

    public override double Evaluate() => Left.Evaluate() + Right.Evaluate();
}

/// <summary>A ConcreteComposite.</summary>
public sealed class Subtract(IExpression left, IExpression right) : BinaryOperation(left, right)
{
    protected override string Symbol => "-";

    public override double Evaluate() => Left.Evaluate() - Right.Evaluate();
}

/// <summary>A ConcreteComposite.</summary>
public sealed class Multiply(IExpression left, IExpression right) : BinaryOperation(left, right)
{
    protected override string Symbol => "*";

    public override double Evaluate() => Left.Evaluate() * Right.Evaluate();
}

/// <summary>
/// A ConcreteComposite that can fail.
/// </summary>
/// <remarks>
/// Worth having: it forces the question of what a Composite operation does when one node
/// cannot answer. Throwing is the honest choice here — a wrong number is worse than no number.
/// </remarks>
public sealed class Divide(IExpression left, IExpression right) : BinaryOperation(left, right)
{
    protected override string Symbol => "/";

    public override double Evaluate()
    {
        var divisor = Right.Evaluate();
        if (divisor == 0)
        {
            throw new DivideByZeroException($"division by zero in {ToText()}");
        }

        return Left.Evaluate() / divisor;
    }
}
