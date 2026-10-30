namespace dev.kaldiroglu.Command.Ac;

public class HeatCommand : ICommand
{
    private readonly AirConditioner _ac;
    private readonly Temperature _temperature;

    public HeatCommand(AirConditioner ac, Temperature temperature)
    {
        _ac = ac;
        _temperature = temperature;
    }

    public void Execute()
    {
        _ac.TurnOnHeater(_temperature);
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
