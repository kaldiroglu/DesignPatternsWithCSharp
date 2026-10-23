namespace dev.kaldiroglu.Observer.Price.Solution;

/// <summary>
/// Three screens and a lambda listen to the feed. Every change reaches all of them, and the
/// alert at 105 fires.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Observer.Demo -- price-listeners</c>.
/// </remarks>
public static class ListenersMain
{
    public static void Run() => Console.WriteLine(TenSeconds.Listeners());
}
