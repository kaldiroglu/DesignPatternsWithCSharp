using System.Reflection;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace dev.kaldiroglu.Composite.Tests;

/// <summary>
/// Every runner entry named by a slide of this pattern: it must return normally and print
/// the lines the slide quotes. A slide that shows code and no figure gets the line that shows
/// the slide's claim. Timestamps are left out.
/// </summary>
public class SlideRunsTests
{
    [Theory]
    [InlineData("bom-problem-right-answers", "Bicycle cost:   $396.00", "Bicycle weight: 5950 g", "Bicycle parts:  80")]
    [InlineData("bom-problem-wheel-drift", "Are the two wheels the same object? false", "Wheel 1 now costs $86.60, wheel 2 still costs $85.00")]
    [InlineData("gof-equipment", "The net price of the chassis is $440.00", "The net price of the cabinet is $530.00")]
    [InlineData("graphic-shape-count", "shapes on the display: 5", "shapes in the logo   : 2", "a single circle      : 1")]
    [InlineData("graphic-safety", "Drawing canvas: Logo\n", "\n5\n")]
    [InlineData("file-system-six-questions", "size of the whole tree : 170980 bytes", "biggest leaf           : ImportantReport.docx")]
    [InlineData("file-system-disk-report", "whole tree : 167,816 bytes", "one file   : 45,000 bytes")]
    [InlineData("file-system-cache", "three calls, nothing changed : 0", "after one file is added      : 3 — exactly Reports, Dev and akin")]
    [InlineData("bom-roll-ups", "City Bicycle           (assembly)         cost   $396.00     5950 g    80 part(s)", "14g Spoke              (purchased part)   cost     $0.40        5 g     1 part(s)")]
    [InlineData("bom-sharing", "Line 2 of the bicycle requires 2 x 700c Wheel", "Is it the same object as the catalog's wheel? true")]
    [InlineData("bom-new-item", "Bicycle cost $399.20 -> $413.20, weight unchanged at 5990 g, part count unchanged at 88.", "Shipping estimate is still $29.40.")]
    public void PrintsWhatTheSlideQuotes(string entry, params string[] lines)
    {
        var printed = Printed.By("Composite.Demo", entry);

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
