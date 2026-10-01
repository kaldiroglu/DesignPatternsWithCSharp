namespace dev.kaldiroglu.Command.Ac;

public interface ICommand
{
    void Execute(Temperature? temperature);

    void Undo();

    void Redo();
}
