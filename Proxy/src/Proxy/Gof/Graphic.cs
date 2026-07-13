namespace dev.kaldiroglu.Proxy.Gof;

/// <summary>
/// Subject role of the Proxy pattern (GoF, <i>Design Patterns</i>, p. 207).
/// <para>
/// A graphical element that can appear in a document. Both the real
/// <see cref="Image"/> and its stand-in <see cref="ImageProxy"/> implement this
/// interface, so the document editor works with them interchangeably and cannot
/// tell one from the other.
/// </para>
/// </summary>
public interface IGraphic
{
    /// <summary>Renders the graphic at the given position.</summary>
    void Draw(Point at);

    /// <summary>Returns the bounding size, used by the editor for page layout.</summary>
    Extent GetExtent();

    /// <summary>Persists the graphic to storage.</summary>
    void Store();

    /// <summary>Restores the graphic from storage.</summary>
    void Load();
}
