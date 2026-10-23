namespace dev.kaldiroglu.Command.Gof.Solution;

/// <summary>
/// GoF's template as a lambda: "Paste again" is a SimpleCommand. A menu refuses a label it
/// does not have.
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/Command.Demo -- gof-simple-command</c>.
/// </remarks>
public static class SimpleCommandMain
{
    public static void Run()
    {
        Document report = SampleReport.HelloCopied();
        var menu = new Menu()
            .Add(new MenuItem("Paste", new PasteCommand(report)))
            .Add(new MenuItem("Paste again", new SimpleCommand<Document>(report, d => d.Paste())));
        menu.Click("Paste");
        menu.Click("Paste again");
        Console.WriteLine("Menu [" + string.Join(", ", menu.Labels()) + "] clicked. The report reads: "
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
