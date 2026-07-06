using dev.kaldiroglu.Adapter.Gof;

namespace dev.kaldiroglu.Adapter.Gof.Pluggable;

/// <summary>
/// <b>Pluggable adapter</b>, parameterized form - GoF technique (c), p. 143.
///
/// <para>A single adapter class turns <i>any</i> object into a <see cref="Shape"/>: instead of
/// writing a new adapter subclass per adaptee, the adaptation for the <b>narrow interface</b>
/// (<c>BoundingBox</c> + <c>IsEmpty</c>) is <b>injected as lambdas</b>. GoF's Smalltalk "blocks"
/// are C#'s delegates (<see cref="Func{TResult}"/>).</para>
///
/// <para>See <c>PluggableDemo</c> for the same class adapting two completely unrelated adaptees
/// (a <c>TextView</c> and a <see cref="Circle"/>) with no new subclasses.</para>
/// </summary>
public sealed class PluggableShapeAdapter : Shape
{
    private readonly string description;
    private readonly Func<BoundingBox> boundingBoxFn;
    private readonly Func<bool> isEmptyFn;

    public PluggableShapeAdapter(string description,
                                 Func<BoundingBox> boundingBoxFn,
                                 Func<bool> isEmptyFn)
    {
        this.description = description;
        this.boundingBoxFn = boundingBoxFn;
        this.isEmptyFn = isEmptyFn;
    }

    public BoundingBox BoundingBox()
    {
        return boundingBoxFn();
    }

    public bool IsEmpty()
    {
        return isEmptyFn();
    }

    /// <summary>
    /// Manipulation is not the operation that varies between adaptees, so it is not part of the
    /// narrow interface. We reuse <see cref="TextManipulator"/>; <c>this</c> is available at call
    /// time, so there is no construction-time cycle.
    /// </summary>
    public Manipulator CreateManipulator()
    {
        return new TextManipulator(this);
    }

    public override string ToString()
    {
        return description;
    }
}
