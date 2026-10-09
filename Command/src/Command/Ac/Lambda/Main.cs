namespace dev.kaldiroglu.Command.Ac.Lambda;

/// <summary>
/// The same steps as <c>Ac.Person</c>, through the switch whose requests are functions.
/// Run it with <c>dotnet run --project src/Command.Demo -- ac-lambda</c>.
/// </summary>
public static class Main
{
    public static void Run()
    {
        var acSwitch = new ACSwitch();
        int temperature = 20;
        acSwitch.TurnOn(temperature);
        acSwitch.TurnOff();
        acSwitch.TurnOnCooler(18);
        acSwitch.TurnOnHeater(25);
        acSwitch.TurnOn(temperature);
        acSwitch.TurnOnCooler(18);
        acSwitch.TurnOnCooler(25);
        acSwitch.TurnOnCooler(15);
        acSwitch.TurnOnHeater(23);
        acSwitch.TurnOnHeater(20);
        acSwitch.TurnOnHeater(25);
        acSwitch.TurnOff();
    }
}
