namespace dev.kaldiroglu.Command.Ac;

public class TurnOffCommand : ICommand
{
    private readonly AirConditioner _ac;

    public TurnOffCommand(AirConditioner ac)
    {
        _ac = ac;
    }

    public void Execute(Temperature? temperature)
    {
        _ac.TurnOff();
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
