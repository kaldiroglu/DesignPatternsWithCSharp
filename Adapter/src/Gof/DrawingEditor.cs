using dev.kaldiroglu.Adapter.Gof.ClassAdapter;

namespace dev.kaldiroglu.Adapter.Gof;

/// <summary>
/// <b>Client</b> participant (GoF p. 146).
///
/// <para>The drawing editor collaborates with everything through the <see cref="Shape"/> interface.
/// It never learns that some of those shapes are really <see cref="TextView"/>s underneath - that
/// knowledge is sealed inside the adapters. Notice the <c>for</c> loop treats adapted and native
/// shapes identically: that uniformity is the whole benefit of the Adapter pattern.</para>
/// </summary>
public sealed class DrawingEditor
{
    private DrawingEditor()
    {
    }

    public static void Run()
    {
        Shape[] drawing = new Shape[]
        {
            new LineShape(new Point(0, 0), new Point(40, 30)),
            // Object adapter: wraps an existing TextView instance.
            new TextShape(
                new TextView(new Point(10, 10), 100, 20, "Hello, Adapter")),
            // Object adapter around empty text (IsEmpty delegates to the adaptee).
            new TextShape(
                new TextView(new Point(5, 5), 0, 0, "")),
            // Class adapter: is-a TextView, behaves-as Shape.
            new dev.kaldiroglu.Adapter.Gof.ClassAdapter.TextShape(new Point(50, 50), 80, 16, "Class adapter")
        };

        foreach (Shape shape in drawing)
        {
            Console.WriteLine(shape);
            Console.WriteLine("  boundingBox = " + shape.BoundingBox());
            Console.WriteLine("  isEmpty     = " + shape.IsEmpty());
            shape.CreateManipulator().Manipulate();
        }
    }
}
