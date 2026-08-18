namespace dev.kaldiroglu.Bridge.Shape.Solution;

/// <summary>
/// A ConcreteImplementor whose device has <i>no</i> arc call.
/// <para>
/// This is GoF's Presentation Manager detail (p. 157) in a second domain: the device cannot do
/// what it was asked, so it builds the result out of the primitives it does have — here,
/// sixteen short line segments. The <see cref="Circle"/> that asked for the arc never finds
/// out, and must not have to.
/// </para>
/// </summary>
public class XWindowsDrawer : AbstractShapeDrawer
{
    /// <summary>How finely an arc is approximated when the device cannot draw one.</summary>
    private const int Segments = 16;

    public XWindowsDrawer() : this("XWindows")
    {
    }

    public XWindowsDrawer(string name) : base(name)
    {
    }

    public override void DrawLine(int x1, int y1, int x2, int y2) =>
        Record($"line ({x1},{y1}) -> ({x2},{y2})");

    public override void DrawArc(int centerX, int centerY, int radius, int startDegrees,
        int sweepDegrees)
    {
        var previousX = PointX(centerX, radius, startDegrees);
        var previousY = PointY(centerY, radius, startDegrees);
        for (var i = 1; i <= Segments; i++)
        {
            var angle = startDegrees + sweepDegrees * i / Segments;
            var x = PointX(centerX, radius, angle);
            var y = PointY(centerY, radius, angle);
            DrawLine(previousX, previousY, x, y);
            previousX = x;
            previousY = y;
        }
    }

    public override void Clear(int x, int y, int width, int height) =>
        Record($"clear {width}x{height} at ({x},{y})");

    private static int PointX(int centerX, int radius, int degrees) =>
        centerX + (int)Math.Round(radius * Math.Cos(double.DegreesToRadians(degrees)));

    private static int PointY(int centerY, int radius, int degrees) =>
        centerY + (int)Math.Round(radius * Math.Sin(double.DegreesToRadians(degrees)));
}
