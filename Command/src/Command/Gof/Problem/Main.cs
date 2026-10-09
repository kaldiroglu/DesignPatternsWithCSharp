namespace dev.kaldiroglu.Command.Gof.Problem;

/// <summary>
/// Shows a menu item that branches on its own label, and a misspelled label that fails on the
/// first click.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Command.Demo -- gof-problem</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var application = new Application();
        new MenuItem("Open", application, () => "letter").Clicked();
        Document letter = application.Current();
        letter.Type("Dear Deniz");

        new MenuItem("Copy", application, () => "").Clicked();
        new MenuItem("Paste", application, () => "").Clicked();
        Console.WriteLine("Open, Copy, Paste clicked. The letter reads: " + letter.Text);

        var misspelled = new MenuItem("paste", application, () => "");
        try
        {
            misspelled.Clicked();
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine("A menu entry labeled 'paste' compiled, and the first click failed: "
                + e.Message);
        }
    }
}
