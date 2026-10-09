namespace dev.kaldiroglu.Command.Hw.Macro;

/// <summary>Shows edits recorded in one editor and replayed in another, through a copy of each command.</summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Command.Demo -- hw-macro</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var first = new Editor();
        var recorder = new MacroRecorder();

        recorder.Start();
        recorder.Run(new TypeText(first, "hello world"));
        recorder.Run(new UpperCaseLastWord(first));
        recorder.Run(new TypeText(first, "!!"));
        recorder.Run(new DeleteLast(first, 1));
        recorder.Stop();
        Console.WriteLine("Recorded " + recorder.Size + " edits. First editor: " + first.Text);

        var second = new Editor();
        second.Type("goodbye ");
        recorder.ReplayOn(second);
        Console.WriteLine("Replayed on the second editor: " + second.Text);
        Console.WriteLine("The first editor did not change: " + first.Text);
    }
}
