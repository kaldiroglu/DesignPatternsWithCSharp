namespace dev.kaldiroglu.Proxy.Network;

/// <summary>
/// Proxy role — a <i>protection proxy</i>. It implements the same
/// <see cref="INetwork"/> interface as the real <see cref="Gateway"/>, but
/// before forwarding a request it logs it and enforces an access policy.
/// The client cannot tell it is not talking to the real gateway.
/// </summary>
public class ProxyServer : INetwork
{
    private readonly INetwork _gateway;

    public ProxyServer()
    {
        _gateway = Gateway.GetInstance();
    }

    public void Ftp(string ip, string targetIp)
    {
        Logger.Log($"{ip} wants to make an ftp to {targetIp}");

        Filter("ftp", ip, targetIp);

        _gateway.Ftp(ip, targetIp);
    }

    public void Telnet(string ip, string targetIp)
    {
        Logger.Log($"{ip} wants to make a telnet to {targetIp}");

        Filter("telnet", ip, targetIp);

        _gateway.Telnet(ip, targetIp);
    }

    private static void Filter(string protocol, string ip, string targetIp)
    {
        if (protocol == "ftp" && targetIp.StartsWith("192"))
        {
            throw new AccessDeniedException($"FTP to {targetIp} is forbidden!");
        }

        if (protocol == "telnet" && ip.StartsWith("10"))
        {
            throw new AccessDeniedException($"Telnet from {ip} is forbidden!");
        }
    }
}
