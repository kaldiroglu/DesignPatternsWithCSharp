using System.Reflection;

namespace dev.kaldiroglu.Mediator.Tests;

/// <summary>Reads which types a class holds references to: its field types, and the element types of its lists.</summary>
public static class Fields
{
    /// <summary>
    /// The types a class's instance fields refer to, including the type inside a generic field.
    /// Only fields declared in the class itself are read, as Java's getDeclaredFields does.
    /// The backing field of an auto-property is a real field, so it is included.
    /// </summary>
    public static List<Type> HeldBy(Type type)
    {
        List<Type> held = [];
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                                   | BindingFlags.DeclaredOnly;
        foreach (FieldInfo field in type.GetFields(flags))
        {
            held.Add(field.FieldType);
            if (field.FieldType.IsGenericType)
            {
                // Java keeps only the type arguments that are plain classes; a nested generic
                // type such as HashSet<string> is left out in the same way here.
                held.AddRange(field.FieldType.GetGenericArguments().Where(t => !t.IsGenericType));
            }
        }
        return held;
    }

    /// <summary>True if the class holds a reference to any of the given types.</summary>
    public static bool HoldsAny(Type type, IEnumerable<Type> others)
    {
        List<Type> list = others.ToList();
        return HeldBy(type).Any(held => list.Any(o => o.IsAssignableFrom(held)));
    }
}
