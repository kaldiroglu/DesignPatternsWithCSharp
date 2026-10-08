namespace dev.kaldiroglu.TemplateMethod.Gof.Solution;

/// <summary>A <b>ConcreteClass</b> for documents: a spreadsheet reads cells.</summary>
public sealed class SpreadsheetDocument(string name) : Document(name)
{
    protected internal override void DoRead(List<string> events)
    {
        events.Add("read cells from " + Name);
    }
}
