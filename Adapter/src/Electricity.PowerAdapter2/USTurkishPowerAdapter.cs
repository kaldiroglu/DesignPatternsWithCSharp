using dev.kaldiroglu.Adapter.Electricity.Domain.Tr;
using dev.kaldiroglu.Adapter.Electricity.Domain.Us;

namespace dev.kaldiroglu.Adapter.Electricity.PowerAdapter2;

public class USTurkishPowerAdapter : TurkishPowerSource
{
    private USPowerSource usPowerSource;

    public USTurkishPowerAdapter(USPowerSource usPowerSource)
    {
        Console.WriteLine("\nUSTurkishPowerAdapter: Converting from USPowerSource to TurkishPowerSource");
        this.usPowerSource = usPowerSource;
        Convert110To220(usPowerSource);
    }

    public void ProvidePowerAt220V()
    {
        usPowerSource.ProvidePowerAt110V();
        Convert110To220(usPowerSource);
    }

    public void TurnOn()
    {
        usPowerSource.PushSwitch();
    }

    public void TurnOff()
    {
        usPowerSource.PushSwitch();
    }

    private void Convert110To220(USPowerSource usPowerSource)
    {
        Check();
        RegulateVoltage();
        Console.WriteLine("USTurkishPowerAdapter: Converting from 110V to 220V");
    }

    /// <summary>
    /// Some extra functionality the adaptor provides.
    /// </summary>
    private void Check()
    {
        Console.WriteLine("Making some safety checks.");
    }

    /// <summary>
    /// Some extra functionality the adaptor provides.
    /// </summary>
    private void RegulateVoltage()
    {
        Console.WriteLine("Regulating the voltage.");
    }
}
