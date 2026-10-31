using System.Reflection;
using Xunit;

namespace dev.kaldiroglu.Bridge.Tests;

/// <summary>
/// Every runner entry named by a slide of this pattern: it must return normally and print
/// the lines the slide quotes. A slide that shows code and no figure gets the line that shows
/// the slide's claim. Timestamps are left out.
/// </summary>
public class SlideRunsTests
{
    [Theory]
    [InlineData("notifications-problem-stage-one", "3 kinds x 3 channels = 9 branches, in one method")]
    [InlineData("notifications-problem-stage-two", "3 kinds x 3 channels = 9 classes")]
    [InlineData("notifications-problem-stage-three", "Bora's stored preference : Sms", "the notification used    : email")]
    [InlineData("violation", "AType -> My variable: 42")]
    [InlineData("files-problem-stage-one", "FileNet    insurance!claims/2026/case-4021    kept 8, allowed 2")]
    [InlineData("files-combinations", "Evernote     finance keeps 5, live versions [4, 5, 6, 7, 8]", "FileNet      insurance keeps 2, live versions [7, 8]", "2 departments + 3 stores = 5 classes, 6 combinations.")]
    [InlineData("files-swap", "written on FileNet")]
    [InlineData("notifications-swap", "on the wire        : 1 email, 1 sms")]
    [InlineData("notifications-combinations", "9 messages, and every combination worked")]
    [InlineData("notifications-factory-preference", "Akin   prefers Email -> sent over email", "Bora   prefers Sms   -> sent over sms", "Ceyda  prefers Push  -> sent over push")]
    [InlineData("notifications-shared", "abstractions sharing the channel : 3", "messages through the one channel : 3", "transport connections opened     : 1")]
    public void PrintsWhatTheSlideQuotes(string entry, params string[] lines)
    {
        var printed = Printed.By("Bridge.Demo", entry);

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
