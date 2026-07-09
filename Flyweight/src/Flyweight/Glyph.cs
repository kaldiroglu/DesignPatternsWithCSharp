namespace dev.kaldiroglu.Flyweight;

/// <summary>
/// Flyweight (GoF, p. 198; sample code p. 200).
///
/// Declares the interface through which flyweights receive and act on
/// <em>extrinsic</em> state. In the Lexi editor every visible element of a
/// document is a <see cref="Glyph"/>.
///
/// GoF folds the Composite pattern into the same hierarchy: a Glyph also
/// declares child-management operations so that <see cref="Row"/>s and
/// <see cref="Column"/>s can contain other glyphs. Leaf flyweights
/// (<see cref="CharacterGlyph"/>) simply ignore them.
/// </summary>
public abstract class Glyph
{
    /// <summary>
    /// Renders this glyph. The <paramref name="context"/> supplies the
    /// EXTRINSIC state (the font that applies at the current position), which
    /// is never stored inside the shared flyweight.
    /// </summary>
    public abstract void Draw(Window window, GlyphContext context);

    // --- Composite operations: default no-ops, overridden by Row / Column ---

    public virtual void Insert(Glyph glyph)
    {
        // no-op for leaves
    }

    public virtual Glyph? Child(int index) => null;

    public virtual int ChildCount => 0;
}
