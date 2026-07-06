namespace dev.kaldiroglu.Adapter.Gof;

/// <summary>
/// <b>Target</b> participant (GoF p. 141 / Sample Code p. 146).
///
/// <para>This is the interface the drawing-editor <see cref="DrawingEditor"/> client knows how to
/// use. Every graphical object the editor manages - lines, polygons, text - is manipulated through
/// <c>Shape</c>, so the client never needs to know an object's concrete type.</para>
/// </summary>
public interface Shape
{
    /// <returns>the smallest rectangle that encloses this pluggable.</returns>
    BoundingBox BoundingBox();

    /// <returns>a manipulator that animates this pluggable in response to user input.</returns>
    Manipulator CreateManipulator();

    /// <returns><c>true</c> if the pluggable currently has no visible content.</returns>
    bool IsEmpty();
}
