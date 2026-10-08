namespace dev.kaldiroglu.TemplateMethod.Gof.Problem;

/// <summary>
/// Before the pattern: each application writes the whole of opening a document.
/// <para>
/// GoF's framework has many applications — a drawing program, a spreadsheet — and opening a
/// document is the same five steps in all of them: check the file, create the document, add
/// it to the list, open it, read it. Here each application has its own copy. Compare
/// <see cref="SpreadsheetApplication"/>: the same steps, in the same order, with two of them
/// different.
/// </para>
/// </summary>
public sealed class DrawApplication
{
    private readonly List<string> documents = [];
    private readonly List<string> events = [];

    public void OpenDocument(string name)
    {
        if (!name.EndsWith(".draw", StringComparison.Ordinal))
        {
            return;
        }
        documents.Add(name);
        events.Add("open " + name);
        events.Add("read shapes from " + name);
    }

    public IReadOnlyList<string> Events => events.ToList().AsReadOnly();
}
