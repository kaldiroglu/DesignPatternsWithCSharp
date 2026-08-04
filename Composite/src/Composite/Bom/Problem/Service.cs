using dev.kaldiroglu.Composite.Bom.Domain;

namespace dev.kaldiroglu.Composite.Bom.Problem;

/// <summary>
/// A third kind of item, added to the naive design after the fact: a
/// subcontracted operation such as powder coating.
/// </summary>
/// <remarks>
/// <para>
/// It costs money, but it adds no mass and it is not a part anybody can put on a
/// shelf. It is a perfectly reasonable thing for a bill of materials to contain,
/// and it is exactly the kind of requirement that arrives six months after a
/// design is settled.
/// </para>
/// <para>Watch what it costs the naive design:</para>
/// <list type="number">
///   <item><see cref="Assembly"/> cannot hold one. Its two collections are typed
///     <c>List&lt;Part&gt;</c> and <c>List&lt;Assembly&gt;</c>, so a third collection
///     has to be added — and then <em>every</em> traversal has to visit three lists
///     instead of two.</item>
///   <item><see cref="NaiveCosting"/> and <see cref="NaiveShipping"/> do not
///     recognize it, so each of their type chains needs a new branch. Until they get
///     one, they throw.</item>
///   <item>Any client written by a third party, which the authors of this code
///     cannot edit, is simply broken.</item>
/// </list>
/// <para>
/// Contrast <c>Solution.Service</c>: the same concept, fifteen lines, derives from
/// <c>BomComponent</c>, and every existing client handles it correctly on the day it
/// is written. That is GoF's third consequence (p. 166) as a difference you can run.
/// </para>
/// </remarks>
public class Service(string partNumber, string name, Money fee)
{
    /// <summary>Creates a service from the shared reference data.</summary>
    public Service(Catalog.ServiceSpec spec)
        : this(spec.PartNumber, spec.Name, spec.Fee)
    {
    }

    public string PartNumber { get; } = partNumber;

    public string Name { get; } = name;

    public Money Fee { get; } = fee;
}
