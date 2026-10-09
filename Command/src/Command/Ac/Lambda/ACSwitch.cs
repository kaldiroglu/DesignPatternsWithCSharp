namespace dev.kaldiroglu.Command.Ac.Lambda;

/// <summary>
/// The wall switch of <c>Ac</c>, with each request as a function instead of a command class.
/// <para>
/// <c>Ac.ICommand</c> has three methods — <c>Execute</c>, <c>Undo</c>, <c>Redo</c> — so a
/// lambda cannot implement it. Here each request is a method group of the air conditioner:
/// an <see cref="Action{T}"/> of a temperature, or an <see cref="Action"/> for turning off.
/// The four command classes are gone. So are <c>Undo</c> and <c>Redo</c>: in <c>Ac</c> they
/// are empty, and a function cannot remember what it did. A request that must be undone
/// needs a class, as the teller's transactions show.
/// </para>
/// </summary>
public class ACSwitch
{
    private readonly Action<Temperature> _turnOn;
    private readonly Action _turnOff;
    private readonly Action<Temperature> _turnOnHeater;
    private readonly Action<Temperature> _turnOnCooler;

    public ACSwitch()
    {
        var ac = new AirConditioner(new Temperature(22));
        _turnOn = ac.TurnOn;
        _turnOff = ac.TurnOff;
        _turnOnHeater = ac.TurnOnHeater;
        _turnOnCooler = ac.TurnOnCooler;
    }

    public void TurnOn(int temperature) => _turnOn(new Temperature(temperature));

    public void TurnOff() => _turnOff();

    public void TurnOnHeater(int temperature) => _turnOnHeater(new Temperature(temperature));

    public void TurnOnCooler(int temperature) => _turnOnCooler(new Temperature(temperature));
}
