namespace dev.kaldiroglu.Proxy.Network;

/// <summary>
/// Client scenario: obtains an <see cref="INetwork"/> from the server (which is
/// really a <see cref="ProxyServer"/>) and makes three requests — one allowed
/// and two blocked by the protection proxy.
/// </summary>
public static class NetworkDemo
{
    public static void Run()
    {
        NetworkServer networkServer = NetworkServer.GetInstance();
        INetwork network = networkServer.GetNetwork();

        const string myIp = "10.0.0.2";

        try
        {
            network.Telnet(myIp, "88.168.2.200");
        }
        catch (AccessDeniedException e)
        {
            Console.WriteLine(e.Message);
        }

        try
        {
            network.Ftp(myIp, "192.168.2.200");
        }
        catch (AccessDeniedException e)
        {
            Console.WriteLine(e.Message);
        }

        try
        {
            network.Ftp(myIp, "202.168.2.200");
        }
        catch (AccessDeniedException e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
