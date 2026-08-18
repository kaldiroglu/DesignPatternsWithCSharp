namespace dev.kaldiroglu.Bridge.Gof;

/// <summary>
/// A fixed grid of characters that windows are drawn onto.
/// <para>
/// GoF's example is about a windowing system that must run on more than one platform. A
/// character grid is small enough to assert on in a unit test, and it lets each platform draw
/// with visibly different characters — so "the same window, drawn by a different
/// implementation" is something students can see rather than take on trust.
/// </para>
/// <para>
/// Shared by the <c>Problem</c> and <c>Solution</c> namespaces, so the two designs are compared
/// on identical output.
/// </para>
/// </summary>
public sealed class Canvas
{
    private readonly char[][] _cells;

    public Canvas(int width, int height)
    {
        if (width <= 0 || height <= 0)
        {
            throw new ArgumentException("canvas must have a positive size");
        }

        _cells = new char[height][];
        for (var y = 0; y < height; y++)
        {
            _cells[y] = new char[width];
            Array.Fill(_cells[y], ' ');
        }
    }

    public int Width => _cells[0].Length;

    public int Height => _cells.Length;

    public void Put(int x, int y, char c)
    {
        if (y >= 0 && y < Height && x >= 0 && x < Width)
        {
            _cells[y][x] = c;
        }
    }

    public void Text(int x, int y, string s)
    {
        for (var i = 0; i < s.Length; i++)
        {
            Put(x + i, y, s[i]);
        }
    }

    /// <summary>
    /// Draws a rectangle outline using the characters the caller's platform prefers.
    /// </summary>
    public void Rectangle(int x, int y, int width, int height,
        char corner, char horizontal, char vertical)
    {
        for (var i = 1; i < width - 1; i++)
        {
            Put(x + i, y, horizontal);
            Put(x + i, y + height - 1, horizontal);
        }

        for (var i = 1; i < height - 1; i++)
        {
            Put(x, y + i, vertical);
            Put(x + width - 1, y + i, vertical);
        }

        Put(x, y, corner);
        Put(x + width - 1, y, corner);
        Put(x, y + height - 1, corner);
        Put(x + width - 1, y + height - 1, corner);
    }

    public override string ToString() => string.Join("\n", _cells.Select(row => new string(row)));
}
