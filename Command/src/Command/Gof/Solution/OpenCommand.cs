namespace dev.kaldiroglu.Command.Gof.Solution;

/// <summary>
/// A <b>ConcreteCommand</b> that does more than forward: GoF's <c>OpenCommand</c>.
/// <para>
/// It asks the user for a name, creates a document, adds it to the application and opens
/// it. Several steps and a conversation with the user — so a command can be as clever as
/// the request needs. How clever it should be is GoF implementation issue 1 (how
/// intelligent should a command be?): somewhere between forwarding to a receiver and doing
/// the whole job itself.
/// </para>
/// </summary>
public sealed class OpenCommand : ICommand
{
    private readonly Application _application;
    private readonly Func<string?> _askUser;       // stands in for a file dialog

    public OpenCommand(Application application, Func<string?> askUser)
    {
        _application = application;
        _askUser = askUser;
    }

    public void Execute()
    {
        var name = _askUser();
        if (string.IsNullOrWhiteSpace(name))
        {
            return;                                 // the user cancelled the dialog
        }

        var document = new Document(name, _application.Clipboard);
        _application.Add(document);
        document.Open();
    }
}
