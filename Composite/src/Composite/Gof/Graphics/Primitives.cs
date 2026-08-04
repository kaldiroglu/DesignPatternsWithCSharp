namespace dev.kaldiroglu.Composite.Gof.Graphics;

/// <summary>
/// Leaf role of the Composite pattern (GoF, p. 165) — a primitive graphic.
/// </summary>
/// <remarks>
/// A <c>Line</c> has no children. It implements <see cref="Draw"/> by doing the
/// actual work itself; it inherits the failing child operations from
/// <see cref="Graphic"/>.
/// </remarks>
public class Line(int length) : Graphic
{
    public int Length { get; } = length;

    public override void Draw(Point at) =>
        Console.WriteLine($"Line of length {Length} drawn at {at}");
}

/// <summary>
/// Leaf role of the Composite pattern (GoF, p. 165) — a primitive graphic.
/// </summary>
/// <remarks>
/// Like <see cref="Line"/>, a <c>Rectangle</c> is childless: it draws itself and
/// rejects every child operation it inherits from <see cref="Graphic"/>.
/// </remarks>
public class Rectangle(int width, int height) : Graphic
{
    public int Width { get; } = width;

    public int Height { get; } = height;

    public override void Draw(Point at) =>
        Console.WriteLine($"Rectangle {Width}x{Height} drawn at {at}");
}

/// <summary>
/// Leaf role of the Composite pattern (GoF, p. 165) — a run of text on the
/// canvas. Childless, like the other primitives.
/// </summary>
public class Text(string content) : Graphic
{
    public string Content { get; } = content;

    public override void Draw(Point at) =>
        Console.WriteLine($"Text \"{Content}\" drawn at {at}");
}
