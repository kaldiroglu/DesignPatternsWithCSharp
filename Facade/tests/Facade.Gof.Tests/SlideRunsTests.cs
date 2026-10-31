using System.Reflection;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace dev.kaldiroglu.Facade.Gof.Tests;

/// <summary>
/// Every runner entry named by a slide of this pattern: it must return normally and print
/// the lines the slide quotes. A slide that shows code and no figure gets the line that shows
/// the slide's claim. Timestamps are left out.
/// </summary>
public class SlideRunsTests
{
    [Theory]
    [InlineData("compile", "z = x + y - 1", "PUSH 3", "STORE z", "RETURN")]
    public void PrintsWhatTheSlideQuotes(string entry, params string[] lines)
    {
        var printed = Printed.By("Facade.Gof", entry);

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
