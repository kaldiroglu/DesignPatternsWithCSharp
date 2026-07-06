namespace dev.kaldiroglu.Adapter.Electricity.Domain.Tr;

public interface TurkishPowerSource
{
    void ProvidePowerAt220V();

    void TurnOn();

    void TurnOff();
}
