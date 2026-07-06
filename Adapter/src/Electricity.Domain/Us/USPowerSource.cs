namespace dev.kaldiroglu.Adapter.Electricity.Domain.Us;

public interface USPowerSource
{
    void ProvidePowerAt110V();

    void PushSwitch();
}
