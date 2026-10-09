namespace dev.kaldiroglu.TemplateMethod.Tests;

/// <summary>Runs code that prints to the console and returns what it printed.</summary>
public static class Printed
{
    /// <summary>
    /// Runs the action and returns what it printed, one line per element. A final line break
    /// does not add an empty line at the end, as with Java's <c>String.lines()</c>.
    /// </summary>
    public static List<string> By(Action action)
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
        return Lines(buffer.ToString());
    }

    private static List<string> Lines(string text)
    {
        var lines = text.Split('\n').Select(line => line.TrimEnd('\r')).ToList();
        if (lines.Count > 0 && lines[^1].Length == 0)
        {
            lines.RemoveAt(lines.Count - 1);
        }
        return lines;
    }
}
