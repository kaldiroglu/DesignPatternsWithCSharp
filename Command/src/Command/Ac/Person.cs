namespace dev.kaldiroglu.Command.Ac;

public class Person
{
    private readonly ACSwitch _acSwitch;

    public Person(ACSwitch acSwitch)
    {
        _acSwitch = acSwitch;
    }

    /// <summary>
    /// The Java original's <c>main</c>. Run it with
    /// <c>dotnet run --project src/Command.Demo -- ac</c>.
    /// </summary>
    public static void Run()
    {
        var acSwitch = new ACSwitch();
        var person = new Person(acSwitch);
        person.Action();
    }

    public void Action()
    {
        var temperature = 20;

        _acSwitch.TurnOn(temperature);
        _acSwitch.TurnOff();

        _acSwitch.TurnOnCooler(18);
        _acSwitch.TurnOnHeater(25);

        _acSwitch.TurnOn(temperature);
        _acSwitch.TurnOnCooler(18);
        _acSwitch.TurnOnCooler(25);
        _acSwitch.TurnOnCooler(15);
        _acSwitch.TurnOnHeater(23);
        _acSwitch.TurnOnHeater(20);
        _acSwitch.TurnOnHeater(25);
        _acSwitch.TurnOff();
    }
}
