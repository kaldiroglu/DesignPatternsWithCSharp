namespace dev.kaldiroglu.Observer.Gof.Solution;

/// <summary>
/// Two clocks observe a timer. After two ticks the analog clock is closed, and the third
/// tick draws only the digital clock. The timer knows neither clock by class.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Observer.Demo -- gof-solution</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var timer = new ClockTimer();
        var screen = new List<string>();
        new DigitalClock(timer, screen);
        var analog = new AnalogClock(timer, screen);
        Console.WriteLine("Observers: " + timer.ObserverCount);
        timer.Tick();
        timer.Tick();
        analog.Close();
        timer.Tick();
        foreach (string line in screen)
        {
            Console.WriteLine(line);
        }
        Console.WriteLine("Observers after the analog clock is closed: " + timer.ObserverCount);
    }
}
