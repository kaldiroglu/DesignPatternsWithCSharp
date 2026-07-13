namespace dev.kaldiroglu.Proxy.Gof;

/// <summary>
/// Client role of the Proxy pattern (GoF, p. 207) — a small stand-in for the
/// book's document editor.
/// <para>
/// A document is a sequence of <see cref="IGraphic"/>s. Laying out the page only
/// needs each graphic's <see cref="Extent"/>; drawing the page is what finally
/// forces the images to load. Everything happens through the
/// <see cref="IGraphic"/> interface, so the document never knows whether an
/// element is a real image or a proxy.
/// </para>
/// </summary>
public class TextDocument
{
    private readonly List<IGraphic> _graphics = new();

    public void Insert(IGraphic graphic)
    {
        _graphics.Add(graphic);
    }

    /// <summary>Stacks graphics vertically and returns the total page extent.</summary>
    public Extent LayoutExtent()
    {
        int width = 0;
        int height = 0;
        foreach (IGraphic g in _graphics)
        {
            Extent e = g.GetExtent();
            width = Math.Max(width, e.Width);
            height += e.Height;
        }

        return new Extent(width, height);
    }

    /// <summary>Draws every graphic, stacked from top to bottom starting at <paramref name="at"/>.</summary>
    public void Draw(Point at)
    {
        int y = at.Y;
        foreach (IGraphic g in _graphics)
        {
            g.Draw(new Point(at.X, y));
            y += g.GetExtent().Height;
        }
    }

    public int Size => _graphics.Count;
}
