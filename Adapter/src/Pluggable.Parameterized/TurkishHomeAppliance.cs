namespace dev.kaldiroglu.Adapter.Pluggable.Parameterized;

public class TurkishHomeAppliance : HomeAppliance
{
    protected string name;

    public TurkishHomeAppliance(string name)
    {
        this.name = name;
    }

    public override void Start()
    {
        Console.WriteLine("TurkishHomeAppliance " + name + " is starting!");
        powerSource.TurnOn();
    }

    public override void Stop()
    {
        Console.WriteLine("TurkishHomeAppliance " + name + " stopping!");
        powerSource.TurnOff();
    }
}
