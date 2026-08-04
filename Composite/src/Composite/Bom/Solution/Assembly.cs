using dev.kaldiroglu.Composite.Bom.Domain;

namespace dev.kaldiroglu.Composite.Bom.Solution;

/// <summary>
/// Composite role of the Composite pattern — a sub-assembly or a finished
/// product.
/// </summary>
/// <remarks>
/// <para>
/// An assembly has its own cost (labor, fasteners, paint — whatever is spent
/// putting it together) and a list of <see cref="BomLine"/>s naming what goes
/// into it. Each of the <see cref="BomComponent"/> queries is implemented by
/// adding the assembly's own contribution to the sum over its lines; the lines
/// may point at parts or at other assemblies, so the recursion descends as far as
/// the product structure goes.
/// </para>
/// <para>
/// Three implementation issues from GoF's "Implementation" section (pp. 167–170)
/// show up here, and all three are driven by the same requirement — that the same
/// sub-assembly may be used in more than one place:
/// </para>
/// <list type="number">
///   <item><b>Sharing components (p. 167).</b> Quantity lives on the
///     <see cref="BomLine"/>, so one <c>Assembly</c> instance serves every parent
///     that needs it. Two wheels on a bicycle are one object referenced twice.</item>
///   <item><b>Caching to improve performance (p. 169).</b> Roll-ups are memoized,
///     because a costing screen asks for the same total repeatedly and a deep
///     product structure is expensive to walk.</item>
///   <item><b>Explicit parent references (p. 167).</b> A cache must be
///     invalidated when anything <em>below</em> it changes, which means a modified
///     node has to reach its parents. Because components are shared, that
///     reference is a <em>list</em> of parents, not a single one.</item>
/// </list>
/// </remarks>
public sealed class Assembly : BomComponent
{
    private readonly int _assemblyWeightGrams;
    private readonly List<BomLine> _lines = [];
    private readonly List<Assembly> _parents = [];

    // Memoized roll-ups; null means "not computed since the last change".
    private Money? _cachedCost;
    private int? _cachedWeightGrams;
    private int? _cachedPartCount;

    /// <summary>Creates an assembly.</summary>
    /// <param name="partNumber">The catalog identifier.</param>
    /// <param name="name">The human-readable name.</param>
    /// <param name="assemblyCost">
    /// What it costs to put this level together, excluding the components it contains.
    /// </param>
    /// <param name="assemblyWeightGrams">
    /// The mass this level adds itself, e.g. glue or weld, excluding its components.
    /// </param>
    public Assembly(string partNumber, string name, Money assemblyCost, int assemblyWeightGrams)
        : base(partNumber, name)
    {
        if (assemblyWeightGrams < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(assemblyWeightGrams), assemblyWeightGrams, "weight must not be negative");
        }

