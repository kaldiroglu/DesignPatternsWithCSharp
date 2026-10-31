using System.Reflection;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace dev.kaldiroglu.Facade.Notification.Tests;

/// <summary>
/// Every runner entry named by a slide of this pattern: it must return normally and print
/// the lines the slide quotes. A slide that shows code and no figure gets the line that shows
/// the slide's claim. Timestamps are left out.
/// </summary>
public class SlideRunsTests
{
    [Theory]
    [InlineData("notification-problem", "[EMAIL] To: alice@example.com | Subject: Order #ORD-9876 Confirmed", "[PUSH]  Token: dev_toke*** | Title: Order #ORD-9876 Confirmed")]
    [InlineData("notification", "Order ORD-9876 confirmed.", "NotificationResult{email=True, sms=True, slack=True, push=True}")]
    public void PrintsWhatTheSlideQuotes(string entry, params string[] lines)
    {
        var printed = Printed.By("Facade.Notification", entry);

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
