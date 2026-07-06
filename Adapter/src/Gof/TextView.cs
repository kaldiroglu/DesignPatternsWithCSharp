namespace dev.kaldiroglu.Adapter.Gof;

/// <summary>
/// <b>Adaptee</b> participant (GoF p. 146).
///
/// <para>An existing, useful class for displaying and editing text. It is perfectly good code
/// - we do not want to change it - but its interface is <i>incompatible</i> with
/// <see cref="Shape"/>: it speaks <c>GetOrigin</c>/<c>GetExtent</c>, not
/// <c>BoundingBox</c>/<c>CreateManipulator</c>.</para>
///
/// <para>The whole point of the pattern is to reuse this class <i>without modifying it</i>.</para>
/// </summary>
public class TextView
{
    private readonly Point origin;
    private readonly double width;
    private readonly double height;
    private readonly string text;

    public TextView(Point origin, double width, double height, string text)
    {
        this.origin = origin;
        this.width = width;
        this.height = height;
        this.text = text;
    }

    /// <returns>the bottom-left corner of the text block.</returns>
    public Point GetOrigin()
    {
        return origin;
    }

    /// <returns>the size of the text block as a <c>(width, height)</c> pair.</returns>
    public Point GetExtent()
    {
        return new Point(width, height);
    }

    public bool IsEmpty()
    {
        return text == null || text.Length == 0;
    }

    public override string ToString()
    {
        return "TextView[\"" + text + "\"]";
    }
}
