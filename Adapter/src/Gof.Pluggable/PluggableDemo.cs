using dev.kaldiroglu.Adapter.Gof;

namespace dev.kaldiroglu.Adapter.Gof.Pluggable;

/// <summary>
/// Runnable demo for both pluggable-adapter domains. In each, ONE adapter class serves TWO
/// differently-shaped adaptees, with the adaptation injected as lambdas (GoF technique (c)).
/// </summary>
public sealed class PluggableDemo
{
    private PluggableDemo()
    {
    }

    public static void Main(string[] args)
    {
        Console.WriteLine("== Domain A: drawing editor (one PluggableShapeAdapter, two adaptees) ==");

        TextView text = new TextView(new Point(10, 10), 100, 20, "Pluggable");
        Shape adaptedText = new PluggableShapeAdapter(
            "PluggableShape[" + text + "]",
            () =>
            {
                Point origin = text.GetOrigin();
                Point extent = text.GetExtent();
                return new BoundingBox(origin,
                    new Point(origin.X + extent.X, origin.Y + extent.Y));
            },
            text.IsEmpty);

        Circle circle = new Circle(new Point(50, 50), 30);
        Shape adaptedCircle = new PluggableShapeAdapter(
            "PluggableShape[" + circle + "]",
            () => new BoundingBox(
                new Point(circle.Center().X - circle.Radius(), circle.Center().Y - circle.Radius()),
                new Point(circle.Center().X + circle.Radius(), circle.Center().Y + circle.Radius())),
            () => false);

        foreach (Shape shape in new Shape[] { adaptedText, adaptedCircle })
        {
            Console.WriteLine(shape);
            Console.WriteLine("  boundingBox = " + shape.BoundingBox());
            Console.WriteLine("  isEmpty     = " + shape.IsEmpty());
            shape.CreateManipulator().Manipulate();
        }
    }
}
