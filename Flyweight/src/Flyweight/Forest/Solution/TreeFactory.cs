namespace dev.kaldiroglu.Flyweight.Forest.Solution;

/// <summary>
/// FlyweightFactory. Hands back the existing <see cref="TreeType"/> for a kind of tree, or
/// creates it the first time it is asked. The key names the intrinsic state completely, name
/// and color. The texture is loaded here, from the kind, and callers do not supply one: a
/// caller's texture would be ignored whenever the type already existed.
/// </summary>
public class TreeFactory
{
    private readonly Dictionary<string, TreeType> _types = new();
    private readonly TextureLoader _loader;
    private int _requests;

    public TreeFactory(TextureLoader loader)
    {
        _loader = loader;
    }

    /// <summary>Returns the shared type for this kind of tree, loading its texture at most once.</summary>
    public TreeType GetType(string name, string color)
    {
        _requests++;
        string key = name + "|" + color;
        if (!_types.TryGetValue(key, out TreeType? type))
        {
            type = new TreeType(name, color, _loader.Load(name));
            _types[key] = type;
        }

        return type;
    }

    public int DistinctTypes() => _types.Count;

    public int RequestCount() => _requests;

    /// <summary>Total texture bytes the whole forest holds, however many trees there are.</summary>
    public long TextureBytes() => _types.Values.Sum(type => (long)type.TextureBytes());
}
