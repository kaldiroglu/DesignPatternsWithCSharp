namespace dev.kaldiroglu.Memento.Hw.Editor;

/// <summary>
/// Types two words, undoes twice and redoes once. The bar shows where the cursor is.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Memento.Demo -- hw-editor</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        TextEditor editor = new TextEditor();
        History history = new History(editor);

        history.Type("Hello");
        history.Type(" world");
        Console.WriteLine("After typing:     " + editor);
        history.Undo();
        Console.WriteLine("After one undo:   " + editor);
        history.Undo();
        Console.WriteLine("After two undos:  " + editor);
        history.Redo();
        Console.WriteLine("After one redo:   " + editor);
    }
}
