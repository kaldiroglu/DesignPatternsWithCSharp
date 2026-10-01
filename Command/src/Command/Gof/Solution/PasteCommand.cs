namespace dev.kaldiroglu.Command.Gof.Solution;

/// <summary>
/// A <b>ConcreteCommand</b>: GoF's <c>PasteCommand</c>, whose "receiver is the Document
/// object it is supplied upon instantiation" (p. 234).
/// <para>
/// It binds a receiver to an action and does nothing else. The work is the document's.
/// </para>
/// </summary>
public sealed class PasteCommand : ICommand
{
    private readonly Document _document;

    public PasteCommand(Document document)
    {
        _document = document;
    }

    public void Execute() => _document.Paste();
}
