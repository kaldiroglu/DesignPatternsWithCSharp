namespace dev.kaldiroglu.Bridge.Files.Problem;

/// <summary>
/// Naive design 2: one class per (department, store) pair.
/// <para>
/// Three of the six are written out here. The name has to state both axes, because the class is
/// both — and that is the smell: a class name carrying two ideas is hiding a field.
/// </para>
/// <para>
/// Compare with <see cref="InsuranceEvernoteManager"/>: the Evernote calls are identical and the
/// retention number differs. Compare with <see cref="FinanceSharePointManager"/>: the retention
/// number is identical and the vendor calls differ. Neither axis can be edited alone.
/// </para>
/// </summary>
public sealed class FinanceEvernoteManager
{
    private const int Keep = 5;
    private const string Notebook = "finance";

    private readonly VendorStores _stores;

    public FinanceEvernoteManager(VendorStores stores) => _stores = stores;

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
