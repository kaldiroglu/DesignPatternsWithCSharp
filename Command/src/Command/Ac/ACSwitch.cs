namespace dev.kaldiroglu.Command.Ac;

/// <summary>The invoker. Each press makes one complete request, and the switch only runs it.</summary>
public class ACSwitch
{
    private readonly AirConditioner _ac = new(new Temperature(22));

    public void TurnOn(int temperature)
    {
        Press(new TurnOnCommand(_ac, new Temperature(temperature)));
    }

    public void TurnOff()
    {
        Press(new TurnOffCommand(_ac));
    }

    public void TurnOnHeater(int temperature)
    {
        Press(new HeatCommand(_ac, new Temperature(temperature)));
    }

    public void TurnOnCooler(int temperature)
    {
        Press(new CoolCommand(_ac, new Temperature(temperature)));
    }

    private static void Press(ICommand command)
    {
        command.Execute();
    }
}
