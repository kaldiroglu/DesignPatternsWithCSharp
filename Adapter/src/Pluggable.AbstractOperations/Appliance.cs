namespace dev.kaldiroglu.Adapter.Pluggable.AbstractOperations;

public interface Appliance
{
    void SetPowerSource(TurkishPowerSource powerSource);

    void Start();

    void Stop();
}
