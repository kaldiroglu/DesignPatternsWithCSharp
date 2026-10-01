namespace dev.kaldiroglu.Command.Ac;

public class TurnOnCommand : ICommand
{
    private readonly AirConditioner _ac;

    public TurnOnCommand(AirConditioner ac)
    {
        _ac = ac;
    }

    public void Execute(Temperature? temperature)
    {
        _ac.TurnOn(temperature!);
    }

    public void Undo()
    {
        // TODO: not implemented
    }

    public void Redo()
    {
        // TODO: not implemented
    }
}
