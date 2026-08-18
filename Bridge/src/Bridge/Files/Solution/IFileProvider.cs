namespace dev.kaldiroglu.Bridge.Files.Solution;

/// <summary>
/// The Implementor: the primitives a document store offers.
/// <para>
/// <b>Not an adapter.</b> The earlier names here were <c>FileProviderAdaptor</c>,
/// <c>EvernoteAdaptor</c> and so on, and in a course that also teaches Adapter that naming does
/// real damage: an adapter makes an existing, incompatible interface fit one you already have,
/// after the fact. This interface was designed up front, alongside <see cref="FileManager"/>, so
/// that the two hierarchies could vary independently. That is Bridge.
/// </para>
/// <para>
/// Note also what these methods are. Not <c>ReadFile</c>/<c>WriteFile</c>/<c>UpdateFile</c>
/// mirroring the manager one-for-one — that would be the same interface written twice, and a new
/// manager operation would force every provider to grow. These are storage primitives, and the
/// managers compose them into whatever their department needs.
/// </para>
/// </summary>
public interface IFileProvider
{
    string Name { get; }

    /// <summary>Opens a document and returns a handle, creating it if it does not exist.</summary>
    string Open(string path);

    byte[] Read(string handle);

    /// <summary>Stores content as a new version and returns that version's number.</summary>
    int Write(string handle, byte[] content);

    /// <summary>Version numbers held for this document, oldest first.</summary>
    IReadOnlyList<int> Versions(string handle);

    void DeleteVersion(string handle, int version);
}
