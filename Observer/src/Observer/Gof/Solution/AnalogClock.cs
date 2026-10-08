namespace dev.kaldiroglu.Observer.Gof.Solution;

/// <summary>A <b>ConcreteObserver</b>: GoF's <c>AnalogClock</c>. Same timer, different drawing.</summary>
public sealed class AnalogClock : IObserver
{
    private readonly ClockTimer timer;
    private readonly List<string> screen;

    public AnalogClock(ClockTimer timer, List<string> screen)
    {
        this.timer = timer;
        this.screen = screen;
        timer.Attach(this);
    }

    public void Update(Subject changed)
    {
        if (changed == timer)
        {
            screen.Add("analog second hand at " + timer.Second * 6 + " degrees");
        }
    }

    public void Close()
    {
        timer.Detach(this);
    }
}
