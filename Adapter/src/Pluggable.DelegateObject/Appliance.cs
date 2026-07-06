namespace dev.kaldiroglu.Adapter.Pluggable.DelegateObject;

public interface Appliance
{
    void SetPowerSource(TurkishPowerSource powerSource);

    void Start();

    void Stop();
}
