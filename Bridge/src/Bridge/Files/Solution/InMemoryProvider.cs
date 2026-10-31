namespace dev.kaldiroglu.Bridge.Files.Solution;

/// <summary>
/// Shared behavior for the three providers in this namespace.
/// <para>
/// Each vendor really would call its own SDK here. They are simulated in memory so the example
/// runs anywhere and so a test can assert what was stored — the same reason
/// <c>Notifications.Domain.Transports</c> exists.
/// </para>
/// </summary>
public abstract class InMemoryProvider : IFileProvider
{
    private readonly Dictionary<string, SortedDictionary<int, byte[]>> _documents = new();
    private readonly Dictionary<string, int> _lastVersion = new();

    protected InMemoryProvider(string name) => Name = name;

    public string Name { get; }

    public string Open(string path)
    {
        var handle = $"{Name}:{path}";
        if (!_documents.ContainsKey(handle))
        {
            _documents[handle] = new SortedDictionary<int, byte[]>();
        }

        return handle;
    }

    public byte[] Read(string handle)
    {
        var stored = ContentsOf(handle);
        if (stored.Count == 0)
        {
            throw new InvalidOperationException($"nothing stored at {handle}");
        }

        return (byte[])stored.Values.Last().Clone();
    }

    public int Write(string handle, byte[] content)
    {
        var stored = ContentsOf(handle);
        var version = _lastVersion.GetValueOrDefault(handle) + 1;
        _lastVersion[handle] = version;
        stored[version] = (byte[])content.Clone();
        return version;
    }

    public IReadOnlyList<int> Versions(string handle) =>
        ContentsOf(handle).Keys.ToList();

    public void DeleteVersion(string handle, int version)
    {
        if (!ContentsOf(handle).Remove(version))
        {
            throw new ArgumentException($"no version {version} at {handle}");
        }
    }

    private SortedDictionary<int, byte[]> ContentsOf(string handle)
    {
        if (!_documents.TryGetValue(handle, out var stored))
        {
            throw new InvalidOperationException($"Open() first: {handle}");
        }

        return stored;
    }
}
