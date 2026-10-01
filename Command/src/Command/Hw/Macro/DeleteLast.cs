namespace dev.kaldiroglu.Command.Hw.Macro;

public sealed record DeleteLast(Editor Editor, int Count) : IEditorCommand
{
    public void Execute() => Editor.DeleteLast(Count);

    public IEditorCommand On(Editor other) => new DeleteLast(other, Count);
}
