namespace dev.kaldiroglu.Proxy.Gof;

/// <summary>
/// Bounding size (width x height) of a graphic. The document editor uses the
/// extent to lay out the page, which is why the proxy caches it.
/// </summary>
public readonly record struct Extent(int Width, int Height)
{
    public static readonly Extent Zero = new(0, 0);

    public override string ToString() => $"{Width}x{Height}";
}
