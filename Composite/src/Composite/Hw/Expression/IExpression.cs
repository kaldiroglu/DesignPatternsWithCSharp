namespace dev.kaldiroglu.Composite.Hw.Expression;

/// <summary>
/// The Component: anything that can be evaluated.
/// </summary>
/// <remarks>
/// This example is worth doing because the tree <em>is</em> the data. In the org chart the
/// hierarchy models something that exists in the world; here the hierarchy <em>is</em> the
/// arithmetic, and the recursion in <see cref="Evaluate"/> is the evaluation.
/// </remarks>
public interface IExpression
{
    double Evaluate();

    /// <summary>Fully parenthesized, so the shape of the tree is visible in the text.</summary>
    string ToText();

    /// <summary>How many numbers and operations this expression is made of.</summary>
    int NodeCount();
}
