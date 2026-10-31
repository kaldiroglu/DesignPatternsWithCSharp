using System.Reflection;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace dev.kaldiroglu.Decorator.Tests;

/// <summary>
/// Every runner entry named by a slide of this pattern: it must return normally and print
/// the lines the slide quotes. A slide that shows code and no figure gets the line that shows
/// the slide's claim. Timestamps are left out.
/// </summary>
public class SlideRunsTests
{
    [Theory]
    [InlineData("copypaste", "priceForOrder logged   3 lines (retry included)", "priceForReorder logged 2 lines")]
    [InlineData("flags", "5 booleans = 32 configurations")]
    [InlineData("flags-rate-limit", "fourth request  : refused", "limiter counted : 3", "supplier calls  : 9")]
    [InlineData("subclasses", "would take 325 classes")]
    [InlineData("gof-visual", "|hello^|", "|therev|", "|there|#", "||hello||")]
    [InlineData("gof-stream-sample-code", "file now holds   : 12 3a cafe")]
    [InlineData("classic", "supplier calls : 2", "log lines      : 4")]
    [InlineData("vendor", "vendor calls: 1 for 2 requests", "log lines   : 2")]
    [InlineData("functional", "supplier calls : 2")]
    [InlineData("fluent", "supplier calls : 2")]
    [InlineData("toast-problem-menu", "Cheese toast = 8", "Cheese sausage toast = 11")]
    [InlineData("toast-step-by-step", "bare bread          5", "+ cheddar cheese    8", "+ sucuk sausage     11", "+ tomato            13", "+ ketchup           14", "+ Russian salad     16")]
    [InlineData("toast-double-cheese", "Double cheese: 11")]
    [InlineData("toast-promotion-position", "outermost, so it discounts everything   12", "innermost, so it discounts only bread   14")]
    [InlineData("io", "total: $646.82  file: 274 bytes", "total: $646.82  file: 194 bytes", "Compression saved 80 bytes")]
    public void PrintsWhatTheSlideQuotes(string entry, params string[] lines)
    {
        var printed = Printed.By("Decorator.Demo", entry);

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
