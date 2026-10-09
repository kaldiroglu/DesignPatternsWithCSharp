using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace dev.kaldiroglu.State.Tests;

/// <summary>
/// Helper for the State tests, as in the Java suite: several examples print to the console,
/// and two tests read the source text of a class.
/// </summary>
public static class Printed
{
    /// <summary>Runs the action and returns what it printed, one line per element.</summary>
    public static List<string> By(Action action)
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

        // The same lines Java's String.lines() returns: no line ending, no empty last line.
        var lines = buffer.ToString().Split('\n').Select(line => line.TrimEnd('\r')).ToList();
        if (lines.Count > 0 && lines[^1].Length == 0)
        {
            lines.RemoveAt(lines.Count - 1);
        }
        return lines;
    }

    /// <summary>
    /// The text of a C# source file under <c>src/State</c>, with its comments removed.
    /// Well-commented code names what it leaves out, so a check on the raw text would match
    /// the comments.
    /// </summary>
    public static string CodeOf(string relativePath)
    {
        var text = File.ReadAllText(Path.Combine(SourceRoot, relativePath));
        text = Regex.Replace(text, @"/\*.*?\*/", " ", RegexOptions.Singleline);
        return Regex.Replace(text, @"//[^\n]*", " ");
    }

    /// <summary>How many times <paramref name="needle"/> occurs in <paramref name="text"/>.</summary>
    public static int CountOf(string text, string needle)
    {
        var count = 0;
        for (var i = text.IndexOf(needle, StringComparison.Ordinal); i >= 0;
             i = text.IndexOf(needle, i + needle.Length, StringComparison.Ordinal))
        {
            count++;
        }
        return count;
    }

    /// <summary>
    /// Where the library's C# sources live, found from this file's path at compile time.
    /// <c>CallerFilePath</c> is filled in at the call site, so it is read here, in this file.
    /// </summary>
    public static string SourceRoot =>
        Path.GetFullPath(Path.Combine(
            Path.GetDirectoryName(ThisFile())!, "..", "..", "src", "State"));

    private static string ThisFile([CallerFilePath] string path = "") => path;
}
