namespace dev.kaldiroglu.Adapter.Electricity.ClassAdapter;

/// <summary>
/// <b>Adaptee</b> participant: an existing American source whose interface is incompatible with
/// <see cref="TurkishPowerSource"/>. A single <c>PushSwitch</c> toggles power on and off (US style), and
/// <c>IsLive</c> reports the current state. We reuse this class without modifying it.
/// </summary>
public class USPowerSource
{
    private bool live;

    /// <summary>US style: one switch toggles power on/off.</summary>
    public void PushSwitch()
    {
        live = !live;
        Console.WriteLine("    US source: switch pushed -> " + (live ? "LIVE (110V/60Hz)" : "OFF"));
    }

    public bool IsLive()
    {
        return live;
    }
}
