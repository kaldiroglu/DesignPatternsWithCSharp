namespace dev.kaldiroglu.Command.Hw.Macro;

public sealed record TypeText(Editor Editor, string Words) : IEditorCommand
{
    public void Execute() => Editor.Type(Words);

    public IEditorCommand On(Editor other) => new TypeText(other, Words);
}
