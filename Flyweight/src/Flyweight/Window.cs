using System.Text;

namespace dev.kaldiroglu.Flyweight;

/// <summary>
/// The drawing surface (a stand-in for the on-screen window).
///
/// It tracks a simple (x, y) cursor as the extrinsic <em>position</em> is
/// realised during drawing, and records every rendered character together with
/// the font that was active at that point.
/// </summary>
public sealed class Window
{
    /// <summary>One rendered glyph: which character, in which font, at which position.</summary>
    public readonly record struct RenderedGlyph(char Charcode, Font Font, int X, int Y);

    private readonly List<RenderedGlyph> _rendered = new();
    private int _x;
    private int _y;

    public void Draw(char charcode, Font font)
    {
        _rendered.Add(new RenderedGlyph(charcode, font, _x, _y));
        _x++;
    }

    public void NewLine()
    {
        _x = 0;
        _y++;
    }

    public IReadOnlyList<RenderedGlyph> Rendered => _rendered;

    /// <summary>Reconstructs the plain text that was drawn, for display in the demo.</summary>
    public string Text()
    {
        var sb = new StringBuilder();
        int line = 0;
        foreach (RenderedGlyph r in _rendered)
        {
            while (line < r.Y)
            {
                sb.Append('\n');
                line++;
            }
            sb.Append(r.Charcode);
        }
        return sb.ToString();
    }
}
