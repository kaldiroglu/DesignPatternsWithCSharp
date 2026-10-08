namespace dev.kaldiroglu.TemplateMethod.Gof.Problem;

/// <summary>
/// Before the pattern, second copy. The spreadsheet also wants to remember the last file it
/// opened, so it adds a step — in its own copy of the algorithm, in a place it chose.
/// </summary>
public sealed class SpreadsheetApplication
{
    private readonly List<string> documents = [];
    private readonly List<string> events = [];

    public void OpenDocument(string name)
    {
        if (!name.EndsWith(".sheet", StringComparison.Ordinal))
        {
            return;
        }
        documents.Add(name);
        events.Add("remember " + name + " as the last file");
        events.Add("open " + name);
        events.Add("read cells from " + name);
    }

    public IReadOnlyList<string> Events => events.ToList().AsReadOnly();
}
