namespace dev.kaldiroglu.Adapter.Pluggable.AbstractOperations;

public sealed class USPowerSource
{
    public void PushSwitch()
    {
        Console.WriteLine("US source: switch pushed (110V / 60Hz)");
    }
}
