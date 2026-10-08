namespace dev.kaldiroglu.TemplateMethod.Export.Domain;

/// <summary>
/// The record the auditors read. Every export must add one line here, after the file is
/// made. This is the promise of the story.
/// </summary>
public sealed class AuditLog
{
    private readonly List<string> lines = [];

    public void Record(User user, Export export, int rows)
    {
        lines.Add(user.Name + " exported " + export.FileName + " (" + rows + " rows)");
    }

    public IReadOnlyList<string> Lines => lines.ToList().AsReadOnly();
}
