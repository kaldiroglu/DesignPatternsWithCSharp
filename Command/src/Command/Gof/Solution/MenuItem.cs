namespace dev.kaldiroglu.Command.Gof.Solution;

/// <summary>
/// The <b>Invoker</b>: GoF's <c>MenuItem</c>, which "asks the command to carry out the
/// request".
/// <para>
/// Compare <c>Problem.MenuItem</c>. That one imported the application and branched on its
/// own label. This one imports nothing from any application — it is toolkit code, and it
/// could ship in a library compiled years before the application that uses it.
/// </para>
/// </summary>
public sealed class MenuItem
{
    private ICommand _command;

    public MenuItem(string label, ICommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        Label = label;
        _command = command;
    }

    public string Label { get; }

    /// <summary>The same item can be given a different job while the program runs.</summary>
    public void SetCommand(ICommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        _command = command;
    }

    public void Clicked() => _command.Execute();
}
