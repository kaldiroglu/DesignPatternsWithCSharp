namespace dev.kaldiroglu.Flyweight;

/// <summary>
/// ConcreteFlyweight (GoF, p. 201).
///
/// GoF names this participant <c>Character</c>; we call it
/// <c>CharacterGlyph</c> to avoid clashing with <see cref="System.Char"/>.
///
/// Stores the only INTRINSIC state a character needs: its character code.
/// Because that state is independent of where the character appears, a single
/// <see cref="CharacterGlyph"/> instance can be shared by every occurrence of
/// that letter in the whole document.
///
/// The type is immutable and its constructor is <c>internal</c>: instances are
/// created and pooled only by <see cref="GlyphFactory"/>, which is what makes
/// sharing safe.
/// </summary>
public sealed class CharacterGlyph : Glyph
{
    private readonly char _charcode; // intrinsic, immutable -> safely shareable

    internal CharacterGlyph(char charcode) => _charcode = charcode;

    public char Charcode => _charcode;

    public override void Draw(Window window, GlyphContext context)
    {
        // Extrinsic state (the font) is fetched from the context, not stored here.
        Font font = context.GetFont();
        window.Draw(_charcode, font);
        context.Next(); // advance the context to the next position
    }
}
