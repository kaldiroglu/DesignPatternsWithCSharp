namespace dev.kaldiroglu.Adapter.Pluggable.AbstractOperations;

internal class USTurkishPowerAdapter : PowerAdapter
{
    private readonly USPowerSource usPowerSource;

    public USTurkishPowerAdapter(USPowerSource usPowerSource)
    {
        Console.WriteLine("\nUSTurkishPowerAdapter: Converting from USPowerSource to TurkishPowerSource");
        this.usPowerSource = usPowerSource;
    }

    protected override void Deliver()
    {
        usPowerSource.PushSwitch();
    }

    protected override void Cut()
    {
        usPowerSource.PushSwitch();
    }
}
