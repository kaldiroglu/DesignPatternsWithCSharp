namespace dev.kaldiroglu.Adapter.Pluggable.Parameterized;

/// <summary>
/// <b>Pluggable adapter</b>, parameterized form &ndash; GoF technique (c), p. 143.
///
/// <para>One adapter class adapts <i>any</i> foreign source to <see cref="TurkishPowerSource"/>. The narrow
/// interface is just "turn on" / "turn off", supplied as <see cref="System.Action"/> blocks. This replaces the
/// family <c>USTurkishPowerAdapter</c>, <c>UKTurkishPowerAdapter</c>, &hellip; with a single
/// parameterizable class &ndash; the answer to the deck's "one adapter per interface" consequence.</para>
/// </summary>
public sealed class PluggablePowerAdapter : TurkishPowerSource
{
    private readonly Action onTurnOn;
    private readonly Action onTurnOff;

    public PluggablePowerAdapter(Action onTurnOn, Action onTurnOff)
    {
        this.onTurnOn = onTurnOn;
        this.onTurnOff = onTurnOff;
    }

    public void TurnOn()
    {
        onTurnOn();
    }

    public void TurnOff()
    {
        onTurnOff();
    }
}
