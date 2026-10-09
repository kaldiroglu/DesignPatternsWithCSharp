using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace dev.kaldiroglu.Mediator.Tests;

/// <summary>Helpers for the tests of this pattern: capture what a run prints, and read source code.</summary>
public static class Printed
{
    /// <summary>Runs the action and returns what it printed, one line per element.</summary>
    public static IReadOnlyList<string> By(Action action)
    {
        TextWriter original = Console.Out;
        StringWriter buffer = new StringWriter();
        Console.SetOut(buffer);
        try
        {
            action();
        }
        finally
        {
            Console.SetOut(original);
        }
        return LinesOf(buffer.ToString());
    }

    /// <summary>Splits text into lines as Java's String.lines() does: no empty line after a final line break.</summary>
    public static IReadOnlyList<string> LinesOf(string text)
    {
        List<string> lines = text.Replace("\r\n", "\n").Split('\n').ToList();
        if (lines.Count > 0 && lines[^1].Length == 0)
        {
            lines.RemoveAt(lines.Count - 1);
        }
        return lines;
    }

    /// <summary>
    /// The source of a library file under src/Mediator, with every comment removed.
    /// The path is relative to that folder, for example "Gof/Problem/HelpDesk.cs".
    /// </summary>
    public static string CodeOf(string pathUnderSource)
    {
        string text = File.ReadAllText(Path.Combine(SourceRoot, pathUnderSource));
        text = Regex.Replace(text, @"/\*.*?\*/", "", RegexOptions.Singleline);
        return Regex.Replace(text, @"//[^\n]*", "");
    }

    /// <summary>How many times the needle occurs in the text.</summary>
    public static int CountOf(string text, string needle)
    {
        int count = 0;
        for (int i = text.IndexOf(needle, StringComparison.Ordinal); i >= 0;
             i = text.IndexOf(needle, i + needle.Length, StringComparison.Ordinal))
        {
            count++;
        }
        return count;
    }

    /// <summary>
    /// Where the library sources live. CallerFilePath is filled in at the call site, so it is
    /// captured here, in this file, and the path does not depend on the working directory.
    /// </summary>
    public static string SourceRoot =>
        Path.GetFullPath(Path.Combine(Path.GetDirectoryName(ThisFile())!, "..", "..", "src", "Mediator"));

    private static string ThisFile([CallerFilePath] string path = "") => path;
}
