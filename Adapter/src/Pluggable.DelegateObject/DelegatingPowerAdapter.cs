namespace dev.kaldiroglu.Adapter.Pluggable.DelegateObject;

/// <summary>
/// <b>Pluggable adapter via a delegate object</b> &ndash; GoF technique (b), p. 143. One adapter
/// class serves any adaptee: swap the <see cref="PowerDelivery"/> delegate to re-target, no subclassing.
/// </summary>
public sealed class DelegatingPowerAdapter : TurkishPowerSource
{
    private readonly PowerDelivery delivery;

    public DelegatingPowerAdapter(PowerDelivery delivery)
    {
        this.delivery = delivery;
    }

    public void TurnOn()
    {
        delivery.Deliver();
    }

    public void TurnOff()
    {
        delivery.Cut();
    }
}
