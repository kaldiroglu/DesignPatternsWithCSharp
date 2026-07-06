namespace dev.kaldiroglu.Adapter.Electricity.Domain.Us;

public class USPowerProvider : USPowerSource
{
    private bool on;

    public void ProvidePowerAt110V()
    {
        Console.WriteLine("USPowerProvider provides electricity at 110V!");
    }

    public USPowerProvider()
    {
        Console.WriteLine("USPowerProvider is up and running.");
    }

    public void PushSwitch()
    {
        if (!on)
        {
            on = true;
            Console.WriteLine("USPowerProvider started to provide electricity.");
        }
        else
        {
            on = false;
            Console.WriteLine("USPowerProvider stopped to provide electricity.");
        }
    }
}
