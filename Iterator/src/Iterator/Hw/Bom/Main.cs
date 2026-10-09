using dev.kaldiroglu.Iterator.Hw.Bom.Composite;

namespace dev.kaldiroglu.Iterator.Hw.Bom;

/// <summary>
/// Walks a city bicycle's bill of materials and lists the parts only, with quantities
/// multiplied down the tree.
/// </summary>
/// <remarks>
/// <para>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Iterator.Demo -- hw-bom</c>.
/// </para>
/// <para>
/// Java takes the bicycle from <c>ProductCatalog.cityBicycle()</c> in the Composite package,
/// built with 32 spokes per wheel, and then calls <c>wheel.changeQuantity(spoke, 36)</c>. The
/// C# port copies only the bill-of-materials types, with no catalog and no
/// <c>ChangeQuantity</c>, so the same tree is built here with 36 spokes from the start, as the
/// homework tests do.
/// </para>
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var bicycle = CityBicycle(spokesPerWheel: 36);

        Console.WriteLine("Parts of a city bicycle, with 36 spokes in each of its two wheels:");
        foreach (var line in PartIterator.PartsOf(bicycle))
        {
            Console.WriteLine("  " + line);
        }
        Console.WriteLine("Assemblies such as the wheel are walked through, not listed.");
    }

    /// <summary>The city bicycle of the Composite deck, with the same part numbers, names and quantities.</summary>
    private static Assembly CityBicycle(int spokesPerWheel)
    {
        var rim = new Part("RIM-700C", "700c Rim", Money.Of(24.00m), 850);
        var spoke = new Part("SPOKE-14G", "14g Spoke", Money.Of(0.40m), 5);
        var axle = new Part("AXLE-QR", "Quick-release Axle", Money.Of(6.50m), 120);
        var bearing = new Part("BEARING-6001", "6001 Sealed Bearing", Money.Of(2.10m), 15);
        var tire = new Part("TIRE-700x25", "700x25 Tire", Money.Of(18.00m), 260);
        var tube = new Part("TUBE-700", "700c Inner Tube", Money.Of(4.50m), 95);
        var saddle = new Part("SADDLE-CR", "Cromoly Saddle", Money.Of(18.00m), 310);
        var tubeset = new Part("TUBESET-CR", "Cromoly Tubeset", Money.Of(95.00m), 1800);
        var fork = new Part("FORK-CR", "Cromoly Fork", Money.Of(42.00m), 700);
        var paint = new Part("PAINT-KIT", "Paint & Decals", Money.Of(6.00m), 40);

        var hub = new Assembly("HUB-ASM", "Wheel Hub")
            .Add(axle)
            .Add(bearing, 2);
        var wheel = new Assembly("WHEEL-ASM", "700c Wheel")
            .Add(rim)
            .Add(spoke, spokesPerWheel)
            .Add(hub)
            .Add(tire)
            .Add(tube);
        var frame = new Assembly("FRAME-ASM", "Frame Assembly")
            .Add(tubeset)
            .Add(fork)
            .Add(paint);
        return new Assembly("BIKE-CITY", "City Bicycle")
            .Add(frame)
            .Add(wheel, 2)
            .Add(saddle);
    }
}
