namespace dev.kaldiroglu.Command.Gof;

/// <summary>
/// GoF's <c>Application</c>: the other <b>Receiver</b>, and the class that knows which
/// documents are open.
/// <para>
/// <c>OpenCommand</c> acts on this rather than on a document, because opening a document
/// means creating one and adding it here.
/// </para>
/// </summary>
public sealed class Application
{
    private readonly List<Document> _documents = [];

    public Clipboard Clipboard { get; } = new();

    public void Add(Document document) => _documents.Add(document);

    public IReadOnlyList<Document> Documents() => _documents.ToList().AsReadOnly();

    /// <summary>The document most recently opened: the one the user is looking at.</summary>
    public Document Current() => _documents[^1];
}
