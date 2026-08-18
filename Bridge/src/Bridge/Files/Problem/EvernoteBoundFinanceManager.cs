namespace dev.kaldiroglu.Bridge.Files.Problem;

/// <summary>
/// The department, welded to a vendor by a base class.
/// <para>
/// It is an <see cref="EvernoteStore"/>. Not "has a store" — it is one. That single word decides
/// everything that follows. The store is fixed when the class is compiled: when the Evernote
/// contract ends and the documents move to SharePoint, this object cannot follow them, so a new
/// class has to be written and every caller that named this one has to be found. The retention
/// rule is trapped inside an Evernote class, and finance on SharePoint needs the same rule and
/// cannot reach it. And a second store is not one new class — it is one <i>per department</i>.
/// </para>
/// <para>
/// There is no <c>SetStore</c> here and there cannot be. A base class is chosen once, by the
/// compiler, and never again — which is the requirement the whole example turns on.
/// </para>
/// </summary>
public sealed class EvernoteBoundFinanceManager : EvernoteStore
{
    private const int Keep = 5;

    public EvernoteBoundFinanceManager(VendorStores stores) : base(stores, "finance")
    {
    }

    public string Save(string path, string content)
    {
        var guid = Put(path, content);
        KeepOnly(path, Keep);
        return guid;
    }
}
