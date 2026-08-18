namespace dev.kaldiroglu.Bridge.Files.Problem;

/// <summary>
/// Naive design 3: put the vendor's details in a base class and inherit them.
/// <para>
/// This is a real improvement on the class-per-pair design, and it should be said out loud: the
/// Evernote calls are now written once instead of once per department, and a department that
/// extends this gets them for free.
/// </para>
/// <para>
/// What it costs is on <see cref="EvernoteBoundFinanceManager"/>.
/// </para>
/// </summary>
public abstract class EvernoteStore
{
    private readonly VendorStores _stores;
    private readonly string _notebook;

    protected EvernoteStore(VendorStores stores, string notebook)
    {
        _stores = stores;
        _notebook = notebook;
    }

    protected string Put(string path, string content) =>
        _stores.EvernoteCreateNote(_notebook, path, content);

    protected void KeepOnly(string path, int versions)
    {
        var kept = _stores.EvernoteNoteVersions(_notebook, path);
        for (var i = 0; i < kept.Count - versions; i++)
        {
            _stores.EvernoteExpunge(_notebook, path, kept[i]);
        }
    }
}
