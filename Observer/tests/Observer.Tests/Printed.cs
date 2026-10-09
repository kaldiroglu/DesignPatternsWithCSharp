using System.Reflection;

namespace dev.kaldiroglu.Observer.Tests;

/// <summary>Helpers for the observer tests: several examples print to standard output.</summary>
public static class Printed
{
    /// <summary>
    /// Runs the action and returns what it printed, one line per element. Like Java's
    /// <c>String.lines()</c>, a final line break does not add an empty last line.
    /// </summary>
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

        var text = buffer.ToString().Replace("\r\n", "\n");
        if (text.Length == 0)
        {
            return [];
        }

        if (text.EndsWith('\n'))
        {
            text = text[..^1];
        }

        return text.Split('\n').ToList();
    }

    /// <summary>The types of every field a type declares, static or instance, of any access.</summary>
    public static List<Type> DeclaredFieldTypes(Type type) =>
        type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static
                       | BindingFlags.Public | BindingFlags.NonPublic)
            .Select(f => f.FieldType)
            .ToList();
}
