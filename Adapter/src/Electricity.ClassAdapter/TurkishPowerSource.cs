namespace dev.kaldiroglu.Adapter.Electricity.ClassAdapter;

/// <summary>
/// <b>Target</b> participant for the electricity domain (matches the course slides): the interface
/// a Turkish appliance knows how to drive.
///
/// <para>This package is the self-contained <b>class-adapter</b> example. For the pluggable
/// (parameterized) technique in the same domain, see
/// dev.kaldiroglu.adapter.gof.pluggable.electricity.</para>
/// </summary>
public interface TurkishPowerSource
{
    void TurnOn();

    void TurnOff();
}
