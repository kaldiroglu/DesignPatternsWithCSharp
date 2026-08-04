using System.Globalization;

namespace dev.kaldiroglu.Composite.Hw.Expression;

/// <summary>A Leaf: a literal value. It evaluates to itself and has nothing below it.</summary>
public sealed class Number(double value) : IExpression
{
    public double Evaluate() => value;

    public string ToText() =>
        Math.Abs(value - Math.Round(value)) < double.Epsilon
            ? ((long)value).ToString(CultureInfo.InvariantCulture)
            : value.ToString(CultureInfo.InvariantCulture);

    public int NodeCount() => 1;
}
