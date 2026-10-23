namespace dev.kaldiroglu.Command.Gof.Solution;

/// <summary>
/// A command that does real work, OpenCommand, and one that combines others, MacroCommand.
/// PasteCommand, which only forwards, is in <see cref="PasteCommandMain"/>.
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/Command.Demo -- gof-three-commands</c>.
/// </remarks>
public static class ThreeCommandsMain
{
    public static void Run()
    {
        var application = new Application();
        new MenuItem("Open", new OpenCommand(application, () => "report")).Clicked();
        Document opened = application.Current();
        Console.WriteLine("Open clicked. Opened '" + opened.Name + "': " + (opened.IsOpen ? "true" : "false"));

        Document report = SampleReport.HelloCopied();
        var pasteTwice = new MacroCommand()
            .Add(new PasteCommand(report))
            .Add(new PasteCommand(report));
        new MenuItem("Paste twice", pasteTwice).Clicked();
        Console.WriteLine("A macro of " + pasteTwice.Size + " commands clicked. The report reads: "
            + report.Text);
    }
}
