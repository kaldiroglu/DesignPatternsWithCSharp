using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace dev.kaldiroglu.TemplateMethod.Tests;

/// <summary>Reads the library's source files and its declared methods.</summary>
public static class Code
{
    /// <summary>
    /// The folder of the library's C# sources, found from this file's own path at compile
    /// time. <c>CallerFilePath</c> is filled in at the call site, so the path is taken here,
    /// in this file, and not from a parameter of a public method.
    /// </summary>
    public static string SourceRoot =>
        Path.GetFullPath(Path.Combine(
            Path.GetDirectoryName(ThisFile())!, "..", "..", "src", "TemplateMethod"));

    private static string ThisFile([CallerFilePath] string path = "") => path;

    /// <summary>
    /// The source of a file with its comments removed, so a comment cannot match a search.
    /// The path is relative to <see cref="SourceRoot"/>.
    /// </summary>
    public static string Of(string relativePath)
    {
        string text = File.ReadAllText(Path.Combine(SourceRoot, relativePath));
        text = Regex.Replace(text, @"/\*.*?\*/", "", RegexOptions.Singleline);
        return Regex.Replace(text, @"//[^\n]*", "");
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
    /// The names of the methods a type declares itself, of any access. Property accessors
    /// and methods the compiler adds are left out.
    /// </summary>
    public static HashSet<string> DeclaredMethodsOf(Type type) =>
        type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static
                        | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(m => !m.IsSpecialName)
            .Where(m => !m.IsDefined(typeof(CompilerGeneratedAttribute), false))
            .Select(m => m.Name)
            .ToHashSet();

    /// <summary>A method the type declares itself, of any access, by name.</summary>
    public static MethodInfo? Declared(Type type, string name) =>
        type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static
                        | BindingFlags.Public | BindingFlags.NonPublic)
            .FirstOrDefault(m => m.Name == name);
}
