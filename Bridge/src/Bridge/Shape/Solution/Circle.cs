namespace dev.kaldiroglu.Bridge.Shape.Solution;

/// <summary>A RefinedAbstraction: a circle is one arc, all the way round.</summary>
public class Circle : AbstractShape
{
    private readonly int _centerX;
    private readonly int _centerY;
    private readonly int _radius;

    public Circle(string name, IShapeDrawer drawer, int centerX, int centerY, int radius)
        : base(name, drawer)
    {
        _centerX = centerX;
        _centerY = centerY;
        _radius = radius;
    }

    public override void Draw() => Drawer.DrawArc(_centerX, _centerY, _radius, 0, 360);

    public override void Erase() =>
        Drawer.Clear(_centerX - _radius, _centerY - _radius, _radius * 2, _radius * 2);
}
