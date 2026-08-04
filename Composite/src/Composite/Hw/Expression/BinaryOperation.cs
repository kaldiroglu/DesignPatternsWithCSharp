namespace dev.kaldiroglu.Composite.Hw.Expression;

/// <summary>
/// The Composite: an operation over two sub-expressions.
/// </summary>
/// <remarks>
/// Note that it holds exactly two children rather than a list. A Composite does not have to
/// hold an unbounded collection — it has to hold <em>components</em>, and a binary operator
/// holding two is as much a composite as a directory holding a hundred.
/// </remarks>
public abstract class BinaryOperation(IExpression left, IExpression right) : IExpression
{
    protected readonly IExpression Left =
        left ?? throw new ArgumentNullException(nameof(left));

    protected readonly IExpression Right =
        right ?? throw new ArgumentNullException(nameof(right));

    protected abstract string Symbol { get; }

    public abstract double Evaluate();

    public string ToText() => $"({Left.ToText()} {Symbol} {Right.ToText()})";

    public int NodeCount() => 1 + Left.NodeCount() + Right.NodeCount();
}
