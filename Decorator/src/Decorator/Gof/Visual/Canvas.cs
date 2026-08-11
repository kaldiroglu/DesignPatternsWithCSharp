namespace dev.kaldiroglu.Decorator.Gof.Visual;

/// <summary>
/// A fixed grid of characters that visual components draw themselves onto.
/// <para>
/// The GoF example talks about drawing on a screen. A character grid keeps the example
/// honest — a border really is drawn, and you can see where it lands — while staying
/// small enough to assert on in a unit test.
/// </para>
/// <para>
/// Deliberately shared by the Problem and Solution namespaces so that the two designs are
/// compared on identical output.
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

    /// <summary>Writes one character, ignoring anything that falls outside the grid.</summary>
    public void Put(int x, int y, char c)
    {
        if (y >= 0 && y < Height && x >= 0 && x < Width)
        {
            _cells[y][x] = c;
        }
    }

    /// <summary>Writes a string left to right starting at (x, y).</summary>
    public void Text(int x, int y, string s)
    {
        for (var i = 0; i < s.Length; i++)
        {
            Put(x + i, y, s[i]);
        }
    }

    /// <summary>Draws the outline of a rectangle whose top-left corner is (x, y).</summary>
    public void Rectangle(int x, int y, int width, int height)
    {
        for (var i = 1; i < width - 1; i++)
        {
            Put(x + i, y, '-');
            Put(x + i, y + height - 1, '-');
        }

        for (var i = 1; i < height - 1; i++)
        {
            Put(x, y + i, '|');
            Put(x + width - 1, y + i, '|');
        }

        Put(x, y, '+');
        Put(x + width - 1, y, '+');
        Put(x, y + height - 1, '+');
        Put(x + width - 1, y + height - 1, '+');
    }

    /// <summary>The grid as text, one line per row, without a trailing newline.</summary>
    public override string ToString() =>
        string.Join("\n", _cells.Select(row => new string(row)));
}
