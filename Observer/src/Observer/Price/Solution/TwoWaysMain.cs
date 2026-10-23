namespace dev.kaldiroglu.Observer.Price.Solution;

/// <summary>
/// The same ten seconds of prices, read by stage three's polling readers and by listeners.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Observer.Demo -- price-two-ways</c>.
/// </remarks>
public static class TwoWaysMain
{
    public static void Run()
    {
        Console.WriteLine(TenSeconds.Polling());
        Console.WriteLine(TenSeconds.Listeners());
    }
}
