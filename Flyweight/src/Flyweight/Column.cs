namespace dev.kaldiroglu.Flyweight;

/// <summary>
/// UnsharedConcreteFlyweight (GoF, p. 198).
///
/// A <see cref="Column"/> stacks child glyphs (typically <see cref="Row"/>s)
/// top-to-bottom. Like <see cref="Row"/> it owns its children and is therefore
/// not shared.
/// </summary>
public class Column : Glyph
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
    }
}
