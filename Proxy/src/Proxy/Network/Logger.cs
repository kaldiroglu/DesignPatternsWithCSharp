namespace dev.kaldiroglu.Proxy.Network;

/// <summary>
/// Records what the proxy was asked to do. Entries are kept in a list as well as printed,
/// which is what lets a test show that a refused request was still logged.
/// </summary>
public static class Logger
{
    private static readonly List<string> Recorded = [];

    public static void Log(string message)
    {
        Recorded.Add(message);
        Console.WriteLine($"[log] {message}");
    }

    public static IReadOnlyList<string> Entries() => Recorded.ToList();

    public static void Clear() => Recorded.Clear();
}
