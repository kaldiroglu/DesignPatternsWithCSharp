using System.Reflection;

namespace dev.kaldiroglu.Memento.Tests;

/// <summary>Reads the public methods of a class.</summary>
public static class Methods
{
    private const BindingFlags Declared =
        BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;

    /// <summary>
    /// The names of the public methods a class declares, sorted. A C# property gives a method
    /// for each accessor, named get_Name and set_Name, so a public property setter is listed
    /// as set_Name.
    /// </summary>
    public static List<string> PublicMethodsOf(Type type) =>
        type.GetMethods(Declared)
            .Select(m => m.Name)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToList();

    /// <summary>
    /// The names of the public setters a class declares: a public property setter (set_Name)
    /// or a public method whose name starts with "Set".
    /// </summary>
    public static List<string> PublicSettersOf(Type type) =>
        PublicMethodsOf(type)
            .Where(name => name.StartsWith("set_", StringComparison.Ordinal)
                           || name.StartsWith("Set", StringComparison.Ordinal))
            .ToList();
}
