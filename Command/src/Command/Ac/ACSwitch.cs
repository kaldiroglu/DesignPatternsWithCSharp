namespace dev.kaldiroglu.Command.Ac;

public class ACSwitch
{
    private readonly AirConditioner _ac;
    private readonly ICommand _turnOnCommand;
    private readonly ICommand _turnOffCommand;
    private readonly ICommand _turnOnHeaterCommand;
    private readonly ICommand _turnOnCoolerCommand;

    private readonly Temperature _startingTemperature = new(22);

    public ACSwitch()
    {
        _ac = new AirConditioner(_startingTemperature);

        _turnOnCommand = new TurnOnCommand(_ac);
        _turnOffCommand = new TurnOffCommand(_ac);
        _turnOnHeaterCommand = new HeatCommand(_ac);
        _turnOnCoolerCommand = new CoolCommand(_ac);
    }

    public void TurnOn(int temperature)
    {
        _turnOnCommand.Execute(new Temperature(temperature));
    }

    public void TurnOff()
    {
        _turnOffCommand.Execute(null);
    }

    public void TurnOnHeater(int temperature)
    {
        _turnOnHeaterCommand.Execute(new Temperature(temperature));
    }

    public void TurnOnCooler(int temperature)
    {
        _turnOnCoolerCommand.Execute(new Temperature(temperature));
    }
}
