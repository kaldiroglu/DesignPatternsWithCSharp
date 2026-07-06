using dev.kaldiroglu.Adapter.Gof;

namespace dev.kaldiroglu.Adapter.Gof.Pluggable;

/// <summary>
/// A second, <i>unrelated</i> adaptee with its own interface - it knows nothing about
/// <c>Shape</c>, <c>TextView</c>, or bounding boxes. It exists to prove that the single
/// <see cref="PluggableShapeAdapter"/> class can adapt adaptees that share no common base.
/// </summary>
public sealed class Circle
{
    private readonly Point center;
    private readonly double radius;

    public Circle(Point center, double radius)
    {
        this.center = center;
        this.radius = radius;
    }

    public Point Center()
    {
        return center;
    }

    public double Radius()
    {
        return radius;
    }

    public override string ToString()
    {
        return "Circle@" + center + " r=" + radius;
    }
}
