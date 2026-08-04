using dev.kaldiroglu.Composite.Bom.Domain;

namespace dev.kaldiroglu.Composite.Bom.Problem;

/// <summary>
/// Builds the same city bicycle as <c>Solution.ProductCatalog</c>, from the same
/// figures in <see cref="Catalog"/> — but in the naive design.
/// </summary>
/// <remarks>
/// <para>Two things are worth watching while reading <see cref="CityBicycle"/>:</para>
/// <list type="number">
///   <item><b>The wheel is built twice.</b> There is no way to say "two of these",
///     so the wheel factory is called once per wheel and the bicycle ends up holding
///     two separate objects that merely happen to match. Change one and the other
///     does not follow.</item>
///   <item><b>Thirty-two spokes are thirty-two list entries.</b> Every traversal, in
///     every client, walks all of them — 85 list entries for this bicycle, where the
///     Composite version walks 13.</item>
/// </list>
/// </remarks>
public static class NaiveProductCatalog
{
    /// <summary>
    /// The bicycle, and the interior objects the tests need to reach.
    /// </summary>
    /// <remarks>
    /// Note that this record has to expose <b>two</b> wheels and <b>two</b> hubs,
    /// where the Composite version exposes one of each. The duplication is not an
    /// accident of this class; it is forced by the design.
    /// </remarks>
    public sealed record BicycleStructure(
        Assembly Bicycle,
        Assembly Frame,
        Assembly Wheel1,
        Assembly Wheel2,
        Assembly Hub1,
        Assembly Hub2);

    /// <summary>Assembles the sample bicycle.</summary>
    public static BicycleStructure CityBicycle()
    {
        var frame = new Assembly(Catalog.Frame);
        frame.AddPart(new Part(Catalog.Tubeset));
        frame.AddPart(new Part(Catalog.Fork));
        frame.AddPart(new Part(Catalog.Paint));

        // The wheel has to be built once per wheel, because a quantity cannot be
        // expressed. These two objects are equal in every field and identical in no
        // way that the code can rely on.
        var wheel1 = BuildWheel();
        var wheel2 = BuildWheel();

        var bicycle = new Assembly(Catalog.Bicycle);
        bicycle.AddSubAssembly(frame);
        bicycle.AddSubAssembly(wheel1);
        bicycle.AddSubAssembly(wheel2);
        bicycle.AddPart(new Part(Catalog.Saddle));

        return new BicycleStructure(bicycle, frame, wheel1, wheel2,
            wheel1.SubAssemblies[0], wheel2.SubAssemblies[0]);
    }

    /// <summary>Builds one wheel, hub and all. Called once per wheel on the product.</summary>
    private static Assembly BuildWheel()
    {
        var hub = new Assembly(Catalog.Hub);
        hub.AddPart(new Part(Catalog.Axle));
        hub.AddPart(new Part(Catalog.Bearing), Catalog.BearingsPerHub);

        var wheel = new Assembly(Catalog.Wheel);
        wheel.AddPart(new Part(Catalog.Rim));
        // 32 entries in the list, walked by every client on every query.
        wheel.AddPart(new Part(Catalog.Spoke), Catalog.SpokesPerWheel);
        wheel.AddSubAssembly(hub);
        wheel.AddPart(new Part(Catalog.Tire));
        wheel.AddPart(new Part(Catalog.Tube));
        return wheel;
    }
}
