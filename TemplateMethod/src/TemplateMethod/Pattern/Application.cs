namespace dev.kaldiroglu.TemplateMethod.Pattern;

/// <summary>
/// GoF's example in a short form. <see cref="OpenDocument"/> is the template method: it
/// checks the file with <see cref="CanOpenDocument"/>, creates the document with
/// <see cref="CreateDocument"/>, and adds it with <see cref="AddDocument"/>.
/// <para>
/// The methods here are <c>internal</c>, and <c>OpenDocument</c> and <c>AddDocument</c> are
/// <c>virtual</c>, so a subclass in the assembly can override them. The <c>Gof</c> namespace
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
