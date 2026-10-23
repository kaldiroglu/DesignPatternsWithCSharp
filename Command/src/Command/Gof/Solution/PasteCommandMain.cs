namespace dev.kaldiroglu.Command.Gof.Solution;

/// <summary>A menu item holds a PasteCommand and calls Execute() when it is clicked.</summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Command.Demo -- gof-paste-command</c>.
/// </remarks>
public static class PasteCommandMain
{
    public static void Run()
    {
        Document report = SampleReport.HelloCopied();
        new MenuItem("Paste", new PasteCommand(report)).Clicked();
        Console.WriteLine("Paste clicked. The report reads: " + report.Text);
    }
}
