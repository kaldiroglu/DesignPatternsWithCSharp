namespace dev.kaldiroglu.Command.Gof;

/// <summary>
/// Shows the receivers on their own: an application, a document and a clipboard, with no menu
/// at all.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Command.Demo -- gof</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var application = new Application();
        var letter = new Document("letter", application.Clipboard);
        application.Add(letter);
        letter.Open();
        Console.WriteLine("Opened '" + letter.Name + "': " + (letter.IsOpen ? "true" : "false"));

        letter.Type("Dear Deniz");
        letter.Copy();
        Console.WriteLine("Typed and copied. Clipboard holds: " + application.Clipboard.Contents);

        letter.Paste();
        Console.WriteLine("Pasted. The letter reads: " + letter.Text);
        Console.WriteLine("The document knows how to copy and paste, and nothing about menus.");
    }
}
