namespace dev.kaldiroglu.Proxy.Network;

/// <summary>
/// Subject role of the Proxy pattern. Both the real <see cref="Gateway"/> and
/// the access-controlling <see cref="ProxyServer"/> implement this interface, so
/// a client uses them interchangeably.
/// </summary>
public interface INetwork
{
    /// <exception cref="AccessDeniedException">Thrown when the proxy forbids the connection.</exception>
    void Telnet(string ip, string targetIp);

    /// <exception cref="AccessDeniedException">Thrown when the proxy forbids the connection.</exception>
    void Ftp(string ip, string targetIp);
}
