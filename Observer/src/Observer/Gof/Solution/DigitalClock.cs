using System.Globalization;

namespace dev.kaldiroglu.Observer.Gof.Solution;

/// <summary>
/// A <b>ConcreteObserver</b>: GoF's <c>DigitalClock</c>. It attaches itself when it is made
/// and detaches when it is closed, so the timer never holds a closed clock. GoF
/// implementation issue 4 (dangling references to deleted subjects) is the other side of
/// the same rule.
/// </summary>
public sealed class DigitalClock : IObserver
{
    private readonly ClockTimer timer;
    private readonly List<string> screen;

    public DigitalClock(ClockTimer timer, List<string> screen)
    {
        this.timer = timer;
        this.screen = screen;
        timer.Attach(this);
    }

    public void Update(Subject changed)
    {
        if (changed == timer)
        {
            screen.Add(string.Format(CultureInfo.InvariantCulture, "digital {0:00}:{1:00}:{2:00}",
                timer.Hour, timer.Minute, timer.Second));
        }
    }

    public void Close()
    {
        timer.Detach(this);
    }
}
