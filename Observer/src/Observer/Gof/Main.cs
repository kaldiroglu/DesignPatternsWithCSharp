using dev.kaldiroglu.Observer.Gof.Solution;

namespace dev.kaldiroglu.Observer.Gof;

/// <summary>Two ticks with both clocks, then the analog clock is closed and the timer ticks once more.</summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Observer.Demo -- gof</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var before = new Problem.ClockTimer();
        before.Tick();
        before.Tick();
        Console.WriteLine("Timer draws its clocks: " + Show(before.Screen));

        var timer = new ClockTimer();
        var screen = new List<string>();
        new DigitalClock(timer, screen);
        var analog = new AnalogClock(timer, screen);
        timer.Tick();
        timer.Tick();
        analog.Close();
        timer.Tick();
        Console.WriteLine("Clocks observe the timer: " + Show(screen));
        Console.WriteLine("Observers left: " + timer.ObserverCount);
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
