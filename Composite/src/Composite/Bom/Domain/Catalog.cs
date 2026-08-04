namespace dev.kaldiroglu.Composite.Bom.Domain;

/// <summary>
/// The engineering reference data for the sample product — the numbers, and
/// nothing else.
/// </summary>
/// <remarks>
/// <para>
/// Both the naive design in <c>..Bom.Problem</c> and the Composite design in
/// <c>..Bom.Solution</c> build their bicycle from these constants. That is
/// deliberate: it makes the two designs provably comparable. If the naive bicycle
/// and the Composite bicycle ever disagreed about a total, the difference would be
/// in the <em>design</em>, not in the data.
/// </para>
/// <para>
/// Notice that this class contains no structure at all — only figures. How the
/// parts nest, and how the nesting is traversed, is exactly what the two
/// namespaces disagree about.
/// </para>
/// </remarks>
public static class Catalog
{
    /// <summary>
    /// A purchased part: something bought from a supplier and not broken down
    /// further.
    /// </summary>
    /// <param name="PartNumber">The catalog identifier.</param>
    /// <param name="Name">The human-readable name.</param>
    /// <param name="UnitCost">The supplier's price for one.</param>
    /// <param name="WeightGrams">The mass of one, in grams.</param>
    public sealed record PartSpec(string PartNumber, string Name, Money UnitCost, int WeightGrams);

    /// <summary>An assembly: something the factory builds from other items.</summary>
    /// <param name="PartNumber">The catalog identifier.</param>
    /// <param name="Name">The human-readable name.</param>
    /// <param name="AssemblyCost">Labor, fasteners and paint for this level alone.</param>
    /// <param name="AssemblyWeightGrams">The mass this level adds itself, e.g. weld.</param>
    public sealed record AssemblySpec(
        string PartNumber, string Name, Money AssemblyCost, int AssemblyWeightGrams);

    /// <summary>
    /// A subcontracted operation: it costs money, but it adds no mass and it is
    /// not a part anyone can put on a shelf.
    /// </summary>
    /// <remarks>
    /// This is the "new kind of component" both namespaces are asked to absorb.
    /// See <c>Solution.Service</c> and <c>Problem.Service</c>.
    /// </remarks>
    /// <param name="PartNumber">The catalog identifier.</param>
    /// <param name="Name">The human-readable name.</param>
    /// <param name="Fee">What the subcontractor charges.</param>
    public sealed record ServiceSpec(string PartNumber, string Name, Money Fee);

    // --- Purchased parts ----------------------------------------------------

    public static readonly PartSpec Rim =
        new("RIM-700C", "700c Rim", Money.Of(24.00m), 850);
    public static readonly PartSpec Spoke =
        new("SPOKE-14G", "14g Spoke", Money.Of(0.40m), 5);
    public static readonly PartSpec Axle =
        new("AXLE-QR", "Quick-release Axle", Money.Of(6.50m), 120);
    public static readonly PartSpec Bearing =
        new("BEARING-6001", "6001 Sealed Bearing", Money.Of(2.10m), 15);
    public static readonly PartSpec Tire =
        new("TIRE-700x25", "700x25 Tire", Money.Of(18.00m), 260);
    public static readonly PartSpec Tube =
        new("TUBE-700", "700c Inner Tube", Money.Of(4.50m), 95);
    public static readonly PartSpec Saddle =
        new("SADDLE-CR", "Cromoly Saddle", Money.Of(18.00m), 310);
    public static readonly PartSpec Tubeset =
        new("TUBESET-CR", "Cromoly Tubeset", Money.Of(95.00m), 1800);
    public static readonly PartSpec Fork =
        new("FORK-CR", "Cromoly Fork", Money.Of(42.00m), 700);
    public static readonly PartSpec Paint =
        new("PAINT-KIT", "Paint & Decals", Money.Of(6.00m), 40);

    // --- Assemblies ---------------------------------------------------------

    public static readonly AssemblySpec Hub =
        new("HUB-ASM", "Wheel Hub", Money.Of(3.00m), 20);
    public static readonly AssemblySpec Wheel =
        new("WHEEL-ASM", "700c Wheel", Money.Of(12.00m), 0);
    public static readonly AssemblySpec Frame =
        new("FRAME-ASM", "Frame Assembly", Money.Of(25.00m), 30);
    public static readonly AssemblySpec Bicycle =
        new("BIKE-CITY", "City Bicycle", Money.Of(40.00m), 0);

    // --- Subcontracted operations -------------------------------------------

    public static readonly ServiceSpec PowderCoating =
        new("SVC-COAT", "Powder Coating", Money.Of(14.00m));

    // --- Quantities ---------------------------------------------------------

    public const int SpokesPerWheel = 32;
    public const int BearingsPerHub = 2;
    public const int WheelsPerBicycle = 2;
}
