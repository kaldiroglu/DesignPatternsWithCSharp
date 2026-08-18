namespace dev.kaldiroglu.Bridge.Shape.Solution;

/// <summary>
/// The shape that proves the point.
/// <para>
/// It was added after both drawers were written, and neither drawer changed. Under the old
/// interface — the one with <c>DrawCircle</c> and <c>DrawRectangle</c> on it — this class would
/// have forced a <c>DrawTriangle</c> into every device, which is the definition of two
/// hierarchies that are not independent.
/// </para>
/// </summary>
public class Triangle : AbstractShape
{
    private readonly int _x1, _y1, _x2, _y2, _x3, _y3;

    public Triangle(string name, IShapeDrawer drawer,
        int x1, int y1, int x2, int y2, int x3, int y3)
        : base(name, drawer)
    {
        _x1 = x1; _y1 = y1;
        _x2 = x2; _y2 = y2;
        _x3 = x3; _y3 = y3;
    }

    public override void Draw()
    {
        Drawer.DrawLine(_x1, _y1, _x2, _y2);
        Drawer.DrawLine(_x2, _y2, _x3, _y3);
        Drawer.DrawLine(_x3, _y3, _x1, _y1);
    }

    public override void Erase()
    {
        var minX = Math.Min(_x1, Math.Min(_x2, _x3));
        var minY = Math.Min(_y1, Math.Min(_y2, _y3));
        var maxX = Math.Max(_x1, Math.Max(_x2, _x3));
        var maxY = Math.Max(_y1, Math.Max(_y2, _y3));
        Drawer.Clear(minX, minY, maxX - minX, maxY - minY);
    }
}
