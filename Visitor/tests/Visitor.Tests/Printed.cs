using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace dev.kaldiroglu.Visitor.Tests;

/// <summary>
/// Helpers for the tests: several examples print to the console, and some tests read the
/// C# source files.
/// </summary>
public static class Printed
{
    /// <summary>Runs the action and returns what it printed, one line per element.</summary>
    public static IReadOnlyList<string> By(Action action)
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

        // Like Java's String.lines(): a final line break does not make an empty last line.
        var text = buffer.ToString().Replace("\r\n", "\n");
        if (text.Length == 0)
        {
            return [];
        }

        if (text.EndsWith('\n'))
        {
            text = text[..^1];
        }

        return text.Split('\n');
    }

    /// <summary>
    /// Where the library's C# sources live, anchored at compile time.
    /// <para>
    /// <c>CallerFilePath</c> is filled in at the call site, so it is captured here, in this
    /// file, and not on a parameter of a public method.
    /// </para>
    /// </summary>
    public static string SourceRoot =>
        Path.GetFullPath(Path.Combine(
            Path.GetDirectoryName(ThisFile())!, "..", "..", "src", "Visitor"));

    private static string ThisFile([CallerFilePath] string path = "") => path;

    /// <summary>
    /// The text of a C# source file under <see cref="SourceRoot"/>, with its comments removed.
    /// A check that a file contains no X must not match a comment that names X.
    /// </summary>
    public static string CodeOf(string relativePath)
    {
        var text = System.IO.File.ReadAllText(Path.Combine(SourceRoot, relativePath));
        text = Regex.Replace(text, @"/\*.*?\*/", " ", RegexOptions.Singleline);
        return Regex.Replace(text, @"//[^\n]*", " ");
    }

    /// <summary>The code with its string literals removed, so printed text is not read as code.</summary>
    public static string WithoutStrings(string code) =>
        Regex.Replace(code, @"\$?""(?:[^""\\\n]|\\.)*""", "\"\"");

    /// <summary>How many times the pattern matches in the text.</summary>
    public static int CountOf(string text, string pattern) =>
        Regex.Matches(text, pattern, RegexOptions.Multiline).Count;

    /// <summary>True when the type is a C# record: the compiler gives every record a clone method.</summary>
    public static bool IsRecord(Type type) => type.GetMethod("<Clone>$") is not null;
}
