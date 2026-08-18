using System.Reflection;
using System.Runtime.CompilerServices;

namespace dev.kaldiroglu.Bridge.Tests;

/// <summary>
/// Counts the types in a namespace, so the arithmetic on the slides fails when the code changes.
/// <para>
/// The Java suite walks the classpath directory for a package. The .NET equivalent is simpler,
/// because a namespace is a queryable property of every type in a loaded assembly — but the
/// point is the same one: listing the classes here and then asserting <c>2 + 3 == 5</c> would
/// prove something about integers, and would go on passing the day a fourth store is added and
/// the slide still says five.
/// </para>
/// </summary>
public static class TypeCensus
{
    /// <summary>
    /// Every type declared directly in this namespace, ignoring the compiler's own.
    /// <para>
    /// The anchor type is fully qualified with <c>global::</c> on purpose: the test namespaces
    /// mirror the library's, so a bare <c>Retrofit.IVendorClient</c> resolves to
    /// <c>Bridge.Tests.Retrofit</c> and does not compile.
    /// </para>
    /// </summary>
    public static IReadOnlyList<Type> In(string @namespace) =>
        typeof(global::dev.kaldiroglu.Bridge.Retrofit.IVendorClient).Assembly.GetTypes()
            .Where(t => t.Namespace == @namespace)
            .Where(t => !t.IsNested)
            .Where(t => !t.IsDefined(typeof(CompilerGeneratedAttribute), false))
            .OrderBy(t => t.Name)
            .ToList();

    /// <summary>Concrete types in this namespace assignable to <paramref name="root"/>.</summary>
    public static int ConcreteImplementationsOf(string @namespace, Type root) =>
        In(@namespace).Count(t => root.IsAssignableFrom(t) && t != root && !t.IsAbstract);

    /// <summary>A declared instance field, by name — the bridge reference, in every example.</summary>
    public static FieldInfo Field(Type type, string name) =>
        type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
        ?? throw new InvalidOperationException($"{type.Name} has no field {name}");

    /// <summary>
    /// Where this repository's C# sources live, anchored at compile time.
    /// <para>
    /// <c>CallerFilePath</c> is filled in at each <i>call site</i>, so it has to be captured
    /// here, in this file, rather than on a parameter of the public helper — a caller in a
    /// subfolder would otherwise anchor the path one directory too deep.
    /// </para>
    /// </summary>
    public static string SourceRoot =>
        Path.GetFullPath(Path.Combine(
            Path.GetDirectoryName(ThisFile())!, "..", "..", "src", "Bridge"));

    private static string ThisFile([CallerFilePath] string path = "") => path;

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
