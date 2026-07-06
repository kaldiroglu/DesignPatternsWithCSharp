namespace dev.kaldiroglu.Adapter.Electricity.Domain.Us;

public class USHomeApplicance
{
    private string name;
    private USPowerSource usPowerSource;

    public USHomeApplicance(string name)
    {
        this.name = name;
    }

    public void SetPowerSource(USPowerSource usPowerSource)
    {
        this.usPowerSource = usPowerSource;
    }

    public void Start()
    {
        Console.WriteLine("USHomeApplicance " + name + " is turning on!");
        usPowerSource.PushSwitch();
    }

    public void Stop()
    {
        Console.WriteLine("USHomeApplicance " + name + " turning off!");
        usPowerSource.PushSwitch();
    }
}
