namespace dev.kaldiroglu.Command.Gof.Solution;

/// <summary>
/// The <b>Command</b>: "declares an interface for executing an operation" (p. 233).
/// <para>
/// One method, with no arguments and no name worth the word. A menu item that holds one can
/// ask for it to happen without knowing what "it" is or what it happens to.
/// </para>
/// </summary>
public interface ICommand
{
    void Execute();
}
