namespace dev.kaldiroglu.TemplateMethod.Pattern;

/// <summary>
/// GoF's example in a short form. <see cref="OpenDocument"/> is the template method: it
/// checks the file with <see cref="CanOpenDocument"/>, creates the document with
/// <see cref="CreateDocument"/>, and adds it with <see cref="AddDocument"/>.
/// <para>
/// The Java methods here have package access and are not <c>final</c>, so a subclass in the
/// package could override any of them. The port keeps that: they are <c>internal</c>, and
/// <c>OpenDocument</c> and <c>AddDocument</c> are <c>virtual</c>. The <c>Gof</c> namespace
/// has the closed form, where the template method is not <c>virtual</c>.
/// </para>
/// </summary>
public abstract class Application
{
    private readonly List<Document> documents = [];

    internal virtual void OpenDocument(string fileName)
    {
        if (CanOpenDocument(fileName))
        {
            Document doc = CreateDocument(fileName);
            AddDocument(doc);
        }
    }

    internal abstract bool CanOpenDocument(string fileName);

    internal abstract Document CreateDocument(string fileName);

    internal virtual void AddDocument(Document doc)
    {
        documents.Add(doc);
    }
}
