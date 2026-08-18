namespace dev.kaldiroglu.Bridge.Hw.StatementRun;

/// <summary>
/// The Abstraction: a document that can be rendered onto any <see cref="IMedium"/>.
/// <para>
/// It holds a medium and never asks which one it has. Three document kinds and three media are
/// six classes here, not nine — and a fourth medium is one class that no document knows about.
/// </para>
/// </summary>
public abstract class Document
{
    protected readonly IMedium Medium;

    protected Document(IMedium medium) =>
        Medium = medium ?? throw new ArgumentNullException(nameof(medium));

    /// <summary>Writes this document onto its medium and returns whatever the medium produced.</summary>
    public string Render()
    {
        Body();
        return Medium.Output();
    }

    /// <summary>The document's own content, expressed only in the medium's primitives.</summary>
    protected abstract void Body();
}
