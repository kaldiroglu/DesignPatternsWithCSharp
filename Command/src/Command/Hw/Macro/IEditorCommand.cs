namespace dev.kaldiroglu.Command.Hw.Macro;

/// <summary>
/// The <b>Command</b>. Besides doing its job, it can make a copy of itself aimed at another
/// editor — which is what replaying a recorded macro somewhere else needs, because a
/// recorded command is bound to the editor it was recorded in. A copy that only changes its
/// receiver is the Prototype pattern, the relation GoF draw between the two.
/// </summary>
public interface IEditorCommand
{
    void Execute();

    IEditorCommand On(Editor other);
}
