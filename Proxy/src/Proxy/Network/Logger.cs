namespace dev.kaldiroglu.Proxy.Network;

/// <summary>Minimal console logger used by the protection proxy to record every request.</summary>
public static class Logger
{
    public static void Log(string message)
    {
        Console.WriteLine($"\n{DateTime.Now}: {message}");
    }
}
