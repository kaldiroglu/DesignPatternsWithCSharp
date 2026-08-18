using System.Text;

namespace dev.kaldiroglu.Bridge.Files.Problem;

/// <summary>
/// The three document stores, as their SDKs were handed to us.
/// <para>
/// This is the part nobody gets to redesign, and it is the fixed point every design in this
/// namespace ends at — the same role <c>Notifications.Domain.Transports</c> plays for the
/// notification example.
/// </para>
/// <para>
/// Read the three groups of methods together. No two vendors agree on anything.
/// <b>What an address is.</b> Evernote wants a notebook and a title, SharePoint a
/// site-relative URL, FileNet an object store and a document id.
/// <b>What a version is.</b> SharePoint numbers them from 1. Evernote and FileNet issue opaque
/// identifiers, and the two are not the same shape as each other.
/// <b>What "store this" is called.</b> Create a note, upload a file, check a document in.
/// </para>
/// <para>
/// The stores are simulated in memory so the example runs anywhere and so a test can assert
/// what was actually kept.
/// </para>
/// </summary>
public sealed class VendorStores
{
    /// <summary>Version identifiers per "vendor|address", oldest first.</summary>
    private readonly Dictionary<string, List<string>> _versions = new();

    /// <summary>Content per "vendor|address|version".</summary>
    private readonly Dictionary<string, string> _contents = new();

    // ------------------------------------------------------------ Evernote

    /// <summary>Evernote's API: a note in a notebook, addressed by title. Returns a note GUID.</summary>
    public string EvernoteCreateNote(string notebook, string title, string body)
    {
        var address = $"{notebook}/{title}";
        var guid = $"note-{Count("Evernote", address) + 1}";
        StoreVersion("Evernote", address, guid, body);
        return guid;
    }

    public IReadOnlyList<string> EvernoteNoteVersions(string notebook, string title) =>
        VersionsOf("Evernote", $"{notebook}/{title}").ToList();

    public void EvernoteExpunge(string notebook, string title, string guid) =>
        Remove("Evernote", $"{notebook}/{title}", guid);

    // ---------------------------------------------------------- SharePoint

    /// <summary>SharePoint's API: bytes to a site-relative URL. Versions are numbered from 1.</summary>
    public int SharePointUpload(string siteRelativeUrl, byte[] content)
    {
        var version = Count("SharePoint", siteRelativeUrl) + 1;
        StoreVersion("SharePoint", siteRelativeUrl, version.ToString(),
            Encoding.UTF8.GetString(content));
        return version;
    }

    public IReadOnlyList<int> SharePointVersionHistory(string siteRelativeUrl) =>
        VersionsOf("SharePoint", siteRelativeUrl).Select(int.Parse).ToList();

    public void SharePointDeleteVersion(string siteRelativeUrl, int version) =>
        Remove("SharePoint", siteRelativeUrl, version.ToString());

    // ------------------------------------------------------------- FileNet

    /// <summary>FileNet's API: check a document in to an object store. Returns a version series id.</summary>
    public string FileNetCheckin(string objectStore, string documentId, byte[] content)
    {
        var address = $"{objectStore}!{documentId}";
        var seriesId = $"vs-{Count("FileNet", address) + 1}";
        StoreVersion("FileNet", address, seriesId, Encoding.UTF8.GetString(content));
        return seriesId;
    }

    public IReadOnlyList<string> FileNetVersionSeries(string objectStore, string documentId) =>
        VersionsOf("FileNet", $"{objectStore}!{documentId}").ToList();

    public void FileNetDelete(string objectStore, string documentId, string seriesId) =>
        Remove("FileNet", $"{objectStore}!{documentId}", seriesId);

    // -------------------------------------------------- what a test can see

    /// <summary>How many versions this vendor is currently holding at this address.</summary>
    public int VersionsHeld(string vendor, string address) => Count(vendor, address);

    /// <summary>The most recently stored content at this address, whichever vendor holds it.</summary>
    public string LatestContent(string vendor, string address)
    {
        var held = VersionsOf(vendor, address);
        if (held.Count == 0)
        {
            throw new InvalidOperationException($"nothing stored at {vendor}|{address}");
        }

        return _contents[Key(vendor, address, held[^1])];
    }

    // ------------------------------------------------------------- private

    private void StoreVersion(string vendor, string address, string version, string content)
    {
        VersionsOf(vendor, address).Add(version);
        _contents[Key(vendor, address, version)] = content;
    }

    private void Remove(string vendor, string address, string version)
    {
        VersionsOf(vendor, address).Remove(version);
        _contents.Remove(Key(vendor, address, version));
    }

    private List<string> VersionsOf(string vendor, string address)
    {
        var key = $"{vendor}|{address}";
        if (!_versions.TryGetValue(key, out var list))
        {
            list = [];
            _versions[key] = list;
        }

        return list;
    }

    private int Count(string vendor, string address) => VersionsOf(vendor, address).Count;

    private static string Key(string vendor, string address, string version) =>
        $"{vendor}|{address}|{version}";
}
