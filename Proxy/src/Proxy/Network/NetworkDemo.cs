namespace dev.kaldiroglu.Proxy.Network;

/// <summary>Three requests: one refused by the telnet rule, one by the ftp rule, one allowed.</summary>
public static class NetworkDemo
{
    public static void Run()
    {
        INetwork network = NetworkServer.GetInstance().GetNetwork();
        const string myIp = "10.0.0.2";
        Gateway gateway = Gateway.GetInstance();

        // The log and the counts are shared by the whole process, so a run starts them at zero.
        Logger.Clear();
        gateway.ResetCounts();

        Attempt(() => network.Telnet(myIp, "88.168.2.200"));   // refused: from 10.*
        Attempt(() => network.Ftp(myIp, "192.168.2.200"));     // refused: to 192.*
        Attempt(() => network.Ftp(myIp, "202.168.2.200"));     // allowed

        Console.WriteLine();
        Console.WriteLine($"requests logged by the proxy : {Logger.Entries().Count}");
        Console.WriteLine($"requests that reached the gateway : {gateway.FtpCalls() + gateway.TelnetCalls()}");
        Console.WriteLine("""

            Three requests logged, one forwarded. The two refused ones never
            reached the gateway — and the gateway contains no rule that could
            have refused them.

            Same shape as pm3.ProxyPM, in a domain with no people in it.
            """);
    }

    private static void Attempt(Action request)
    {
        try
        {
            request();
        }
        catch (ForbiddenAccessException e)
        {
            Console.WriteLine($"refused: {e.Message}");
        }
    }
}
