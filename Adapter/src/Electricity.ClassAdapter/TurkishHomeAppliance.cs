namespace dev.kaldiroglu.Adapter.Electricity.ClassAdapter;

/// <summary>
/// <b>Client</b>: a Turkish appliance that works only through <see cref="TurkishPowerSource"/> and never
/// learns that its power really comes from a <see cref="USPowerSource"/> behind the adapter.
/// </summary>
public class TurkishHomeAppliance
{
    private readonly string name;
    private readonly TurkishPowerSource power;

    public TurkishHomeAppliance(string name, TurkishPowerSource power)
    {
        this.name = name;
        this.power = power;
    }

    public void Start()
    {
        Console.WriteLine(name + " starting");
        power.TurnOn();
    }

    public void Stop()
    {
        Console.WriteLine(name + " stopping");
        power.TurnOff();
    }
}