        AssemblyCost = assemblyCost;
        _assemblyWeightGrams = assemblyWeightGrams;
    }

    /// <summary>Creates an assembly that costs nothing extra to put together.</summary>
    public Assembly(string partNumber, string name)
        : this(partNumber, name, Money.Zero, 0)
    {
    }

    /// <summary>Creates an assembly from the shared reference data.</summary>
    public Assembly(Catalog.AssemblySpec spec)
        : this(spec.PartNumber, spec.Name, spec.AssemblyCost, spec.AssemblyWeightGrams)
    {
    }

    /// <summary>The cost of putting this level together, excluding its contents.</summary>
    public Money AssemblyCost { get; }

    /// <summary>The assemblies that directly contain this one, in insertion order.</summary>
    public IReadOnlyList<Assembly> Parents => _parents.AsReadOnly();

    // --- Child management: declared on the Composite, not on the Component ---

    /// <summary>
    /// Adds <paramref name="quantity"/> of <paramref name="component"/> to this
    /// assembly.
    /// </summary>
    /// <returns>This assembly, so lines can be chained while building a product.</returns>
    /// <exception cref="ArgumentException">
    /// The component is already a line here, or adding it would make the structure
    /// cyclic.
    /// </exception>
    public Assembly Add(BomComponent component, int quantity)
    {
        if (FindLine(component) is not null)
        {
            throw new ArgumentException(
                $"component {component.PartNumber} is already a line of {PartNumber}; "
                + "change its quantity instead");
        }

        if (ReferenceEquals(component, this) || component.ContainsDeep(this))
        {
            // Without this check a product could contain itself and every
            // roll-up would recurse forever.
            throw new ArgumentException(
                $"adding {component.PartNumber} to {PartNumber} would create a cycle "
                + "in the product structure");
        }

        _lines.Add(new BomLine(component, quantity));
        if (component is Assembly assembly)
        {
            assembly._parents.Add(this);
        }

        Invalidate();
        return this;
    }

    /// <summary>Adds exactly one of <paramref name="component"/> to this assembly.</summary>
    public Assembly Add(BomComponent component) => Add(component, 1);

    /// <summary>Removes <paramref name="component"/> from this assembly.</summary>
    /// <returns><c>true</c> if a line was removed.</returns>
    public bool Remove(BomComponent component)
    {
        var line = FindLine(component);
        if (line is null)
        {
            return false;
        }

        _lines.Remove(line);
        if (component is Assembly assembly)
        {
            assembly._parents.Remove(this);
        }

        Invalidate();
        return true;
    }

    /// <summary>
    /// Changes the quantity of an existing line — the everyday engineering change.
    /// </summary>
    /// <exception cref="ArgumentException">The component is not a line here.</exception>
    public void ChangeQuantity(BomComponent component, int newQuantity)
    {
        var line = FindLine(component);
        if (line is null)
        {
            throw new ArgumentException(
                $"{component.PartNumber} is not a line of {PartNumber}", nameof(component));
        }

        _lines[_lines.IndexOf(line)] = new BomLine(component, newQuantity);
        Invalidate();
    }

    // --- The roll-ups: own contribution plus the sum over the lines ----------

    public override Money TotalCost()
    {
        if (_cachedCost is null)
        {
            var total = AssemblyCost;
            foreach (var line in _lines)
            {
                total = total.Plus(line.ExtendedCost()); // recurses through the line
            }

            _cachedCost = total;
        }

        return _cachedCost.Value;
    }

    public override int TotalWeightGrams()
    {
        if (_cachedWeightGrams is null)
        {
            var total = _assemblyWeightGrams;
            foreach (var line in _lines)
            {
                total += line.ExtendedWeightGrams();
            }

            _cachedWeightGrams = total;
        }

        return _cachedWeightGrams.Value;
    }

    public override int PartCount()
    {
        if (_cachedPartCount is null)
        {
            var total = 0;
            foreach (var line in _lines)
            {
                total += line.ExtendedPartCount();
            }

            _cachedPartCount = total;
        }

        return _cachedPartCount.Value;
    }

    public override IReadOnlyList<BomLine> Lines => _lines.AsReadOnly();

    public override bool IsAssembly => true;

    // --- Cache maintenance --------------------------------------------------

    /// <summary>
    /// Discards this assembly's memoized roll-ups and those of every assembly
    /// above it.
    /// </summary>
    /// <remarks>
    /// This is the price of caching in a Composite: a change deep in the tree
    /// invalidates answers held higher up, so the change has to travel upwards.
    /// The cycle check in <see cref="Add(BomComponent, int)"/> is what guarantees
    /// this walk terminates.
    /// </remarks>
    private void Invalidate()
    {
        _cachedCost = null;
        _cachedWeightGrams = null;
        _cachedPartCount = null;
        foreach (var parent in _parents)
        {
            parent.Invalidate();
        }
    }

    private BomLine? FindLine(BomComponent component) =>
        _lines.FirstOrDefault(line => ReferenceEquals(line.Component, component));

    // --- Internal hook so the tests can observe the caching -----------------

    internal bool IsCostCached => _cachedCost is not null;
}
