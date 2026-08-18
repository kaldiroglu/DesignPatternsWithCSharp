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
    private readonly Dictionary<string, List<byte[]>> _documents = new();

    protected InMemoryProvider(string name) => Name = name;

    public string Name { get; }

    public string Open(string path)
    {
        var handle = $"{Name}:{path}";
        if (!_documents.ContainsKey(handle))
        {
            _documents[handle] = [];
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

        return (byte[])stored[^1].Clone();
    }

    public int Write(string handle, byte[] content)
    {
        var stored = ContentsOf(handle);
        stored.Add((byte[])content.Clone());
        return stored.Count;
    }

    public IReadOnlyList<int> Versions(string handle) =>
        Enumerable.Range(1, ContentsOf(handle).Count).ToList();

    public void DeleteVersion(string handle, int version)
    {
        var stored = ContentsOf(handle);
        if (version < 1 || version > stored.Count)
        {
            throw new ArgumentException($"no version {version} at {handle}");
        }

        // Tombstoned, so later numbers do not shift.
        stored[version - 1] = [];
    }

    private List<byte[]> ContentsOf(string handle)
    {
        if (!_documents.TryGetValue(handle, out var stored))
        {
            throw new InvalidOperationException($"Open() first: {handle}");
        }

        return stored;
    }
}
