namespace dev.kaldiroglu.Bridge.Retrofit;

/// <summary>
/// The one bought next year, which is why this is a Bridge and not an Adapter.
/// <para>
/// A different vendor with different habits: it names sessions differently and returns rows in
/// its own shape. No reporting class knows it exists.
/// </para>
/// </summary>
public class PurchasedEngine : IVendorClient
{
    public string Name => "purchased";

    public string Open(string database) => $"conn[{database}]";

    public IReadOnlyList<string> Pull(string handle, string statement) =>
        new[] { $"{handle} >> {statement.ToLowerInvariant()}" };

    // This vendor pools sessions; releasing is a no-op.
    public void Release(string handle)
    {
    }
}
