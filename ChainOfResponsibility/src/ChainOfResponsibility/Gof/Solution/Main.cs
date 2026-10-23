namespace dev.kaldiroglu.ChainOfResponsibility.Gof.Solution;

/// <summary>
/// Builds the editor's window as a chain: button, dialog, application. A control without
/// its own help passes the request to its parent.
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/ChainOfResponsibility.Demo -- gof-solution</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        Application editor = new Application("this is the editor. Press F1 on any control.");
        Dialog printDialog = new Dialog(editor, "the print dialog lets you choose a printer.");
        Button print = new Button(printDialog, "print the document.");
        Button ok = new Button(printDialog);
        Button font = new Button(new Dialog(editor, null));
        // This application has no help. The parameter is a non-nullable string, so null!
        // passes null on purpose.
        Button lonely = new Button(new Dialog(new Application(null!), null));

        Console.WriteLine("print button (has help):        " + print.HandleHelp());
        Console.WriteLine("ok button (asks its dialog):    " + ok.HandleHelp());
        Console.WriteLine("font button (asks the editor):  " + font.HandleHelp());
        Console.WriteLine("no help anywhere in the chain:  " + lonely.HandleHelp());
    }
}
