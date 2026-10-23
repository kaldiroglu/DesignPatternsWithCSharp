namespace dev.kaldiroglu.Adapter.Gof;

/// <summary>
/// The rectangle returned by <see cref="Shape.BoundingBox"/>.
///
/// <para>GoF returns the box through two out-parameters (<c>bottomLeft</c>, <c>topRight</c>);
/// Here the method returns an immutable value object instead.</para>
/// </summary>
public record BoundingBox(Point BottomLeft, Point TopRight)
{
    public override string ToString()
    {
        return "BoundingBox[" + BottomLeft + " -> " + TopRight + "]";
    }
}
