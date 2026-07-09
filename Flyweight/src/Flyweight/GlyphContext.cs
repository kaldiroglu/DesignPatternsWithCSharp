namespace dev.kaldiroglu.Flyweight;

/// <summary>
/// Holds the EXTRINSIC state of the flyweights (GoF, p. 202).
///
/// In the book this maps each glyph's position to its font using a compact
/// <em>BTree</em>. Here we use the simpler substitute the authors mention: a
/// plain <c>index -&gt; Font</c> dictionary. The client keeps this object and
/// passes it into <see cref="Glyph.Draw"/>, so the shared
/// <see cref="CharacterGlyph"/> flyweights never store a font themselves.
/// </summary>
public sealed class GlyphContext
{
    private int _index;
    private readonly Dictionary<int, Font> _fonts = new();
    private readonly Font _defaultFont;

    public GlyphContext(Font defaultFont) => _defaultFont = defaultFont;

    /// <summary>Advances the position cursor by <paramref name="step"/> glyphs (1 by default).</summary>
    public void Next(int step = 1) => _index += step;

    /// <summary>
    /// Assigns <paramref name="font"/> to the next <paramref name="span"/>
    /// positions starting at the current index. Simplified stand-in for GoF's
    /// BTree range insertion.
    /// </summary>
    public void SetFont(Font font, int span)
    {
        for (int i = 0; i < span; i++)
        {
            _fonts[_index + i] = font;
        }
    }

    /// <summary>Returns the font active at the current position.</summary>
    public Font GetFont() => _fonts.TryGetValue(_index, out Font? f) ? f : _defaultFont;

    /// <summary>Rewinds to the start of the document before traversing / drawing.</summary>
    public void Reset() => _index = 0;
}
