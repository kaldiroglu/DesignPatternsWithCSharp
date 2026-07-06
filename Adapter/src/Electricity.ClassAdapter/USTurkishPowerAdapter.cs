namespace dev.kaldiroglu.Adapter.Electricity.ClassAdapter;

/// <summary>
/// <b>Class adapter</b> (GoF p. 142): presents a <see cref="USPowerSource"/> as a
/// <see cref="TurkishPowerSource"/>.
///
/// <para>It <b>extends the adaptee</b> and <b>implements the target</b> &ndash; the single-inheritance
/// idiom. Note there is <b>no adaptee field and no delegation</b>: <c>TurnOn</c>/<c>TurnOff</c>
/// map the two-operation Turkish interface onto the adaptee's single toggle by calling the
/// <i>inherited</i> <see cref="USPowerSource.PushSwitch"/> and <see cref="USPowerSource.IsLive"/>.</para>
///
/// <para>Because the adapter <i>is-a</i> <c>USPowerSource</c>, it may also be used anywhere a
/// <c>USPowerSource</c> is expected, and it could override the adaptee's behaviour &ndash; two
/// abilities the object adapter does not have for free.</para>
/// </summary>
public class USTurkishPowerAdapter : USPowerSource, TurkishPowerSource
{
    public void TurnOn()
    {
        if (!IsLive())   // inherited from USPowerSource
        {
            PushSwitch();  // inherited from USPowerSource
        }
    }

    public void TurnOff()
    {
        if (IsLive())
        {
            PushSwitch();
        }
    }
}
