namespace dev.kaldiroglu.Flyweight;

/// <summary>
/// FlyweightFactory (GoF, p. 203).
///
/// Guarantees that <see cref="CharacterGlyph"/> flyweights are shared: a request
/// for a letter already in the pool returns the existing instance instead of
/// allocating a new one. <see cref="Row"/>s and <see cref="Column"/>s are
/// unshared, so the factory simply creates a fresh one each time.
/// </summary>
public sealed class GlyphFactory
{
    private readonly Dictionary<char, CharacterGlyph> _characters = new();

    /// <summary>Returns the shared flyweight for <paramref name="c"/>, creating it on first use.</summary>
    public CharacterGlyph CreateCharacter(char c)
    {
        if (!_characters.TryGetValue(c, out CharacterGlyph? glyph))
        {
            glyph = new CharacterGlyph(c);
            _characters[c] = glyph;
        }
        return glyph;
    }

    public Row CreateRow() => new();

    public Column CreateColumn() => new();

    /// <summary>Number of distinct <see cref="CharacterGlyph"/> flyweights actually allocated.</summary>
    public int CreatedCharacterCount => _characters.Count;
}
