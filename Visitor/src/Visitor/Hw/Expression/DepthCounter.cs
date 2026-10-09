namespace dev.kaldiroglu.Visitor.Hw.Expression;

/// <summary>A new operation: the depth of the tree. One new class; nothing else changed.</summary>
public sealed class DepthCounter : IExprVisitor<int>
{
    public int Visit(Num num) => 1;

    public int Visit(Add add) => 1 + Math.Max(add.Left.Accept(this), add.Right.Accept(this));

    public int Visit(Mul mul) => 1 + Math.Max(mul.Left.Accept(this), mul.Right.Accept(this));

    public int Visit(Neg neg) => 1 + neg.Operand.Accept(this);
}
