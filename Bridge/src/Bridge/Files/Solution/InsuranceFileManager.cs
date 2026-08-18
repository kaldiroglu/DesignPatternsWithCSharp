namespace dev.kaldiroglu.Bridge.Files.Solution;

/// <summary>
/// A RefinedAbstraction: insurance keeps only the current version and the one before it.
/// <para>
/// Named to match <see cref="FinanceFileManager"/>; it was previously <c>InsuranceManager</c>,
/// which broke the symmetry of the name and made the two look like different kinds of thing.
/// </para>
/// </summary>
public class InsuranceFileManager : FileManager
{
    private const int Keep = 2;

    public InsuranceFileManager(IFileProvider provider) : base(provider)
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
