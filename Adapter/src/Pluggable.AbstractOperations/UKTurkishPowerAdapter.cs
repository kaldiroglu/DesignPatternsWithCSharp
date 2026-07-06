namespace dev.kaldiroglu.Adapter.Pluggable.AbstractOperations;

internal class UKTurkishPowerAdapter : PowerAdapter
{
    private readonly UKPowerSource ukPowerSource;

    public UKTurkishPowerAdapter(UKPowerSource ukPowerSource)
    {
        Console.WriteLine("\nUKTurkishPowerAdapter: Converting from UKPowerSource to TurkishPowerSource");
        this.ukPowerSource = ukPowerSource;
    }

    protected override void Deliver()
    {
        ukPowerSource.FlipToggle();
    }

    protected override void Cut()
    {
        ukPowerSource.FlipToggle();
    }
}
