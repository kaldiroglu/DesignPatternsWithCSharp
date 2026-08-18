using System.Text;

namespace dev.kaldiroglu.Bridge.Files.Problem;

/// <summary>
/// Naive design 1: one class, one method, and a switch on each axis.
/// <para>
/// This is what the code looks like after the second store arrives and before anybody has time
/// to think. It works, and for one department and one store it is the clearest thing in the
/// file.
/// </para>
/// <para>
/// <b>Every branch is a pair.</b> Departments x stores, written out by hand. Two and three is
/// six branches; a third department makes it nine.
/// </para>
/// <para>
/// <b>Both axes are frozen together.</b> Adding a store means editing every department's
/// branch; adding a department means editing every store's. No edit touches one axis alone.
/// </para>
/// <para>
/// <b>The rules leak.</b> Count the retention loops below. The rule is not owned by anything;
/// it is repeated wherever somebody remembered it — and in <c>SaveForInsurance</c>, on FileNet,
/// forgotten.
/// </para>
/// <para>
/// <b>The bug this class exists to show.</b> Insurance may keep two versions. On FileNet it
/// keeps all of them, for ever. Nothing throws, nothing is logged, and the vendor is perfectly
/// happy — the breach is only visible to an auditor, or to a test that counts what was kept.
/// </para>
/// </summary>
public sealed class SwitchingFileManager
{
    private readonly VendorStores _stores;

    public SwitchingFileManager(VendorStores stores) => _stores = stores;

    public string Save(Department department, Store store, string path, string content) =>
        department switch
        {
            Department.Finance => SaveForFinance(store, path, content),
            Department.Insurance => SaveForInsurance(store, path, content),
            _ => throw new ArgumentOutOfRangeException(nameof(department))
        };

    private string SaveForFinance(Store store, string path, string content)
    {
        const string notebook = "finance";
        switch (store)
        {
            case Store.Evernote:
            {
                var guid = _stores.EvernoteCreateNote(notebook, path, content);
                var kept = _stores.EvernoteNoteVersions(notebook, path);
                for (var i = 0; i < kept.Count - 5; i++)
                {
                    _stores.EvernoteExpunge(notebook, path, kept[i]);
                }

                return guid;
            }
            case Store.SharePoint:
            {
                var url = $"/sites/{notebook}/{path}";
                var version = _stores.SharePointUpload(url, Encoding.UTF8.GetBytes(content));
                var kept = _stores.SharePointVersionHistory(url);
                for (var i = 0; i < kept.Count - 5; i++)
                {
                    _stores.SharePointDeleteVersion(url, kept[i]);
                }

                return version.ToString();
            }
            case Store.FileNet:
            {
                var series = _stores.FileNetCheckin(notebook, path, Encoding.UTF8.GetBytes(content));
                var kept = _stores.FileNetVersionSeries(notebook, path);
                for (var i = 0; i < kept.Count - 5; i++)
                {
                    _stores.FileNetDelete(notebook, path, kept[i]);
                }

                return series;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(store));
        }
    }

    private string SaveForInsurance(Store store, string path, string content)
    {
        const string notebook = "insurance";
        switch (store)
        {
            case Store.Evernote:
            {
                var guid = _stores.EvernoteCreateNote(notebook, path, content);
                var kept = _stores.EvernoteNoteVersions(notebook, path);
                for (var i = 0; i < kept.Count - 2; i++)
                {
                    _stores.EvernoteExpunge(notebook, path, kept[i]);
                }

                return guid;
            }
            case Store.SharePoint:
            {
                var url = $"/sites/{notebook}/{path}";
                var version = _stores.SharePointUpload(url, Encoding.UTF8.GetBytes(content));
                var kept = _stores.SharePointVersionHistory(url);
                for (var i = 0; i < kept.Count - 2; i++)
                {
                    _stores.SharePointDeleteVersion(url, kept[i]);
                }

                return version.ToString();
            }
            case Store.FileNet:
                // The two-version rule is missing here. Nobody removed it; the person who added
                // FileNet copied the finance branch, which keeps five, and then deleted the
                // loop rather than change the number. Nothing throws.
                return _stores.FileNetCheckin(notebook, path, Encoding.UTF8.GetBytes(content));
            default:
                throw new ArgumentOutOfRangeException(nameof(store));
        }
    }
}
