namespace dev.kaldiroglu.Command.Hw.Macro;

public sealed record UpperCaseLastWord(Editor Editor) : IEditorCommand
{
    public void Execute() => Editor.UpperCaseLastWord();

    public IEditorCommand On(Editor other) => new UpperCaseLastWord(other);
}
