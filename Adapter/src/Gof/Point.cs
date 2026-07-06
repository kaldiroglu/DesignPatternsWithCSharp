namespace dev.kaldiroglu.Adapter.Gof;

/// <summary>
/// A coordinate pair in the drawing space.
///
/// <para>GoF (p. 146) uses a <c>Coord</c> type and a <c>Point</c> struct; we model both
/// as a single immutable value object.</para>
/// </summary>
public record Point(double X, double Y)
{
    public override string ToString()
    {
        return string.Format("({0:F1}, {1:F1})", X, Y);
    }
}
