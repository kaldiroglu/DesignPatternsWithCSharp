namespace dev.kaldiroglu.Command.Ac;

public class CoolCommand : ICommand
{
    private readonly AirConditioner _ac;
    private readonly Temperature _temperature;

    public CoolCommand(AirConditioner ac, Temperature temperature)
    {
        _ac = ac;
        _temperature = temperature;
    }

    public void Execute()
    {
        _ac.TurnOnCooler(_temperature);
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
