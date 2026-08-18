using System.Text;

namespace dev.kaldiroglu.Bridge.Files.Solution;

/// <summary>
/// The Abstraction: what a department does with its documents.
/// <para>
/// It holds an <see cref="IFileProvider"/> and never asks which one. The operations here are
/// composed from the provider's primitives, and each department's retention rule lives in a
/// subclass rather than in a provider — which is why adding a fourth store costs one class and
/// touches no rule.
/// </para>
/// <para>
/// The bridge reference is <c>protected</c> here rather than private, because the refinements
/// write their own retention loops against it. GoF's own <c>Window</c> keeps it private and
/// reaches it through an accessor; both are defensible, and the choice is about how much of the
/// implementor a refinement is trusted with.
/// </para>
/// </summary>
public abstract class FileManager
{
    protected IFileProvider Provider;

    protected FileManager(IFileProvider provider) =>
        Provider = provider ?? throw new ArgumentNullException(nameof(provider));

    /// <summary>
    /// Bridge, not Strategy: the store can be changed on a manager that already exists.
    /// </summary>
    public void SetProvider(IFileProvider provider) =>
        Provider = provider ?? throw new ArgumentNullException(nameof(provider));

    public string Read(string path) =>
        Encoding.UTF8.GetString(Provider.Read(Provider.Open(path)));

    /// <summary>Stores a new version and then applies this department's retention rule.</summary>
    public int Save(string path, string content)
    {
        var handle = Provider.Open(path);
        var version = Provider.Write(handle, Encoding.UTF8.GetBytes(content));
        ApplyRetention(handle);
        return version;
    }

    public IReadOnlyList<int> Versions(string path) => Provider.Versions(Provider.Open(path));

    /// <summary>How long this department is required to keep old versions.</summary>
    protected abstract void ApplyRetention(string handle);

    /// <summary>How many versions this department keeps, for the demo to print.</summary>
    public abstract int RetainedVersions { get; }
}
