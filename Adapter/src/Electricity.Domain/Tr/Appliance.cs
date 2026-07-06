namespace dev.kaldiroglu.Adapter.Electricity.Domain.Tr;

public interface Appliance
{
    void SetPowerSource(TurkishPowerSource powerSource);

    void Start();

    void Stop();
}
