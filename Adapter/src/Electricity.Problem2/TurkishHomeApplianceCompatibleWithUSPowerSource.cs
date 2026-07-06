using dev.kaldiroglu.Adapter.Electricity.Domain.Tr;
using dev.kaldiroglu.Adapter.Electricity.Domain.Us;

namespace dev.kaldiroglu.Adapter.Electricity.Problem2;

public class TurkishHomeApplianceCompatibleWithUSPowerSource : TurkishHomeAppliance
{
    private USPowerSource usPowerSource;
    private bool turkishPowerSource;

    public TurkishHomeApplianceCompatibleWithUSPowerSource(string name)
        : base(name)
    {
    }

    public override void SetPowerSource(TurkishPowerSource powerSource)
    {
        this.powerSource = powerSource;
        turkishPowerSource = true;
    }

    public void SetUsPowerSource(USPowerSource usPowerSource)
    {
        this.usPowerSource = usPowerSource;
        turkishPowerSource = false;
    }

    public override void Start()
    {
        Console.WriteLine("TurkishHomeAppliance " + name + " is starting!");
        if (turkishPowerSource)
            powerSource.TurnOn();
        else
            usPowerSource.PushSwitch();
    }

    public override void Stop()
    {
        Console.WriteLine("TurkishHomeAppliance " + name + " stoping!");
        if (turkishPowerSource)
            powerSource.TurnOff();
        else
            usPowerSource.PushSwitch();
    }
}
