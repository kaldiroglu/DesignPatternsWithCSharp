using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace dev.kaldiroglu.Strategy.Tests;

/// <summary>
/// Reads the C# sources of the library, for the tests that count lines of code rather than
/// types. The Java tests read their sources from a path relative to the Maven project; here the
/// path is fixed at compile time with <c>CallerFilePath</c>, so the working directory does not
/// matter.
/// </summary>
public static class SourceText
{
    /// <summary>The folder that holds the library's sources: <c>src/Strategy</c>.</summary>
    public static string SourceRoot =>
        Path.GetFullPath(Path.Combine(
            Path.GetDirectoryName(ThisFile())!, "..", "..", "src", "Strategy"));

    // CallerFilePath is filled in at the call site, so it must be read here, in this file.
    private static string ThisFile([CallerFilePath] string path = "") => path;

    /// <summary>The text of one source file, given its path under <c>src/Strategy</c>.</summary>
    public static string Read(string relativePath) =>
        File.ReadAllText(Path.Combine(SourceRoot, relativePath));

    /// <summary>
    /// The text with every comment removed. The classes document what they do not do, so a
    /// search over the raw file can match its own comments.
    /// </summary>
    public static string StripComments(string text)
    {
        var noBlocks = Regex.Replace(text, @"/\*.*?\*/", " ", RegexOptions.Singleline);
        return Regex.Replace(noBlocks, @"//[^\n]*", " ");
    }

    /// <summary>The text from the first occurrence of <paramref name="marker"/> to the end.</summary>
    public static string From(string text, string marker)
    {
        var start = text.IndexOf(marker, StringComparison.Ordinal);
        if (start < 0)
        {
            throw new InvalidOperationException("marker not found: " + marker);
        }
        return text[start..];
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
}
