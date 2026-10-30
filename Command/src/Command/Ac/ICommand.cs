namespace dev.kaldiroglu.Command.Ac;

public interface ICommand
{
    void Execute();

    void Undo();

    void Redo();
}
