namespace dev.kaldiroglu.Command.Hw.Macro;

/// <summary>
/// The <b>Invoker</b>, with a record button.
/// <para>
/// While recording, every command the user runs is kept as well as executed. Replaying on
/// another editor runs a copy of each command aimed at that editor; the recorded commands
/// themselves are never run again, so the editor the macro was recorded in is not touched.
/// </para>
/// </summary>
public sealed class MacroRecorder
{
    private readonly List<IEditorCommand> _recorded = [];
    private bool _recording;

    public void Start()
    {
        _recorded.Clear();
        _recording = true;
    }

    public void Stop() => _recording = false;

    public void Run(IEditorCommand command)
    {
        command.Execute();
        if (_recording)
        {
            _recorded.Add(command);
        }
    }

    public int Size => _recorded.Count;

    public void ReplayOn(Editor other)
    {
        foreach (var command in _recorded)
        {
            command.On(other).Execute();
        }
    }
}
