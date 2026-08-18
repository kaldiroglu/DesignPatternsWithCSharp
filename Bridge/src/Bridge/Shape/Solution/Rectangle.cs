namespace dev.kaldiroglu.Bridge.Shape.Solution;

/// <summary>A RefinedAbstraction: a rectangle is four lines.</summary>
public class Rectangle : AbstractShape
{
    private readonly int _x;
    private readonly int _y;
    private readonly int _width;
    private readonly int _height;

    public Rectangle(string name, IShapeDrawer drawer, int x, int y, int width, int height)
        : base(name, drawer)
    {
        _x = x;
        _y = y;
        _width = width;
        _height = height;
    }

    public override void Draw()
    {
        Drawer.DrawLine(_x, _y, _x + _width, _y);
        Drawer.DrawLine(_x + _width, _y, _x + _width, _y + _height);
        Drawer.DrawLine(_x + _width, _y + _height, _x, _y + _height);
        Drawer.DrawLine(_x, _y + _height, _x, _y);
    }

    public override void Erase() => Drawer.Clear(_x, _y, _width, _height);
}
