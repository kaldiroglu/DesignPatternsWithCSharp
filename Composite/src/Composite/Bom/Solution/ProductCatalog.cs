using dev.kaldiroglu.Composite.Bom.Domain;

namespace dev.kaldiroglu.Composite.Bom.Solution;

/// <summary>
/// Builds the sample product structure used by the demo and the tests, from the
/// same figures in <see cref="Catalog"/> that <c>Problem.NaiveProductCatalog</c> uses.
/// </summary>
/// <remarks>
/// A city bicycle: a welded frame assembly, two identical wheel assemblies — each
/// with its own hub sub-assembly — and a saddle bought as a single part. The
/// structure is four levels deep and reuses one wheel object twice, which is what
/// makes it worth modelling with Composite rather than with a flat list.
/// <para>
/// Read this class beside <c>Problem.NaiveProductCatalog</c>. That one has to build
/// the wheel twice and hand back two wheels and two hubs; this one builds the wheel
/// once and says <c>.Add(wheel, 2)</c>.
/// </para>
/// </remarks>
public static class ProductCatalog
{
    /// <summary>
    /// The bicycle, and the shared sub-assemblies it is built from.
    /// </summary>
    /// <param name="Bicycle">The finished product, the root of the structure.</param>
    /// <param name="Frame">The frame sub-assembly.</param>
    /// <param name="Wheel">The wheel sub-assembly — <b>one</b> object used twice.</param>
    /// <param name="Hub">The hub sub-assembly, nested inside the wheel.</param>
    /// <param name="Spoke">A purchased part, exposed so tests can change its quantity.</param>
    public sealed record BicycleStructure(
        Assembly Bicycle,
        Assembly Frame,
        Assembly Wheel,
        Assembly Hub,
        Part Spoke);

    /// <summary>Assembles the sample bicycle from the ground up.</summary>
    public static BicycleStructure CityBicycle()
    {
        // --- Purchased parts (leaves) ---------------------------------------
        var rim = new Part(Catalog.Rim);
        var spoke = new Part(Catalog.Spoke);
        var axle = new Part(Catalog.Axle);
        var bearing = new Part(Catalog.Bearing);
        var tire = new Part(Catalog.Tire);
        var tube = new Part(Catalog.Tube);
        var saddle = new Part(Catalog.Saddle);
        var tubeset = new Part(Catalog.Tubeset);
        var fork = new Part(Catalog.Fork);
        var paint = new Part(Catalog.Paint);

        // --- Sub-assemblies (composites) ------------------------------------
        var hub = new Assembly(Catalog.Hub)
            .Add(axle)
            .Add(bearing, Catalog.BearingsPerHub);

        var wheel = new Assembly(Catalog.Wheel)
            .Add(rim)
            .Add(spoke, Catalog.SpokesPerWheel)  // one Part, one line, quantity 32
            .Add(hub)                            // an assembly inside an assembly
            .Add(tire)
            .Add(tube);

        var frame = new Assembly(Catalog.Frame)
            .Add(tubeset)
            .Add(fork)
            .Add(paint);

        // --- The finished product -------------------------------------------
        var bicycle = new Assembly(Catalog.Bicycle)
            .Add(frame)
            .Add(wheel, Catalog.WheelsPerBicycle)  // ONE wheel object, required twice
            .Add(saddle);

        return new BicycleStructure(bicycle, frame, wheel, hub, spoke);
    }
}
