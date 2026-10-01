namespace dev.kaldiroglu.Command.Gof.Problem;

/// <summary>
/// A menu item that knows what it does.
/// <para>
/// This is the design GoF's motivation says a toolkit cannot have: "the toolkit can't
/// implement the request explicitly in the button or menu, because only applications that
/// use the toolkit know what should be done on which object" (p. 233). Here it does
/// anyway, so the toolkit's menu item imports the application's classes, branches on its
/// own label, and has to be edited for every menu entry any application will ever add.
/// </para>
/// <para>
/// The label is a string, so a menu entry spelled differently from its branch compiles and
/// fails the first time somebody clicks it.
/// </para>
/// </summary>
public sealed class MenuItem
{
    private readonly Application _application;    // the toolkit now knows the application
    private readonly Func<string> _askUser;        // stands in for a file dialog

    public MenuItem(string label, Application application, Func<string> askUser)
    {
        Label = label;
        _application = application;
        _askUser = askUser;
    }

    public string Label { get; }

    public void Clicked()
    {
        switch (Label)
        {
            case "Open":
                var document = new Document(_askUser(), _application.Clipboard);
                _application.Add(document);
                document.Open();
                break;
            case "Copy":
                _application.Current().Copy();
                break;
            case "Paste":
                _application.Current().Paste();
                break;
            default:
                throw new InvalidOperationException("no such menu entry: " + Label);
        }
    }
}
