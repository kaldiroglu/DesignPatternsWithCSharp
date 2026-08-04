using System.Globalization;

namespace dev.kaldiroglu.Composite.FileSystem;

/// <summary>
/// A <b>Client</b>, and the argument for the pattern in one class.
/// </summary>
/// <remarks>
/// <para>
/// It answers five questions about a tree of unknown shape and depth, and it names exactly
/// one type: <see cref="IStorage"/>. There is no type test, no <c>IsDirectory()</c>, and no
/// loop that walks anything — every method here is one call and one line.
/// </para>
/// <para>
/// Hand it a single file and every answer is still correct. That is the whole claim.
/// </para>
/// </remarks>
public class DiskReport(IStorage root)
{
    public long TotalBytes() => root.Size();

    public int Elements() => root.Count();

    public DateTimeOffset Newest() => root.LastModified();

    public string Biggest() => root.Largest()?.GetName() ?? "nothing";

    /// <summary>Everything over a threshold, wherever it lives in the tree.</summary>
    public IReadOnlyList<IStorage> Over(long bytes) => root.FindAll(element => element.Size() > bytes);

    public string Summary() =>
        $"{root.GetName()}{Environment.NewLine}" +
        $"  {TotalBytes().ToString("N0", CultureInfo.InvariantCulture)} bytes in {Elements()} elements{Environment.NewLine}" +
        $"  newest    : {Newest():yyyy-MM-ddTHH:mm:ssZ}{Environment.NewLine}" +
        $"  biggest   : {Biggest()}{Environment.NewLine}";
}
