using dev.kaldiroglu.Proxy.Pm.Pm3;
using Xunit;

namespace dev.kaldiroglu.Proxy.Tests;

/// <summary>Stage 3 of the PM example: the finished GoF proxy sharing the <see cref="IPM"/> interface.</summary>
public class PmProxyTests
{
    [Fact(DisplayName = "The secretary hands out a ProxyPM typed as IPM")]
    public void SecretaryReturnsProxyTypedAsInterface()
    {
        IPM pm = new PMSecretary().GetMePM();

        Assert.IsType<ProxyPM>(pm);
    }

    [Fact(DisplayName = "Both RealPM and ProxyPM satisfy the IPM subject contract")]
    public void RealAndProxyShareTheSameInterface()
    {
        Assert.IsAssignableFrom<IPM>(new RealPM());
        Assert.IsAssignableFrom<IPM>(new ProxyPM(new RealPM()));
    }

    [Fact(DisplayName = "Listening through the proxy logs the proxy, then delegates to the RealPM")]
    public void ProxyLogsThenDelegatesToRealPM()
    {
        IPM pm = new ProxyPM(new RealPM());

        string output = CaptureConsole(() => pm.Listen("The problem is ..."));

        int proxyLine = output.IndexOf("Proxy: Listening", StringComparison.Ordinal);
        int realLine = output.IndexOf("RealPM: Listening", StringComparison.Ordinal);

        Assert.True(proxyLine >= 0, "proxy should announce itself");
        Assert.True(realLine >= 0, "the real PM should be reached");
        Assert.True(proxyLine < realLine, "the proxy runs before it delegates to the real PM");
    }

    private static string CaptureConsole(Action action)
    {
        TextWriter original = Console.Out;
        var buffer = new StringWriter();
        Console.SetOut(buffer);
        try
        {
            action();
        }
        finally
        {
            Console.SetOut(original);
        }

        return buffer.ToString();
    }
}
