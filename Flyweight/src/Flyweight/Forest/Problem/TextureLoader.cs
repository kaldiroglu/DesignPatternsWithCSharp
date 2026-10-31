namespace dev.kaldiroglu.Flyweight.Forest.Problem;

/// <summary>
/// Stands in for whatever reads a texture off disk, and counts how often it happens. The
/// count is what a test can assert: ten thousand loads for two kinds of tree.
/// </summary>
public static class TextureLoader
{
    private static int _loads;
    private static long _bytes;

    /// <summary>Copies the source bytes, as a real loader reading the file again would.</summary>
    public static byte[] Load(byte[] source)
    {
        _loads++;
        _bytes += source.Length;
        return (byte[])source.Clone();
    }

    public static int LoadCount() => _loads;

    public static long BytesLoaded() => _bytes;

    public static void Reset()
    {
        _loads = 0;
        _bytes = 0;
    }
}
