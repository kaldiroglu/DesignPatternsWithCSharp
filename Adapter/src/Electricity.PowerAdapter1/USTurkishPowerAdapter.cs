using dev.kaldiroglu.Adapter.Electricity.Domain.Tr;
using dev.kaldiroglu.Adapter.Electricity.Domain.Us;

namespace dev.kaldiroglu.Adapter.Electricity.PowerAdapter1;

public class USTurkishPowerAdapter : TurkishPowerSource
{
    private USPowerSource usPowerSource;
    private bool on;

    public USTurkishPowerAdapter(USPowerSource usPowerSource)
    {
        Console.WriteLine("\nUSTurkishPowerAdapter: Converting from USPowerSource to TurkishPowerSource");
        this.usPowerSource = usPowerSource;
        //		Convert110To220(usPowerSource);
    }

    public void ProvidePowerAt220V()
    {
        usPowerSource.ProvidePowerAt110V();
        Convert110To220(usPowerSource);
    }

    public void TurnOn()
    {
        if (!on)
        {
            usPowerSource.PushSwitch();
            on = true;
        }
    }

    public void TurnOff()
    {
        if (on)
        {
            usPowerSource.PushSwitch();
            on = false;
        }
    }

    private void Convert110To220(USPowerSource usPowerSource)
    {
        Console.WriteLine("USTurkishPowerAdapter: Converting from 110V to 220V");
    }
}
