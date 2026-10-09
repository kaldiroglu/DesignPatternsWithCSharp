namespace dev.kaldiroglu.Command.Gof.Solution;

/// <summary>Shows menu items that only execute a command: open, paste, a method reference and a macro.</summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Command.Demo -- gof-solution</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var application = new Application();
        new MenuItem("Open", new OpenCommand(application, () => "report")).Clicked();
        Document report = application.Current();
        Console.WriteLine("Open clicked. Opened '" + report.Name + "': " + (report.IsOpen ? "true" : "false"));

        report.Type("Hello");
        report.Copy();
        var menu = new Menu()
            .Add(new MenuItem("Paste", new PasteCommand(report)))
            .Add(new MenuItem("Paste again", new SimpleCommand<Document>(report, d => d.Paste())));
        menu.Click("Paste");
        menu.Click("Paste again");
        Console.WriteLine("Menu [" + string.Join(", ", menu.Labels()) + "] clicked. The report reads: "
            + report.Text);

        var pasteTwice = new MacroCommand()
            .Add(new PasteCommand(report))
            .Add(new PasteCommand(report));
        new MenuItem("Paste twice", pasteTwice).Clicked();
        Console.WriteLine("A macro of " + pasteTwice.Size + " commands clicked. The report reads: "
            + report.Text);

        try
        {
            menu.Click("paste");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine("A label the menu does not have is refused: " + e.Message);
        }
    }
}
