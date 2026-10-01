namespace dev.kaldiroglu.Command.Hw.Remote;

/// <summary>
/// The <b>Invoker</b>: buttons, and an undo button.
/// <para>
/// Each button holds a way to <em>make</em> a command rather than a command. A command that
/// remembers what it did cannot be pressed twice and undone twice if it is the same object —
/// the second press would overwrite what the first remembered. So every press makes a fresh
/// one, and the history holds them all. This is GoF implementation issue 2 (supporting undo
/// and redo): a command with state may have to be copied before it goes on the history.
/// </para>
/// </summary>
public sealed class RemoteControl
{
    private readonly Dictionary<string, Func<ICommand>> _buttons = new();
    private readonly Stack<ICommand> _history = new();

    public void Assign(string button, Func<ICommand> command) => _buttons[button] = command;

    public void Press(string button)
    {
        if (!_buttons.TryGetValue(button, out var maker))
        {
            throw new ArgumentException("nothing assigned to " + button);
        }

        var command = maker();
        command.Execute();
        _history.Push(command);
    }

    public void Undo()
    {
        if (_history.Count > 0)
        {
            _history.Pop().Undo();
        }
    }

    /// <summary>A remote set up the way the homework describes it.</summary>
    public static RemoteControl StandardFor(Television tv)
    {
        var remote = new RemoteControl();
        remote.Assign("on", () => new TurnOn(tv));
        remote.Assign("off", () => new TurnOff(tv));
        remote.Assign("volume+", () => new VolumeUp(tv));
        remote.Assign("volume-", () => new VolumeDown(tv));
        for (var channel = 0; channel <= 9; channel++)
        {
            var selected = channel;
            remote.Assign(channel.ToString(), () => new SelectChannel(tv, selected));
        }

        return remote;
    }
}
