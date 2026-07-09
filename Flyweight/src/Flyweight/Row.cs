namespace dev.kaldiroglu.Flyweight;

/// <summary>
/// UnsharedConcreteFlyweight (GoF, p. 198).
///
/// A <see cref="Row"/> groups child glyphs laid out left-to-right. Rows are NOT
/// shared: each one owns its list of children. It still implements the
/// <see cref="Glyph"/> interface, which is what lets a shared
/// <see cref="CharacterGlyph"/> sit inside an unshared Row.
/// </summary>
public class Row : Glyph
{
    private readonly List<Glyph> _children = new();

    public override void Insert(Glyph glyph) => _children.Add(glyph);

    public override Glyph? Child(int index) => _children[index];

    public override int ChildCount => _children.Count;

    public override void Draw(Window window, GlyphContext context)
    {
        foreach (Glyph child in _children)
        {
            child.Draw(window, context);
        }
        window.NewLine();
    }
}
