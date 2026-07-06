namespace dev.kaldiroglu.Adapter.Gof;

/// <summary>
/// <b>Object Adapter</b> (GoF p. 147).
///
/// <para>Adapts <see cref="TextView"/> to the <see cref="Shape"/> interface by <i>holding a
/// reference</i> to a <c>TextView</c> and forwarding/translating each call. This is the form GoF
/// recommends for single-inheritance languages: it uses <b>composition</b> rather than inheritance,
/// so a single adapter class can wrap <i>any</i> <c>TextView</c> instance (or subclass).</para>
///
/// <para>Contrast with <see cref="dev.kaldiroglu.Adapter.Gof.ClassAdapter.TextShape"/> the class
/// adapter, which inherits <c>TextView</c> instead.</para>
/// </summary>
public class TextShape : Shape
{
    /// <summary>The wrapped adaptee.</summary>
    private readonly TextView textView;

    public TextShape(TextView textView)
    {
        this.textView = textView;
    }

    /// <summary>
    /// Translates the adaptee's <c>GetOrigin</c>/<c>GetExtent</c> into the <c>BoundingBox</c>
    /// the <see cref="Shape"/> interface promises. This method <i>is</i> the adaptation.
    /// </summary>
    public BoundingBox BoundingBox()
    {
        Point origin = textView.GetOrigin();
        Point extent = textView.GetExtent();
        Point bottomLeft = origin;
        Point topRight = new Point(origin.X + extent.X, origin.Y + extent.Y);
        return new BoundingBox(bottomLeft, topRight);
    }

    public Manipulator CreateManipulator()
    {
        return new TextManipulator(this);
    }

    /// <summary>Delegates straight to the adaptee - the names happen to match.</summary>
    public bool IsEmpty()
    {
        return textView.IsEmpty();
    }

    public override string ToString()
    {
        return "TextShape wrapping " + textView;
    }
}
