namespace dev.kaldiroglu.Command.Ac;

public class TurnOffCommand : ICommand
{
    private readonly AirConditioner _ac;

    public TurnOffCommand(AirConditioner ac)
    {
        _ac = ac;
    }

    public void Execute()
    {
        _ac.TurnOff();
    }

    public void Undo()
    {
        // Left open: an exercise writes it.
    }

    public void Redo()
    {
        // Left open: an exercise writes it.
    }
}
