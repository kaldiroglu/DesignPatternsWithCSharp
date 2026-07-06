namespace dev.kaldiroglu.Adapter.Pluggable.Parameterized;

/// <summary>
/// <b>Target</b> interface for the electricity domain (matches the course slides): the appliance
/// knows only how to <c>TurnOn</c>/<c>TurnOff</c> a Turkish power source.
/// </summary>
public interface TurkishPowerSource
{
    void TurnOn();

    void TurnOff();
}
