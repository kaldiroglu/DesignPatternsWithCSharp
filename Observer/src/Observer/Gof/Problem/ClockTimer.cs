using System.Globalization;

namespace dev.kaldiroglu.Observer.Gof.Problem;

/// <summary>
/// Before the pattern: a timer that draws its own clocks.
/// <para>
/// GoF's sample code has a timer and two clocks that show its time. Here the timer
/// holds both clocks and draws them on every tick. A third clock is an edit to the timer, and
/// a clock cannot be closed while the timer runs.
/// </para>
/// </summary>
public sealed class ClockTimer
{
    private int seconds;
    private readonly List<string> screen = [];

    public void Tick()
    {
        seconds++;
        DrawDigital();
        DrawAnalog();
    }

    private void DrawDigital()
    {
        screen.Add(string.Format(CultureInfo.InvariantCulture, "digital {0:00}:{1:00}:{2:00}",
            seconds / 3600, seconds / 60 % 60, seconds % 60));
    }

    private void DrawAnalog()
    {
        screen.Add("analog second hand at " + (seconds % 60) * 6 + " degrees");
    }

    public IReadOnlyList<string> Screen => screen.ToList().AsReadOnly();
}
