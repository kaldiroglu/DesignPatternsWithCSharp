namespace dev.kaldiroglu.TemplateMethod.Gof.Solution;

/// <summary>
/// A <b>ConcreteClass</b> that also uses the hook. Compare <c>Problem.SpreadsheetApplication</c>,
/// which added the same step in its own copy of the algorithm. Here the step goes where the
/// template method allows it, and nowhere else.
/// </summary>
public sealed class SpreadsheetApplication : Application
{
    protected override bool CanOpenDocument(string name)
    {
        return name.EndsWith(".sheet", StringComparison.Ordinal);
    }

    protected override Document DoCreateDocument(string name)
    {
        return new SpreadsheetDocument(name);
    }

    protected override void AboutToOpenDocument(Document document)
    {
        Record("remember " + document.Name + " as the last file");
    }
}
