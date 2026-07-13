namespace dev.kaldiroglu.Proxy.Gof;

/// <summary>
/// Proxy role of the Proxy pattern (GoF, p. 207) — a <i>virtual proxy</i>.
/// <para>
/// A lightweight stand-in for an <see cref="Image"/>. It keeps the file name and
/// a cached <see cref="Extent"/> so the document editor can lay out the page
/// without paying the cost of loading the real image. The real
/// <see cref="Image"/> is created only the first time the proxy is asked to
/// <see cref="Draw(Point)"/>; afterwards every call is forwarded to it.
/// </para>
/// </summary>
public class ImageProxy : IGraphic
{
    private readonly string _fileName;
    private Extent _extent;    // cached, so GetExtent() needs no real Image
    private Image? _image;      // the RealSubject, created lazily
    private int _loadCount;     // how many times the real Image has been created

    public ImageProxy(string fileName, Extent extent)
    {
        _fileName = fileName;
        _extent = extent;
    }

    /// <summary>Lazily creates and returns the real image, loading it at most once.</summary>
    private Image RealImage()
    {
        if (_image is null)
        {
            _image = new Image(_fileName);
            _extent = _image.GetExtent(); // keep the cached extent in sync with reality
            _loadCount++;
        }

        return _image;
    }

    public void Draw(Point at)
    {
        RealImage().Draw(at); // forces the real image to load, then delegates
    }

    public Extent GetExtent()
    {
        // Answered from the cached extent — the real image is NOT loaded.
        return _extent;
    }

    public void Store()
    {
        // Only the file name and extent need to be persisted for the proxy.
        Console.WriteLine($"[ImageProxy] Storing reference to '{_fileName}' {_extent}");
    }

    public void Load()
    {
        Console.WriteLine($"[ImageProxy] Restoring reference to '{_fileName}' (image not loaded yet)");
    }

    // --- hooks so tests can observe the laziness ---

    /// <summary>True once the real <see cref="Image"/> has been created.</summary>
    public bool IsImageLoaded => _image is not null;

    /// <summary>How many times the real <see cref="Image"/> has been created (should be at most 1).</summary>
    public int LoadCount => _loadCount;
}
