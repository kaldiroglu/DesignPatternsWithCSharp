namespace dev.kaldiroglu.Composite.Hw.OrgChart;

/// <summary>
/// Homework 1 — the org chart.
/// </summary>
/// <remarks>
/// The roll-up is the easy half. The interesting half is what happens when somebody reports
/// to two managers, which is common in real companies and fatal to a tree.
/// </remarks>
public static class OrgChartDemo
{
    public static void Run()
    {
        var ayse = new IndividualContributor("Ayse", "engineer", 90_000);
        var bora = new IndividualContributor("Bora", "engineer", 85_000);
        var cem = new IndividualContributor("Cem", "designer", 80_000);

        var engineering = new Manager("Deniz", "eng manager", 120_000).Add(ayse).Add(bora);
        var design = new Manager("Ece", "design manager", 115_000).Add(cem);
        var chief = new Manager("Fatma", "CTO", 180_000).Add(engineering).Add(design);

        Console.WriteLine(chief.Render(""));
        Console.WriteLine();
        Console.WriteLine($"headcount  {chief.Headcount()}");
        Console.WriteLine($"total cost {chief.TotalCost()}");
        Console.WriteLine($"engineering only: {engineering.Headcount()} people, "
            + $"{engineering.TotalCost()}");

        Console.WriteLine();
        Console.WriteLine("Now the part a tree cannot express. Cem is a designer who also");
        Console.WriteLine("works permanently inside the engineering team, so he reports to");
        Console.WriteLine("both Ece and Deniz.");

        engineering.Add(cem);   // the same object, in two places

        Console.WriteLine();
        Console.WriteLine($"headcount now          {chief.Headcount()}  <- Cem counted twice");
        Console.WriteLine($"distinct headcount     {chief.DistinctHeadcount()}  <- the true number");
        Console.WriteLine($"total cost now         {chief.TotalCost()}  <- and his salary paid twice");

        Console.WriteLine();
        Console.WriteLine("Nothing threw. Nothing warned. The structure is now a graph and");
        Console.WriteLine("every roll-up over it silently over-counts.");
        Console.WriteLine();
        Console.WriteLine("A cycle is caught, because that one hangs the program:");
        try
        {
            engineering.Add(chief);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"  rejected: {e.Message}");
        }
    }
}
