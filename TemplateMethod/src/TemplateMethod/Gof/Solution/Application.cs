namespace dev.kaldiroglu.TemplateMethod.Gof.Solution;

/// <summary>
/// The <b>AbstractClass</b>: GoF's <c>Application</c>.
/// <para>
/// <see cref="OpenDocument"/> is the <b>template method</b>. It fixes the order of the steps
/// and calls three kinds of operation:
/// </para>
/// <list type="bullet">
///   <item>a primitive operation that a subclass must write: <see cref="CanOpenDocument"/>;</item>
///   <item>a factory method that a subclass must write: <see cref="DoCreateDocument"/>;</item>
///   <item>a hook that a subclass may write: <see cref="AboutToOpenDocument"/>, which does
///   nothing here.</item>
/// </list>
/// <para>
/// In Java it is <c>final</c>. In C# it is a public method without <c>virtual</c>, which
/// says the same thing: an application can change a step but not the order of the steps.
/// </para>
/// </summary>
public abstract class Application
{
    private readonly List<Document> documents = [];
    private readonly List<string> events = [];

    /// <summary>The template method. Not <c>virtual</c>, so no subclass can override it.</summary>
    public void OpenDocument(string name)
    {
        if (!CanOpenDocument(name))
        {
            return;
        }
        Document? document = DoCreateDocument(name);
        if (document != null)
        {
            documents.Add(document);
            AboutToOpenDocument(document);
            document.Open();
            events.Add("open " + name);
            document.DoRead(events);
        }
    }

    /// <summary>A primitive operation: can this application open this file?</summary>
    protected abstract bool CanOpenDocument(string name);

    /// <summary>A factory method: which kind of document this application creates.</summary>
    protected abstract Document? DoCreateDocument(string name);

    /// <summary>A hook: called just before a document is opened. Does nothing by default.</summary>
    protected virtual void AboutToOpenDocument(Document document)
    {
    }

    protected void Record(string @event)
    {
        events.Add(@event);
    }

    public IReadOnlyList<Document> Documents => documents.ToList().AsReadOnly();

    public IReadOnlyList<string> Events => events.ToList().AsReadOnly();
}
