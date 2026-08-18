namespace dev.kaldiroglu.Bridge.Shape.Solution;

/// <summary>
/// A ConcreteImplementor whose device draws arcs natively.
/// <para>
/// One call in, one call out. Compare with <see cref="XWindowsDrawer"/>, which has to build the
/// same arc out of line segments.
/// </para>
/// </summary>
public class MacOSDrawer : AbstractShapeDrawer
{
    public MacOSDrawer() : this("MacOS")
    {
    }

    public MacOSDrawer(string name) : base(name)
    {
    }

    public override void DrawLine(int x1, int y1, int x2, int y2) =>
        Record($"line ({x1},{y1}) -> ({x2},{y2})");

    public override void DrawArc(int centerX, int centerY, int radius, int startDegrees,
        int sweepDegrees) =>
        Record($"arc r={radius} from {startDegrees} sweep {sweepDegrees}");

    public override void Clear(int x, int y, int width, int height) =>
        Record($"clear {width}x{height} at ({x},{y})");
}
