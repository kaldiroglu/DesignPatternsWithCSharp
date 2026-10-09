namespace dev.kaldiroglu.Observer.Gof.Problem;

/// <summary>
/// Two ticks of a timer that draws its own two clocks. A third clock, or closing one,
/// would be an edit to the timer.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Observer.Demo -- gof-problem</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var timer = new ClockTimer();
        timer.Tick();
        timer.Tick();
        foreach (string line in timer.Screen)
        {
            Console.WriteLine(line);
        }
    }
}
