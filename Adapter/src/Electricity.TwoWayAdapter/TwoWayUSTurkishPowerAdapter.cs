using dev.kaldiroglu.Adapter.Electricity.Domain.Tr;
using dev.kaldiroglu.Adapter.Electricity.Domain.Us;

namespace dev.kaldiroglu.Adapter.Electricity.TwoWayAdapter;

public class TwoWayUSTurkishPowerAdapter : TurkishPowerSource, USPowerSource
{
    private USPowerSource usPowerSource;
    private TurkishPowerSource turkishPowerSource;

    private bool on;

    private string powerSource;

    public TwoWayUSTurkishPowerAdapter(TurkishPowerSource turkishPowerSource)
    {
        this.turkishPowerSource = turkishPowerSource;
        powerSource = "tr";
    }

    public TwoWayUSTurkishPowerAdapter(USPowerSource usPowerSource)
    {
        this.usPowerSource = usPowerSource;
        powerSource = "us";
    }

    public void ProvidePowerAt110V()
    {
        if (powerSource.Equals("us"))
            usPowerSource.ProvidePowerAt110V(); // That's USPowerSource
        else
            Convert220To110(); // That's TurkishPowerSource
    }

    public void ProvidePowerAt220V()
    {
        if (powerSource.Equals("us"))
            Convert110To220(); // That's USPowerSource
        else
            turkishPowerSource.ProvidePowerAt220V(); // That's
                                                     // TurkishPowerSource
    }

    public void PushSwitch()
    {
        if (!on)
        {
            on = true;
            if (powerSource.Equals("us"))
                usPowerSource.PushSwitch();
            else
                turkishPowerSource.TurnOn();
        }
        else
        {
            on = false;
            if (powerSource.Equals("us"))
                usPowerSource.PushSwitch();
            else
                turkishPowerSource.TurnOff();
        }
    }

    public void TurnOn()
    {
        if (!on)
        {
            if (powerSource.Equals("us"))
                usPowerSource.PushSwitch();
            else
                turkishPowerSource.TurnOn();
            on = true;
        }
    }

    public void TurnOff()
    {
        if (on)
        {
            if (powerSource.Equals("us"))
                usPowerSource.PushSwitch();
            else
                turkishPowerSource.TurnOff();
            on = false;
        }
    }

    private void Convert110To220()
    {
        Console.WriteLine("TwoWayUSTurkishPowerAdapter: Converting from USPowerSource to provide 220V");
    }

    private void Convert220To110()
    {
        Console.WriteLine("TwoWayUSTurkishPowerAdapter: Converting from TurkishPowerSource to provide 110V");
    }
}
