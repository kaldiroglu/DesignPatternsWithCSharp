namespace dev.kaldiroglu.Adapter.Pluggable.DelegateObject;

/// <summary>
/// <b>Adaptee</b> #1: an American source with its own, incompatible interface (<c>pushSwitch</c>).
/// </summary>
public sealed class USPowerSource
{
    public void PushSwitch()
    {
        Console.WriteLine("US source: switch pushed (110V / 60Hz)");
    }
}
