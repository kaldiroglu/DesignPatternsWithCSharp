using dev.kaldiroglu.Composite.Bom.Domain;

namespace dev.kaldiroglu.Composite.Bom.Solution;

/// <summary>
/// Client of the bill-of-materials Composite.
/// </summary>
/// <remarks>
/// <para>
/// Every method below prints one slide's output, and each is a question the business asks, answered by one call against
/// <see cref="BomComponent"/>. Nowhere does this client test whether it is holding a
/// part or an assembly.
/// </para>
/// <para>
/// <c>Problem.ProblemDemo</c> builds the same bicycle from the same
/// figures and reaches the same totals — the difference this demo is meant to show is
/// not in the answers, it is in what the code had to do to get them.
/// </para>
/// </remarks>
public static class SolutionDemo
{
    public static void ProductStructure()
    {
        Console.WriteLine("=== The product structure ===");
        Console.Write(ProductCatalog.CityBicycle().Bicycle.ToTree());
    }

    public static void RollUps()
    {
        var catalog = ProductCatalog.CityBicycle();

        Console.WriteLine("=== Roll-ups: one call each, at any level ===");
        Report(catalog.Bicycle);
        Report(catalog.Wheel);
        Report(catalog.Hub);
        Report(catalog.Spoke); // a leaf answers the same three questions
    }

    public static void Sharing()
    {
        var catalog = ProductCatalog.CityBicycle();
        var bicycle = catalog.Bicycle;

        Console.WriteLine("=== Sharing: two wheels, one object ===");
        var firstWheel = bicycle.Lines[1].Component;
        Console.WriteLine($"Line 2 of the bicycle requires {bicycle.Lines[1].Quantity} "
                          + $"x {firstWheel.Name}");
        Console.WriteLine("Is it the same object as the catalog's wheel? "
                          + (ReferenceEquals(firstWheel, catalog.Wheel) ? "true" : "false"));
    }

    public static void EngineeringChange()
    {
        var catalog = ProductCatalog.CityBicycle();
        var bicycle = catalog.Bicycle;

        Console.WriteLine("=== An engineering change deep in the tree ===");
        Console.WriteLine($"Before: bicycle costs {bicycle.TotalCost()} "
                          + $"and weighs {bicycle.TotalWeightGrams()} g");
        catalog.Wheel.ChangeQuantity(catalog.Spoke, 36); // 32 -> 36 spokes per wheel
        Console.WriteLine("Change: 36 spokes per wheel instead of 32");
        Console.WriteLine($"After:  bicycle costs {bicycle.TotalCost()} "
                          + $"and weighs {bicycle.TotalWeightGrams()} g");
        Console.WriteLine("Both wheels changed, and the roll-up above them "
                          + "was recomputed automatically.");
    }

    public static void ShippingEstimate()
    {
        var catalog = ProductCatalog.CityBicycle();
        // The estimate is for the bicycle after the engineering change: 36 spokes per wheel.
        catalog.Wheel.ChangeQuantity(catalog.Spoke, 36);

        Console.WriteLine("=== A client that does not know or care about the type ===");
        Console.WriteLine($"Shipping estimate for the bicycle: {EstimateShipping(catalog.Bicycle)}");
        Console.WriteLine($"Shipping estimate for a single spoke: {EstimateShipping(catalog.Spoke)}");
    }

    public static void NewItem()
    {
        var catalog = ProductCatalog.CityBicycle();
        var bicycle = catalog.Bicycle;
        // The figures are for the bicycle after the engineering change: 36 spokes per wheel.
        catalog.Wheel.ChangeQuantity(catalog.Spoke, 36);

        Console.WriteLine("=== A new kind of item costs one class and no client edits ===");
        var coating = new Service(Catalog.PowderCoating);
        var costBeforeCoating = bicycle.TotalCost();
        catalog.Frame.Add(coating);
        Console.WriteLine($"Added {coating.Name} ({coating.TotalCost()}) to the frame.");
        Console.WriteLine($"Bicycle cost {costBeforeCoating} -> {bicycle.TotalCost()}, "
                          + $"weight unchanged at {bicycle.TotalWeightGrams()} g, "
                          + $"part count unchanged at {bicycle.PartCount()}.");
        Console.WriteLine("The tree printer and the shipping estimator below were never "
                          + "told this class exists:");
        Console.WriteLine($"Shipping estimate is still {EstimateShipping(bicycle)}.");
    }

    public static void Acyclic()
    {
        var catalog = ProductCatalog.CityBicycle();

        Console.WriteLine("=== The structure is kept acyclic ===");
        try
        {
            catalog.Wheel.Add(catalog.Bicycle); // a wheel that contains the whole bicycle
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Rejected as expected: {e.Message}");
        }
    }

    /// <summary>Prints the three roll-ups for any component whatsoever.</summary>
    private static void Report(BomComponent component)
    {
        var kind = component.IsAssembly ? "(assembly)" : "(purchased part)";
        Console.WriteLine($"{component.Name,-22} {kind,-18} cost {component.TotalCost(),9}"
                          + $"   {component.TotalWeightGrams(),6} g   {component.PartCount(),3} part(s)");
    }

    /// <summary>
    /// A second, unrelated client. It was written against <see cref="BomComponent"/>
    /// and therefore works for a whole bicycle and for a single spoke, with no changes
    /// and no type tests.
    /// </summary>
    private static Money EstimateShipping(BomComponent component)
    {
        var kilos = Math.Max(1, (component.TotalWeightGrams() + 999) / 1000);
        return Money.Of(4.90m).Times(kilos);
    }
}
