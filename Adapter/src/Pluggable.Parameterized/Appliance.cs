namespace dev.kaldiroglu.Adapter.Pluggable.Parameterized;

public interface Appliance
{
    void SetPowerSource(TurkishPowerSource powerSource);

    void Start();

    void Stop();
}
