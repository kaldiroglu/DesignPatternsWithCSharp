namespace dev.kaldiroglu.Observer.Gof.Solution;

/// <summary>
/// A <b>ConcreteSubject</b>: GoF's <c>ClockTimer</c>. It keeps the time and notifies its
/// observers on every tick. It knows nothing about clocks.
/// <para>
/// The observers pull what they need — hour, minute, second — after they are told something
/// changed. This is the pull model, from GoF implementation issue 6 (avoiding
/// observer-specific update protocols: the push and pull models).
/// </para>
/// </summary>
public sealed class ClockTimer : Subject
{
    private int seconds;

    public void Tick()
    {
        seconds++;
        NotifyObservers();
    }

    public int Hour => seconds / 3600;

    public int Minute => seconds / 60 % 60;

    public int Second => seconds % 60;
}
