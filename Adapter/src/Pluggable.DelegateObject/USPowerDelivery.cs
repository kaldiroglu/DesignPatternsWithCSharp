namespace dev.kaldiroglu.Adapter.Pluggable.DelegateObject;

/// <summary>A <see cref="PowerDelivery"/> delegate for <see cref="USPowerSource"/> (GoF technique (b)).</summary>
public sealed class USPowerDelivery : PowerDelivery
{
    private readonly USPowerSource us;

    public USPowerDelivery(USPowerSource us)
    {
        this.us = us;
    }

    public void Deliver()
    {
        us.PushSwitch();
    }

    public void Cut()
    {
        us.PushSwitch();
    }
}
