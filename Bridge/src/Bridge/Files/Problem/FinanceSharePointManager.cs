using System.Text;

namespace dev.kaldiroglu.Bridge.Files.Problem;

/// <summary>
/// The same department as <see cref="FinanceEvernoteManager"/>, the other store.
/// <para>
/// The retention rule below is the same rule, written a second time against a different
/// vendor's API. When finance moves from five years to ten, this class and a
/// <c>FinanceFileNetManager</c> have to be found and changed too — and the day one of them is
/// missed, nothing fails.
/// </para>
/// </summary>
public sealed class FinanceSharePointManager
{
    private const int Keep = 5;
    private const string Site = "/sites/finance/";

    private readonly VendorStores _stores;

    public FinanceSharePointManager(VendorStores stores) => _stores = stores;

    public string Save(string path, string content)
    {
        var url = Site + path;
        var version = _stores.SharePointUpload(url, Encoding.UTF8.GetBytes(content));
        var kept = _stores.SharePointVersionHistory(url);
        for (var i = 0; i < kept.Count - Keep; i++)
        {
            _stores.SharePointDeleteVersion(url, kept[i]);
        }

        return version.ToString();
    }
}
