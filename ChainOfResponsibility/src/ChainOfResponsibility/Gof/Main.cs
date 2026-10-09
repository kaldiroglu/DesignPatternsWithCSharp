using dev.kaldiroglu.ChainOfResponsibility.Gof.Problem;
using dev.kaldiroglu.ChainOfResponsibility.Gof.Solution;

namespace dev.kaldiroglu.ChainOfResponsibility.Gof;

/// <summary>Asks for help on three controls in both designs. They print the same lines.</summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/ChainOfResponsibility.Demo -- gof</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        HelpDesk desk = new HelpDesk();
        Console.WriteLine("Before the pattern");
        Console.WriteLine("  print button: " + desk.HelpFor("print button"));
        Console.WriteLine("  ok button:    " + desk.HelpFor("ok button"));
        Console.WriteLine("  font button:  " + desk.HelpFor("font button"));

        Application editor = new Application("this is the editor. Press F1 on any control.");
        Dialog printDialog = new Dialog(editor, "the print dialog lets you choose a printer.");
        Button print = new Button(printDialog, "print the document.");
        Button ok = new Button(printDialog);
        Dialog fontDialog = new Dialog(editor, null);
        Button font = new Button(fontDialog);

        Console.WriteLine("With the chain");
        Console.WriteLine("  print button: " + print.HandleHelp());
        Console.WriteLine("  ok button:    " + ok.HandleHelp());
        Console.WriteLine("  font button:  " + font.HandleHelp());
    }
}
