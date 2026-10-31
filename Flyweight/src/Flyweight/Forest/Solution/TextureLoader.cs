namespace dev.kaldiroglu.Flyweight.Forest.Solution;

/// <summary>
/// Loads a texture for a kind of tree, and records every load so a test can count them. An
/// instance rather than a static, unlike the problem's version: once loading is the
/// factory's job, a test can hand it a different loader.
/// </summary>
public class TextureLoader
{
    private readonly int _size;
    private readonly List<string> _loaded = [];

    public TextureLoader(int size)
    {
        _size = size;
    }

    public byte[] Load(string kind)
    {
        _loaded.Add(kind);
        return new byte[_size];
    }

    public int LoadCount() => _loaded.Count;

    public IReadOnlyList<string> LoadedKinds() => _loaded.ToList();
}
