namespace dev.kaldiroglu.Adapter.Gof;

/// <summary>
/// The rectangle returned by <see cref="Shape.BoundingBox"/>.
///
/// <para>GoF returns the box through two out-parameters (<c>bottomLeft</c>, <c>topRight</c>);
/// Java has no out-parameters, so we return an immutable value object instead.</para>
/// </summary>
public record BoundingBox(Point BottomLeft, Point TopRight)
{
    public override string ToString()
    {
        return "BoundingBox[" + BottomLeft + " -> " + TopRight + "]";
    }
}
