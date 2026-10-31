namespace dev.kaldiroglu.Proxy.Network;

/// <summary>
/// The RealSubject: the gateway that actually moves the traffic.
/// It has no idea it is being protected, and no access rule appears anywhere in it.
/// </summary>
public class Gateway : INetwork
{
    private static readonly Gateway Instance = new();

    private int _ftpCalls;
    private int _telnetCalls;

    private Gateway()
    {
    }

    public static Gateway GetInstance() => Instance;

    public void Ftp(string ip, string targetIp)
    {
        _ftpCalls++;
        Console.WriteLine($"{ip} makes an ftp to {targetIp}");
    }

    public void Telnet(string ip, string targetIp)
    {
        _telnetCalls++;
        Console.WriteLine($"{ip} makes a telnet to {targetIp}");
    }

    public int FtpCalls() => _ftpCalls;

    public int TelnetCalls() => _telnetCalls;

    public void ResetCounts()
    {
        _ftpCalls = 0;
        _telnetCalls = 0;
    }
}
