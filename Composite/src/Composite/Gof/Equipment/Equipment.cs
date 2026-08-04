using System.Collections;

namespace dev.kaldiroglu.Composite.Gof.Equipment;

/// <summary>
/// Component role of the Composite pattern — the <c>Equipment</c> class of the
/// GoF Sample Code (GoF, "Design Patterns", p. 170).
/// </summary>
/// <remarks>
/// <para>
/// Computer equipment such as a disk drive can be assembled into larger units: a
/// bus holds cards, a chassis holds buses and drives, a cabinet holds chassis.
/// This class is the common abstraction for both the simple pieces and the
/// assemblies, so a price or power query can be answered for a single card and
/// for a whole cabinet with the same call.
/// </para>
/// <para>
/// The book's operations are all here: <see cref="Power"/>, <see cref="NetPrice"/>,
/// <see cref="DiscountPrice"/>, the child operations <see cref="Add"/> /
/// <see cref="Remove"/>, and <c>CreateIterator</c> — expressed in C# by
/// implementing <see cref="IEnumerable{T}"/>, which is the language's own
/// Iterator pattern (GoF, p. 257; Iterator is the pattern GoF names for
/// traversing composites).
/// </para>
/// </remarks>
public abstract class Equipment(string name) : IEnumerable<Equipment>
{
    /// <summary>The equipment's name, e.g. "3.5in Floppy".</summary>
    public string Name { get; } = name;

    /// <summary>Power consumption in watts.</summary>
    public abstract int Power();

    /// <summary>The list price of this piece of equipment.</summary>
    public abstract Currency NetPrice();

    /// <summary>The price actually charged, after the applicable discount.</summary>
    public abstract Currency DiscountPrice();

    // --- Child management, declared in the Component (GoF, p. 168) -----------

    /// <summary>Adds a piece of equipment to this assembly.</summary>
    /// <exception cref="NotSupportedException">
    /// Thrown by default: a simple piece of equipment is not an assembly.
    /// <see cref="CompositeEquipment"/> overrides this.
    /// </exception>
    public virtual void Add(Equipment part) =>
        throw new NotSupportedException(
            $"{Name} is not an assembly and cannot contain other equipment");

    /// <summary>Removes a piece of equipment from this assembly.</summary>
    /// <exception cref="NotSupportedException">
    /// Thrown by default, for the same reason as <see cref="Add"/>.
    /// </exception>
    public virtual void Remove(Equipment part) =>
        throw new NotSupportedException(
            $"{Name} is not an assembly and cannot contain other equipment");

    /// <summary>
    /// Enumerates the contained equipment — empty for a simple piece.
    /// </summary>
    /// <remarks>
    /// An empty enumeration is the right default: it lets a client walk any
    /// <c>Equipment</c> uniformly without asking what kind it is.
    /// </remarks>
    public virtual IEnumerator<Equipment> GetEnumerator() =>
        Enumerable.Empty<Equipment>().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>Whether this equipment is an assembly that can hold parts.</summary>
    public virtual bool IsComposite => false;
}
