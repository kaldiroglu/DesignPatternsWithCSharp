namespace dev.kaldiroglu.Proxy.Network;

/// <summary>
/// How a client obtains an <see cref="INetwork"/>, and why it can never obtain the real one.
/// The server hands out a <see cref="ProxyServer"/>. The returned type is
/// <see cref="INetwork"/>, so no client can tell, and none is ever given a reference to the
/// <see cref="Gateway"/>.
/// </summary>
public class NetworkServer
{
    private static readonly NetworkServer Instance = new();
    private readonly INetwork _network;

    private NetworkServer()
    {
        _network = new ProxyServer();
    }

    public static NetworkServer GetInstance() => Instance;

    public INetwork GetNetwork() => _network;
}
