using Department = dev.kaldiroglu.Iterator.OrgChart.Solution.Department;
using DepthFirstWithYield = dev.kaldiroglu.Iterator.OrgChart.Solution.DepthFirstWithYield;
using SampleCompany = dev.kaldiroglu.Iterator.OrgChart.Solution.SampleCompany;
// Several example packages have runnable classes with the same name (Main, ChangeReportMain).
// Aliases name them apart.
using ProblemStageOne = dev.kaldiroglu.Iterator.OrgChart.Problem.StageOneMain;
using ProblemStageTwo = dev.kaldiroglu.Iterator.OrgChart.Problem.StageTwoMain;
using ProblemStageThree = dev.kaldiroglu.Iterator.OrgChart.Problem.StageThreeMain;
using ProblemChangeReport = dev.kaldiroglu.Iterator.OrgChart.Problem.ChangeReportMain;
using SolutionTwoOrders = dev.kaldiroglu.Iterator.OrgChart.Solution.TwoOrdersMain;
using SolutionChangeReport = dev.kaldiroglu.Iterator.OrgChart.Solution.ChangeReportMain;
using GofProblemMain = dev.kaldiroglu.Iterator.Gof.Problem.Main;
using GofTwoIterators = dev.kaldiroglu.Iterator.Gof.Solution.TwoIteratorsMain;
using GofPolymorphicIteration = dev.kaldiroglu.Iterator.Gof.Solution.PolymorphicIterationMain;
using GofInternalIterator = dev.kaldiroglu.Iterator.Gof.Solution.InternalIteratorMain;
using FileSystemTest = dev.kaldiroglu.Iterator.FileSystem.Test;
using RobustMain = dev.kaldiroglu.Iterator.Robust.Main;
using BomMain = dev.kaldiroglu.Iterator.Hw.Bom.Main;
using CalendarMain = dev.kaldiroglu.Iterator.Hw.Calendar.Main;
using PagingMain = dev.kaldiroglu.Iterator.Hw.Paging.Main;

namespace dev.kaldiroglu.Iterator.Demo;

/// <summary>
/// Runs the Iterator examples.
/// <para>
/// Every entry runs a static class with a <c>Run()</c> method, except <c>orgchart-yield</c>:
/// the <c>yield</c> version of the depth-first walk, written here.
/// </para>
/// <para>
/// Each example runs on its own — <c>dotnet run -- gof-problem</c> — and with no argument all of them
/// run in the order the course presents them.
/// </para>
/// </summary>
public static class Program
{
    private static readonly Dictionary<string, (string Group, Action Run)> Examples = new()
    {
        ["orgchart-problem-stage-one"] = ("THE ORG CHART", ProblemStageOne.Run),
        ["orgchart-problem-stage-two"] = ("THE ORG CHART", ProblemStageTwo.Run),
        ["orgchart-problem-stage-three"] = ("THE ORG CHART", ProblemStageThree.Run),
        ["orgchart-problem-change-report"] = ("THE ORG CHART", ProblemChangeReport.Run),
        ["orgchart-two-orders"] = ("THE ORG CHART", SolutionTwoOrders.Run),
        ["orgchart-change-report"] = ("THE ORG CHART", SolutionChangeReport.Run),
        ["orgchart-yield"] = ("THE ORG CHART", OrgChartYield),
        ["gof-problem"] = ("GOF'S LISTS", GofProblemMain.Run),
        ["gof-two-iterators"] = ("GOF'S LISTS", GofTwoIterators.Run),
        ["gof-polymorphic-iteration"] = ("GOF'S LISTS", GofPolymorphicIteration.Run),
        ["gof-internal-iterator"] = ("GOF'S LISTS", GofInternalIterator.Run),
        ["robust"] = ("WHEN THE COLLECTION CHANGES", RobustMain.Run),
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
        Department company = SampleCompany.Build("Deniz");
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
