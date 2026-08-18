namespace dev.kaldiroglu.Bridge.Files.Solution;

/// <summary>
/// A RefinedAbstraction: finance keeps seven years, which here is the last five versions.
/// <para>
/// The rule is written once and is correct on every store, present and future.
/// </para>
/// </summary>
public class FinanceFileManager : FileManager
{
    private const int Keep = 5;

    public FinanceFileManager(IFileProvider provider) : base(provider)
    {
    }

    public override int RetainedVersions => Keep;

    protected override void ApplyRetention(string handle)
    {
        var versions = Provider.Versions(handle);
        for (var i = 0; i < versions.Count - Keep; i++)
        {
            Provider.DeleteVersion(handle, versions[i]);
        }
    }
}
