using System.Reflection;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace dev.kaldiroglu.Adapter.Tests;

/// <summary>
/// Every runner entry named by a slide of this pattern: it must return normally and print
/// the lines the slide quotes. A slide that shows code and no figure gets the line that shows
/// the slide's claim. Timestamps are left out.
/// </summary>
public class SlideRunsTests
{
    [Theory]
    [InlineData("Electricity.Problem1", "", "TurkishHomeAppliance Shaver is starting!", "USPowerProvider started to provide electricity.")]
    [InlineData("Electricity.PowerAdapter1", "", "USTurkishPowerAdapter: Converting from USPowerSource to TurkishPowerSource", "USPowerProvider started to provide electricity.")]
    [InlineData("Electricity.ClassAdapter", "", "US source: switch pushed -> LIVE (110V/60Hz)", "after pushSwitch(); isLive=True")]
    [InlineData("Pluggable.Parameterized", "demo", "US source: switch pushed (110V / 60Hz)", "UK source: toggle flipped (230V / 50Hz)", "Kenyan source: switch is pushed")]
    public void PrintsWhatTheSlideQuotes(string program, string entry, params string[] lines)
    {
        var printed = Printed.By(program, entry);

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
