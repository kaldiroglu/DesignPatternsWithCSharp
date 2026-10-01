namespace dev.kaldiroglu.Command.Ac;

public class HeatCommand : ICommand
{
    private readonly AirConditioner _ac;

    public HeatCommand(AirConditioner ac)
    {
        _ac = ac;
    }

    public void Execute(Temperature? temperature)
    {
        _ac.TurnOnHeater(temperature!);
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
