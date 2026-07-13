using dev.kaldiroglu.Proxy.Network;
using Xunit;

namespace dev.kaldiroglu.Proxy.Tests;

/// <summary>The protection proxy: <see cref="ProxyServer"/> enforces an access policy before delegating.</summary>
public class NetworkProxyTests
{
    [Fact(DisplayName = "The server hands the client a proxy, not the real gateway")]
    public void ServerReturnsProxy()
    {
        INetwork network = NetworkServer.GetInstance().GetNetwork();

        Assert.IsType<ProxyServer>(network);
    }

    [Fact(DisplayName = "FTP to a 192.* address is blocked by the proxy")]
    public void FtpTo192IsForbidden()
    {
        INetwork network = new ProxyServer();

        Assert.Throws<AccessDeniedException>(() => network.Ftp("10.0.0.2", "192.168.2.200"));
    }

    [Fact(DisplayName = "Telnet from a 10.* address is blocked by the proxy")]
    public void TelnetFrom10IsForbidden()
    {
        INetwork network = new ProxyServer();

        Assert.Throws<AccessDeniedException>(() => network.Telnet("10.0.0.2", "88.168.2.200"));
    }

    [Fact(DisplayName = "An allowed FTP is delegated to the real gateway without throwing")]
    public void AllowedFtpIsDelegated()
    {
        INetwork network = new ProxyServer();

        // 202.* is not blocked and the source is irrelevant for ftp filtering.
        Exception? ex = Record.Exception(() => network.Ftp("172.16.0.1", "202.168.2.200"));

        Assert.Null(ex);
    }
}
