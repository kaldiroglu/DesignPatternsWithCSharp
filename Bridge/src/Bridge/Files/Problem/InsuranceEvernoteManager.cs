namespace dev.kaldiroglu.Bridge.Files.Problem;

/// <summary>
/// The same store as <see cref="FinanceEvernoteManager"/>, the other department.
/// <para>
/// Every Evernote call below is character-for-character what the finance class does. Only
/// <c>Keep</c> differs. Change how this vendor is addressed and both classes have to be edited;
/// there are four more like them.
/// </para>
/// </summary>
public sealed class InsuranceEvernoteManager
{
    private const int Keep = 2;
    private const string Notebook = "insurance";

    private readonly VendorStores _stores;

    public InsuranceEvernoteManager(VendorStores stores) => _stores = stores;

    public string Save(string path, string content)
    {
        var guid = _stores.EvernoteCreateNote(Notebook, path, content);
        var kept = _stores.EvernoteNoteVersions(Notebook, path);
        for (var i = 0; i < kept.Count - Keep; i++)
        {
            _stores.EvernoteExpunge(Notebook, path, kept[i]);
        }

        return guid;
    }
}
