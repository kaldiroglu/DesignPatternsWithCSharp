namespace dev.kaldiroglu.Proxy.Network;

/// <summary>
/// RealSubject role: the real network gateway that actually performs the
/// connections. A singleton, mirroring a single shared piece of infrastructure.
/// </summary>
public class Gateway : INetwork
{
    private static readonly Gateway Instance = new();

    private Gateway()
    {
    }

    public void Ftp(string ip, string targetIp)
    {
        Console.WriteLine($"{ip} makes an ftp to {targetIp}\n");
    }

    public void Telnet(string ip, string targetIp)
    {
        Console.WriteLine($"{ip} makes a telnet to {targetIp}\n");
    }

    public static Gateway GetInstance() => Instance;
}
