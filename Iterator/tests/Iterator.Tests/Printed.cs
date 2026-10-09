namespace dev.kaldiroglu.Iterator.Tests;

/// <summary>
/// Helpers for the tests of examples that print to the console. The Java tests use the
/// <c>command.lender.Printed</c> helper for the same job.
/// </summary>
public static class Printed
{
    /// <summary>Runs the action and returns what it printed, one line per element.</summary>
    public static IReadOnlyList<string> By(Action action)
    {
        var original = Console.Out;
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
        return Lines(buffer.ToString());
    }

    /// <summary>
    /// Splits text into lines the way Java's <c>String.lines()</c> does: a line ends at
    /// "\n", "\r" or "\r\n", and a line terminator at the very end does not start one more,
    /// empty line.
    /// </summary>
    public static IReadOnlyList<string> Lines(string text)
    {
        var lines = text.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n').ToList();
        if (lines.Count > 0 && lines[^1].Length == 0)
        {
            lines.RemoveAt(lines.Count - 1);
        }
        return lines;
    }
}
