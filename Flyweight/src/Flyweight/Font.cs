namespace dev.kaldiroglu.Flyweight;

/// <summary>
/// A simple value object describing a font.
///
/// In Lexi the font is part of the EXTRINSIC state held by a
/// <see cref="GlyphContext"/> (GoF, p. 202), never by the
/// <see cref="CharacterGlyph"/> flyweight. That is why the same shared Character can
/// be rendered in many different fonts.
/// </summary>
public sealed record Font(string Name)
{
    public override string ToString() => Name;
}
