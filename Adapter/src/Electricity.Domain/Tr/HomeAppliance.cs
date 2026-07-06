namespace dev.kaldiroglu.Adapter.Electricity.Domain.Tr;

public abstract class HomeAppliance : Appliance
{
    protected TurkishPowerSource powerSource;

    public virtual void SetPowerSource(TurkishPowerSource powerSource)
    {
        this.powerSource = powerSource;
    }

    public abstract void Start();

    public abstract void Stop();
}
