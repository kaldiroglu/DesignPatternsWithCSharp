namespace dev.kaldiroglu.Adapter.Pluggable.AbstractOperations;

internal abstract class PowerAdapter : TurkishPowerSource
{
    public void TurnOn()
    {
        Deliver();
    }

    public void TurnOff()
    {
        Cut();
    }

    protected abstract void Deliver();

    protected abstract void Cut();
}
