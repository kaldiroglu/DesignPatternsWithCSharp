namespace dev.kaldiroglu.Visitor.Hw.Expression;

/// <summary>
/// The <b>Visitor</b>, generic in what it returns.
/// <para>
/// Homework 3 added <see cref="Neg"/>. That meant one new record and one new method here —
/// and then <see cref="Evaluator"/> and <see cref="Printer"/> did not compile until each had
/// <c>Visit(Neg)</c>. A new operation, <see cref="DepthCounter"/>, was one new class and no
/// other change. That is GoF's trade-off: new operations are easy, new element classes are
/// hard.
/// </para>
/// </summary>
public interface IExprVisitor<R>
{
    R Visit(Num num);

    R Visit(Add add);

    R Visit(Mul mul);

    R Visit(Neg neg);
}
