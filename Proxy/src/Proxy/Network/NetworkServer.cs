namespace dev.kaldiroglu.Proxy.Network;

/// <summary>
/// Hands the client an <see cref="INetwork"/> — but always the protection
/// <see cref="ProxyServer"/>, never the real <see cref="Gateway"/> directly.
/// This is how the proxy is transparently substituted for the real subject.
/// </summary>
public class NetworkServer
{
    private static readonly NetworkServer Instance = new();
    private readonly INetwork _network;

    private NetworkServer()
    {
        _network = new ProxyServer();
    }

    public INetwork GetNetwork() => _network;

    public static NetworkServer GetInstance() => Instance;
}
