namespace dev.kaldiroglu.Adapter.Pluggable.DelegateObject;

/// <summary>A <see cref="PowerDelivery"/> delegate for <see cref="UKPowerSource"/> (GoF technique (b)).</summary>
public sealed class UKPowerDelivery : PowerDelivery
{
    private readonly UKPowerSource uk;

    public UKPowerDelivery(UKPowerSource uk)
    {
        this.uk = uk;
    }

    public void Deliver()
    {
        uk.FlipToggle();
    }

    public void Cut()
    {
        uk.FlipToggle();
    }
}
