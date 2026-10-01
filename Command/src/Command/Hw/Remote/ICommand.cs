namespace dev.kaldiroglu.Command.Hw.Remote;

/// <summary>The <b>Command</b>: what every button on the remote does when pressed, and how to take it back.</summary>
public interface ICommand
{
    void Execute();

    void Undo();
}
