namespace dev.kaldiroglu.Command.Ac;

public class CoolCommand : ICommand
{
    private readonly AirConditioner _ac;

    public CoolCommand(AirConditioner ac)
    {
        _ac = ac;
    }

    public void Execute(Temperature? temperature)
    {
        _ac.TurnOnCooler(temperature!);
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
