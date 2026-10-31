namespace dev.kaldiroglu.Proxy.Network;

/// <summary>
/// The Subject: what any network endpoint offers. <see cref="Gateway"/> is the real thing
/// and <see cref="ProxyServer"/> stands in front of it. Because both implement this,
/// <see cref="NetworkServer"/> can hand out either and no client can tell.
/// </summary>
public interface INetwork
{
    /// <exception cref="ForbiddenAccessException">Thrown when the proxy refuses the request.</exception>
    void Telnet(string ip, string targetIp);

    /// <exception cref="ForbiddenAccessException">Thrown when the proxy refuses the request.</exception>
    void Ftp(string ip, string targetIp);
}
