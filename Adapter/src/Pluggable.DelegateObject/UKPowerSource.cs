namespace dev.kaldiroglu.Adapter.Pluggable.DelegateObject;

/// <summary>
/// <b>Adaptee</b> #2: a British source with yet another interface (<c>flipToggle</c>). Having two
/// differently-shaped adaptees is what makes the pluggable adapter worth it: one adapter class,
/// many sources.
/// </summary>
public sealed class UKPowerSource
{
    public void FlipToggle()
    {
        Console.WriteLine("UK source: toggle flipped (230V / 50Hz)");
    }
}
