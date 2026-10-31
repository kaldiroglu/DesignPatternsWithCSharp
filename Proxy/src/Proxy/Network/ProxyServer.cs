namespace dev.kaldiroglu.Proxy.Network;

/// <summary>
/// The Proxy: a <i>protection proxy</i> (GoF, p. 208), which is to say a firewall.
/// It implements <see cref="INetwork"/> and holds an <see cref="INetwork"/>, so it can stand
/// in for the real gateway. Every request is logged, then checked, and only then forwarded.
/// A refused request never reaches the gateway at all.
/// This is the role <c>Pm.Pm3.ProxyPM</c> plays, in a domain with no people in it.
/// </summary>
public class ProxyServer : INetwork
{
    private readonly INetwork _gateway;

    public ProxyServer() : this(Gateway.GetInstance())
    {
    }

    public ProxyServer(INetwork gateway)
    {
        _gateway = gateway;
    }

    public void Ftp(string ip, string targetIp)
    {
        Logger.Log($"{ip} wants to ftp to {targetIp}");
        Filter("ftp", ip, targetIp);
        _gateway.Ftp(ip, targetIp);
    }

    public void Telnet(string ip, string targetIp)
    {
        Logger.Log($"{ip} wants to telnet to {targetIp}");
        Filter("telnet", ip, targetIp);
        _gateway.Telnet(ip, targetIp);
    }

    /// <summary>The access rules. The gateway knows nothing about them.</summary>
    private static void Filter(string protocol, string ip, string targetIp)
    {
        if (protocol == "ftp" && targetIp.StartsWith("192"))
        {
            throw new ForbiddenAccessException($"ftp to {targetIp} is not permitted");
        }

        if (protocol == "telnet" && ip.StartsWith("10."))
        {
            throw new ForbiddenAccessException($"telnet from {ip} is not permitted");
        }
    }
}
