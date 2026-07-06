namespace dev.kaldiroglu.Adapter.Gof;

/// <summary>
/// A <i>native</i> <see cref="Shape"/> - not an adapter. It exists so the demo shows the editor
/// treating adapted text and ordinary shapes <b>uniformly</b>, which is the payoff of the pattern.
///
/// <para>A line is defined by two endpoints.</para>
/// </summary>
public class LineShape : Shape
{
    private readonly Point start;
    private readonly Point end;

    public LineShape(Point start, Point end)
    {
        this.start = start;
        this.end = end;
    }

    public BoundingBox BoundingBox()
    {
        double left = Math.Min(start.X, end.X);
        double bottom = Math.Min(start.Y, end.Y);
        double right = Math.Max(start.X, end.X);
        double top = Math.Max(start.Y, end.Y);
        return new BoundingBox(new Point(left, bottom), new Point(right, top));
    }

    public Manipulator CreateManipulator()
    {
        return new LineManipulator(this);
    }

    public bool IsEmpty()
    {
        return false;
    }

    public override string ToString()
    {
        return "LineShape " + start + "-" + end;
    }
}
