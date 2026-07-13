namespace dev.kaldiroglu.Proxy.Gof;

/// <summary>
/// RealSubject role of the Proxy pattern (GoF, p. 207).
/// <para>
/// The actual raster image. Constructing it is <i>expensive</i> because the
/// whole pixel buffer has to be read from disk, so we want to defer creating it
/// until the image really must be drawn. That deferral is exactly what
/// <see cref="ImageProxy"/> provides.
/// </para>
/// </summary>
public class Image : IGraphic
{
    private readonly string _fileName;
    private Extent _extent;

    public Image(string fileName)
    {
        _fileName = fileName;
        Load(); // expensive: reads the whole raster from disk up front
    }

    public void Load()
    {
        Console.WriteLine($"[Image] Loading raster data from '{_fileName}' (expensive)...");
        // A real implementation would parse the image header and pixel data here;
        // the extent is discovered while reading the file.
        _extent = ReadExtentFromFile();
    }

    private static Extent ReadExtentFromFile() => new(800, 600);

    public void Draw(Point at)
    {
        Console.WriteLine($"[Image] Drawing '{_fileName}' {_extent} at {at}");
    }

    public Extent GetExtent() => _extent;

    public void Store()
    {
        Console.WriteLine($"[Image] Storing raster data of '{_fileName}'");
    }

    public string FileName => _fileName;
}
