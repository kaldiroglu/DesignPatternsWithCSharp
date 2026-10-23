namespace dev.kaldiroglu.Command.Gof.Solution;

/// <summary>A report opened through a menu item, with "Hello" typed and copied.</summary>
internal static class SampleReport
{
    internal static Document HelloCopied()
    {
        var application = new Application();
        new MenuItem("Open", new OpenCommand(application, () => "report")).Clicked();
        Document report = application.Current();
        report.Type("Hello");
        report.Copy();
        return report;
    }
}
