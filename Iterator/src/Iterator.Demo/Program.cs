using dev.kaldiroglu.Iterator.OrgChart.Solution;
// Every example package has a class named Main, as the Java original does. Aliases name them
// apart.
using OrgChartProblemMain = dev.kaldiroglu.Iterator.OrgChart.Problem.Main;
using OrgChartMain = dev.kaldiroglu.Iterator.OrgChart.Solution.Main;
using GofMain = dev.kaldiroglu.Iterator.Gof.Main;
using GofProblemMain = dev.kaldiroglu.Iterator.Gof.Problem.Main;
using GofSolutionMain = dev.kaldiroglu.Iterator.Gof.Solution.Main;
using FileSystemTest = dev.kaldiroglu.Iterator.FileSystem.Test;
using BomMain = dev.kaldiroglu.Iterator.Hw.Bom.Main;
using CalendarMain = dev.kaldiroglu.Iterator.Hw.Calendar.Main;
using PagingMain = dev.kaldiroglu.Iterator.Hw.Paging.Main;

namespace dev.kaldiroglu.Iterator.Demo;

/// <summary>
/// Runs the Iterator examples.
/// <para>
/// Every entry but one is a Java <c>main</c> method, ported as a class <c>Main</c> (or
/// <c>Test</c>) with a <c>Run()</c> method, and prints the same output. <c>orgchart-yield</c>
/// is only in this runner: the <c>yield</c> version of the depth-first walk.
/// </para>
/// <para>
/// Each example runs on its own — <c>dotnet run -- gof</c> — and with no argument all of them
/// run in the order the course presents them.
/// </para>
/// </summary>
public static class Program
{
    private static readonly Dictionary<string, (string Group, Action Run)> Examples = new()
    {
        ["orgchart-problem"] = ("THE ORG CHART", OrgChartProblemMain.Run),
        ["orgchart"] = ("THE ORG CHART", OrgChartMain.Run),
        ["orgchart-yield"] = ("THE ORG CHART", OrgChartYield),
        ["gof"] = ("GOF'S LISTS", GofMain.Run),
        ["gof-problem"] = ("GOF'S LISTS", GofProblemMain.Run),
        ["gof-solution"] = ("GOF'S LISTS", GofSolutionMain.Run),
        ["filesystem"] = ("THE FILE SYSTEM", FileSystemTest.Run),
        ["hw-bom"] = ("HOMEWORK", BomMain.Run),
        ["hw-calendar"] = ("HOMEWORK", CalendarMain.Run),
        ["hw-paging"] = ("HOMEWORK", PagingMain.Run)
    };

    public static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            var name = args[0].ToLowerInvariant();
            if (!Examples.TryGetValue(name, out var example))
            {
                Console.WriteLine($"unknown example '{name}'. One of: {string.Join(", ", Examples.Keys)}");
                return;
            }

            example.Run();
            return;
        }

        string? lastGroup = null;
        foreach (var (name, (group, run)) in Examples)
        {
            if (group != lastGroup)
            {
                Heading(group);
                lastGroup = group;
            }

            Section(name);
            run();
        }
    }

    // ------------------------------------------------------------ the org chart, with yield

    private static void OrgChartYield()
    {
        Department company = OrgChartMain.Company("Deniz");
        var withYield = DepthFirstWithYield.Walk(company).ToList();
        Console.WriteLine("With yield return: " + string.Join(", ", withYield.Select(e => e.Name)));
        Console.WriteLine("Same order as DepthFirstIterator: "
            + (withYield.SequenceEqual(company) ? "yes" : "no"));
    }

    // ---------------------------------------------------------------- output

    private static void Heading(string title)
    {
        Console.WriteLine("\n" + new string('=', 72));
        Console.WriteLine(title);
        Console.WriteLine(new string('=', 72));
    }

    private static void Section(string title) =>
        Console.WriteLine($"\n--- {title} {new string('-', Math.Max(0, 68 - title.Length))}");
}
