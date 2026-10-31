using System.Reflection;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace dev.kaldiroglu.Proxy.Tests;

/// <summary>
/// Every runner entry named by a slide of this pattern: it must return normally and print
/// the lines the slide quotes. A slide that shows code and no figure gets the line that shows
/// the slide's claim. Timestamps are left out.
/// </summary>
public class SlideRunsTests
{
    [Theory]
    [InlineData("pm1", "problems heard by the PM herself : 2", "problems actually resolved       : 1")]
    [InlineData("pm2", "calls screened by the proxy : 2", "calls passed to the PM      : 1")]
    [InlineData("pm3-screening", "calls screened : 2", "passed on      : 1", "refused        : 1")]
    [InlineData("pm3-secretary", "Prime Minister created yet? False\nPrime Minister created yet? True")]
    [InlineData("pm4", "RealPM: the Prime Minister takes office.", "calls screened : 2", "passed on      : 1", "refused        : 1")]
    [InlineData("network", "refused: telnet from 10.0.0.2 is not permitted", "refused: ftp to 192.168.2.200 is not permitted", "requests logged by the proxy : 3", "requests that reached the gateway : 1")]
    [InlineData("gof", "Total page extent: 1024x1368", "[Image] Loading raster data from 'diagram.png' (expensive)...")]
    [InlineData("gof-twenty-figures", "figures in the document : 20", "total page height       : 5940", "images read from disk   : 0")]
    public void PrintsWhatTheSlideQuotes(string entry, params string[] lines)
    {
        var printed = Printed.By("Proxy.Demo", entry);

        foreach (var line in lines)
        {
            Assert.Contains(line, printed);
        }
    }
}

/// <summary>Runs a demo program in this process and returns what it printed.</summary>
internal static class Printed
{
    public static string By(string assembly, params string[] args)
    {
        var original = Console.Out;
        var buffer = new StringWriter();
        Console.SetOut(buffer);
        try
        {
            Assembly.Load(assembly).EntryPoint!.Invoke(null, [args]);
        }
        finally
        {
            Console.SetOut(original);
        }

        return buffer.ToString().Replace("\r\n", "\n");
    }
}
