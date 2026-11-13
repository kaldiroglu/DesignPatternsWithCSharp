namespace dev.kaldiroglu.ChainOfResponsibility.Gof.Parent;

/// <summary>
/// Creates the editor, its dialogs and three buttons, and asks each button for help. Each line shows where the
/// button is, then its help.
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/ChainOfResponsibility.Demo -- gof-parent</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        Widget editor = new Widget(null, "editor", "this is the editor. Press F1 on any control.");
        Widget printDialog = new Widget(editor, "print dialog", "the print dialog lets you choose a printer.");
        Widget print = new Widget(printDialog, "print button", "print the document.");
        Widget ok = new Widget(printDialog, "OK button");
        Widget font = new Widget(new Widget(editor, "font dialog"), "font button");

        foreach (Widget button in new[] { print, ok, font })
        {
            Console.WriteLine(button.Path() + ": " + button.HandleHelp());
        }
    }
}
