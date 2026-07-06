using dev.kaldiroglu.Adapter.Gof;

namespace dev.kaldiroglu.Adapter.Gof.ClassAdapter;

/// <summary>
/// <b>Class Adapter</b> (GoF p. 146).
///
/// <para>GoF writes this in C++ using multiple inheritance: <c>class TextShape : public Shape,
/// private TextView</c>. Java has only single implementation inheritance, so the idiomatic class
/// adapter <b>extends the adaptee</b> (<see cref="TextView"/>) and <b>implements the target</b>
/// (<see cref="Shape"/>).</para>
///
/// <para>Trade-offs versus the <see cref="dev.kaldiroglu.Adapter.Gof.TextShape"/> object adapter:</para>
/// <list type="bullet">
///   <item>It adapts <c>TextView</c> itself, committing to a subclass relationship at compile time
///       (it cannot adapt a <c>TextView</c> subclass passed in at runtime).</item>
///   <item>In exchange it can override <c>TextView</c> behaviour directly and needs no wrapped
///       field.</item>
/// </list>
/// </summary>
public class TextShape : TextView, Shape
{
    public TextShape(Point origin, double width, double height, string text)
        : base(origin, width, height, text)
    {
    }

    public BoundingBox BoundingBox()
    {
        Point origin = GetOrigin();   // inherited from TextView
        Point extent = GetExtent();   // inherited from TextView
        Point topRight = new Point(origin.X + extent.X, origin.Y + extent.Y);
        return new BoundingBox(origin, topRight);
    }

    public Manipulator CreateManipulator()
    {
        return new TextManipulator(this);
    }

    // Note: IsEmpty() is inherited from TextView and already satisfies Shape.IsEmpty(),
    // so the class adapter does not need to declare it. That "free" method is exactly the
    // convenience a class adapter buys you.
}
