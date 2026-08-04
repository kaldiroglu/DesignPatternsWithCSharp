using dev.kaldiroglu.Composite.Bom.Domain;

namespace dev.kaldiroglu.Composite.Bom.Problem;

/// <summary>
/// Runs the naive design and shows what it costs.
/// </summary>
/// <remarks>
/// The totals it prints are <b>correct</b> — this is not a demonstration of a broken
/// program. It is a demonstration of a program that is correct today and expensive to
/// keep correct. <c>Solution.SolutionDemo</c> runs straight afterwards; compare them.
/// </remarks>
public static class ProblemDemo
{
    public static void Run()
    {
        var catalog = NaiveProductCatalog.CityBicycle();
        var bicycle = catalog.Bicycle;

        Console.WriteLine("=== The naive design gets the right answers ===");
        Console.WriteLine($"Bicycle cost:   {NaiveCosting.TotalCost(bicycle)}");
        Console.WriteLine($"Bicycle weight: {NaiveShipping.TotalWeightGrams(bicycle)} g");
        Console.WriteLine($"Bicycle parts:  {NaiveCosting.PartCount(bicycle)}");

        Console.WriteLine();
        Console.WriteLine("=== ...but every client re-implements the same walk ===");
        Console.WriteLine("NaiveCosting.TotalCost   — walk #1");
        Console.WriteLine("NaiveCosting.PartCount   — walk #2");
        Console.WriteLine("NaiveShipping.TotalWeightGrams — walk #3");
        Console.WriteLine("Each one branches on the type and iterates BOTH child lists.");

        Console.WriteLine();
        Console.WriteLine("=== ...and pays for every duplicated list entry ===");
        Console.WriteLine($"Child-list entries walked per query: {ListEntries(bicycle)}");
        Console.WriteLine("(the Composite version walks 13)");

        Console.WriteLine();
        Console.WriteLine("=== ...and cannot share a sub-assembly ===");
        Console.WriteLine("Are the two wheels the same object? "
                          + (ReferenceEquals(catalog.Wheel1, catalog.Wheel2) ? "true" : "false"));
        Console.WriteLine($"Wheel 1 costs {NaiveCosting.TotalCost(catalog.Wheel1)}, "
                          + $"wheel 2 costs {NaiveCosting.TotalCost(catalog.Wheel2)}");

        // An engineering change applied the way a developer naturally would:
        // to "the wheel" — of which there are secretly two.
        catalog.Wheel1.AddPart(new Part(Catalog.Spoke), 4); // 32 -> 36 spokes
        Console.WriteLine("Engineering change: 4 more spokes on the wheel...");
        Console.WriteLine($"Wheel 1 now costs {NaiveCosting.TotalCost(catalog.Wheel1)}, "
                          + $"wheel 2 still costs {NaiveCosting.TotalCost(catalog.Wheel2)}");
        Console.WriteLine("The two wheels have silently drifted apart, and the bicycle now "
                          + $"costs {NaiveCosting.TotalCost(bicycle)} "
                          + "instead of the $399.20 it should.");

        Console.WriteLine();
        Console.WriteLine("=== ...and breaks when a new kind of item arrives ===");
        var coating = new Service(Catalog.PowderCoating);
        try
        {
            NaiveCosting.TotalCost(coating);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Adding a subcontracted operation: {e.Message}");
        }

        Console.WriteLine("Every one of the three walks needs a new type branch, "
                          + "and Assembly needs a third collection.");
    }

    /// <summary>Counts the child-list entries any full traversal has to walk.</summary>
    private static int ListEntries(Assembly assembly)
    {
        var entries = assembly.Parts.Count + assembly.SubAssemblies.Count;
        foreach (var sub in assembly.SubAssemblies)
        {
            entries += ListEntries(sub);
        }

        return entries;
    }
}
