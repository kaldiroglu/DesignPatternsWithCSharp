using dev.kaldiroglu.Composite.Bom.Domain;

namespace dev.kaldiroglu.Composite.Bom.Problem;

/// <summary>
/// The naive design's assembly — a plain class with <b>two separate child
/// collections</b> and no base type in common with <see cref="Part"/>.
/// </summary>
/// <remarks>
/// <para>
/// This is the design most developers reach for first, and it is worth
/// understanding <em>why</em> it is reached for: it is a direct transcription of
/// the sentence "an assembly contains parts and sub-assemblies". The trouble is
/// that the sentence is a description of the data, not of the behavior, and the
/// behavior is where the cost lands.
/// </para>
/// <para>Four consequences follow:</para>
/// <list type="number">
///   <item><b>Two collections must be kept in step.</b> Every traversal, in every
///     client, has to remember to visit both. Forget one and the answer is
///     silently wrong.</item>
///   <item><b>There is nowhere for a quantity to live.</b> Thirty-two spokes means
///     thirty-two entries in <see cref="Parts"/>; two wheels means two separately
///     built <c>Assembly</c> objects in <see cref="SubAssemblies"/>.</item>
///   <item><b>Therefore nothing can be shared.</b> The two wheels of a bicycle are
///     two <em>different</em> objects that merely happen to have been built the same
///     way, and they can drift apart without anyone noticing.</item>
///   <item><b>No operation can live here.</b> There is no type that spans parts and
///     assemblies, so <c>TotalCost</c> cannot be a method on the thing being costed.
///     It has to become a static function somewhere else — see
///     <see cref="NaiveCosting"/> — and it has to branch on type.</item>
/// </list>
/// <para>
/// Compare with <c>Solution.Assembly</c>, which has <em>one</em> child collection of
/// <c>BomLine</c>s and carries its own roll-up operations.
/// </para>
/// </remarks>
public class Assembly(string partNumber, string name, Money assemblyCost, int assemblyWeightGrams)
{
    // Two collections, because there is no type that covers both.
    private readonly List<Part> _parts = [];
    private readonly List<Assembly> _subAssemblies = [];

    /// <summary>Creates an assembly from the shared reference data.</summary>
    public Assembly(Catalog.AssemblySpec spec)
        : this(spec.PartNumber, spec.Name, spec.AssemblyCost, spec.AssemblyWeightGrams)
    {
    }

    public string PartNumber { get; } = partNumber;

    public string Name { get; } = name;

    public Money AssemblyCost { get; } = assemblyCost;

    public int AssemblyWeightGrams { get; } = assemblyWeightGrams;

    /// <summary>The purchased parts, as a <b>live, mutable list</b>.</summary>
    /// <remarks>
    /// Exposing the live list is itself a symptom. Because the operations that
    /// matter live outside this class, the class has no way to react to a change —
    /// so there is no point in defending the collection, and callers end up writing
    /// <c>assembly.Parts.Add(...)</c>.
    /// </remarks>
    public List<Part> Parts => _parts;

    /// <summary>The nested assemblies, as a live, mutable list, for the same reason.</summary>
    public List<Assembly> SubAssemblies => _subAssemblies;

    /// <summary>Adds a part.</summary>
    public void AddPart(Part part) => _parts.Add(part);

    /// <summary>
    /// Adds a part <paramref name="quantity"/> times, because quantity has nowhere
    /// else to live.
    /// </summary>
    public void AddPart(Part part, int quantity)
    {
        for (var i = 0; i < quantity; i++)
        {
            _parts.Add(part);
        }
    }

    /// <summary>Adds a nested assembly.</summary>
    public void AddSubAssembly(Assembly subAssembly) => _subAssemblies.Add(subAssembly);
}
