using dev.kaldiroglu.Proxy.Network;
using Xunit;

namespace dev.kaldiroglu.Proxy.Tests;

/// <summary>
/// A protection proxy earns its place by what it stops. These tests count what reached the
/// gateway, not what was printed.
/// </summary>
public class NetworkProxyTests
{
    private readonly Gateway _gateway;
    private readonly INetwork _network;

    public NetworkProxyTests()
    {
        _gateway = Gateway.GetInstance();
        _gateway.ResetCounts();
        Logger.Clear();
        _network = new ProxyServer(_gateway);
    }

    [Fact(DisplayName = "A permitted request reaches the gateway")]
    public void Permitted()
    {
        _network.Ftp("10.0.0.2", "202.168.2.200");

        Assert.Equal(1, _gateway.FtpCalls());
    }

    [Fact(DisplayName = "Ftp to a 192 address is refused, and never reaches the gateway")]
    public void FtpRuleStopsIt()
    {
        var thrown = Assert.Throws<ForbiddenAccessException>(() => _network.Ftp("10.0.0.2", "192.168.2.200"));

        Assert.Contains("not permitted", thrown.Message);
        Assert.Equal(0, _gateway.FtpCalls());
    }

    [Fact(DisplayName = "Telnet from a 10 address is refused, and never reaches the gateway")]
    public void TelnetRuleStopsIt()
    {
        Assert.Throws<ForbiddenAccessException>(() => _network.Telnet("10.0.0.2", "88.168.2.200"));

        Assert.Equal(0, _gateway.TelnetCalls());
    }

    [Fact(DisplayName = "A refused request is still logged: the proxy saw it")]
    public void RefusedRequestsAreLogged()
    {
        Assert.Throws<ForbiddenAccessException>(() => _network.Ftp("10.0.0.2", "192.168.2.200"));

        var entry = Assert.Single(Logger.Entries());
        Assert.Contains("wants to ftp", entry);
    }

    [Fact(DisplayName = "The gateway holds no access rule of its own")]
    public void TheRealSubjectKnowsNothing()
    {
        // Straight at the gateway, the forbidden request succeeds: the rule is in the proxy
        // and only in the proxy.
        _gateway.Ftp("10.0.0.2", "192.168.2.200");

        Assert.Equal(1, _gateway.FtpCalls());
    }

    [Fact(DisplayName = "Clients are handed the proxy, never the gateway")]
    public void TheServerHandsOutTheProxy()
    {
        INetwork handed = NetworkServer.GetInstance().GetNetwork();

        Assert.IsType<ProxyServer>(handed);
        Assert.True(typeof(INetwork).IsAssignableFrom(typeof(ProxyServer)));
        Assert.True(typeof(INetwork).IsAssignableFrom(typeof(Gateway)));
    }

    [Fact(DisplayName = "The telnet rule is about the 10 network: 100.* and 101.* are not in it")]
    public void AnAddressThatOnlyBeginsWithTheDigitsIsAllowed()
    {
        _network.Telnet("100.64.0.1", "88.168.2.200");
        _network.Telnet("101.2.3.4", "88.168.2.200");

        Assert.Equal(2, _gateway.TelnetCalls());
    }
}
