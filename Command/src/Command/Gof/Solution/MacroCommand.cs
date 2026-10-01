namespace dev.kaldiroglu.Command.Gof.Solution;

/// <summary>
/// A <b>ConcreteCommand</b> made of commands: GoF's <c>MacroCommand</c>.
/// <para>
/// "A MacroCommand has no explicit receiver, because the commands it sequences define their
/// own receiver" (p. 235). It is a Composite of commands, and a menu item holding one
/// cannot tell it from any other command.
/// </para>
/// </summary>
public sealed class MacroCommand : ICommand
{
    private readonly List<ICommand> _commands = [];

    public MacroCommand Add(ICommand command)
    {
        _commands.Add(command);
        return this;
    }

    public void Remove(ICommand command) => _commands.Remove(command);

    public int Size => _commands.Count;

    public void Execute()
    {
        foreach (var command in _commands)
        {
            command.Execute();
        }
    }
}
