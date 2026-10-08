using System.Globalization;

using dev.kaldiroglu.Iterator.Hw.Bom;
using dev.kaldiroglu.Iterator.Hw.Bom.Composite;
using dev.kaldiroglu.Iterator.Hw.Calendar;
using dev.kaldiroglu.Iterator.Hw.Paging;
using dev.kaldiroglu.Iterator.OrgChart.Domain;
using dev.kaldiroglu.Iterator.OrgChart.Problem;
using dev.kaldiroglu.Iterator.OrgChart.Solution;
// The org chart's problem and solution each have a ChangeReport, as the Java original does.
// Aliases name them apart.
using StageThreeChangeReport = dev.kaldiroglu.Iterator.OrgChart.Problem.ChangeReport;
using OrgChartMain = dev.kaldiroglu.Iterator.OrgChart.Solution.Main;
using GofMain = dev.kaldiroglu.Iterator.Gof.Main;
using FileSystemTest = dev.kaldiroglu.Iterator.FileSystem.Test;

namespace dev.kaldiroglu.Iterator.Demo;

/// <summary>
/// Runs the Iterator examples.
/// <para>
/// <c>orgchart</c>, <c>gof</c> and <c>filesystem</c> are the Java original's <c>main</c>
/// methods and print the same output. The others are only in this runner: the three naive
/// org charts, the <c>yield</c> version of the depth-first walk, and the three homework
/// exercises, which have no <c>main</c> in Java.
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
        ["orgchart-problem"] = ("THE ORG CHART", OrgChartProblem),
        ["orgchart"] = ("THE ORG CHART", OrgChartMain.Run),
        ["orgchart-yield"] = ("THE ORG CHART", OrgChartYield),
        ["gof"] = ("GOF'S LISTS", GofMain.Run),
        ["filesystem"] = ("THE FILE SYSTEM", FileSystemTest.Run),
        ["hw-bom"] = ("HOMEWORK", Bom),
        ["hw-calendar"] = ("HOMEWORK", Calendar),
        ["hw-paging"] = ("HOMEWORK", Paging)
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

    // ------------------------------------------------------------ the org chart, before

    /// <summary>The same company as <c>OrgChart.Solution.Main</c>, built three times, once per naive stage.</summary>
    private static void OrgChartProblem()
    {
        OpenDepartment open = new OpenDepartment("Head office")
            .Add(new Employee("Ayse", "CEO"))
            .Add(new OpenDepartment("Sales")
                .Add(new Employee("Deniz", "head of sales"))
                .Add(new Employee("Ali", "sales"))
                .Add(new OpenDepartment("Export").Add(new Employee("Can", "export"))))
            .Add(new OpenDepartment("Operations")
                .Add(new Employee("Mert", "head of operations"))
                .Add(new OpenDepartment("Support").Add(new Employee("Elif", "support"))));
        Console.WriteLine("Stage one, payroll writes the recursion: "
            + string.Join(", ", new PayrollRun().Payslips(open)));

        CopyingDepartment copying = new CopyingDepartment("Head office")
            .Add(new Employee("Ayse", "CEO"))
            .Add(new CopyingDepartment("Sales")
                .Add(new Employee("Deniz", "head of sales"))
                .Add(new Employee("Ali", "sales"))
                .Add(new CopyingDepartment("Export").Add(new Employee("Can", "export"))))
            .Add(new CopyingDepartment("Operations")
                .Add(new Employee("Mert", "head of operations"))
                .Add(new CopyingDepartment("Support").Add(new Employee("Elif", "support"))));
        Console.WriteLine("Stage two, a copy of everyone:           "
            + string.Join(", ", copying.Everyone().Select(e => e.Name)));

        CallbackDepartment before = Callback("Deniz");
        var depthFirst = new List<string>();
        before.ForEachMember(e => depthFirst.Add(e.Name));
        var byLevel = new List<string>();
        before.ForEachMemberByLevel(e => byLevel.Add(e.Name));
        Console.WriteLine("Stage three, department by department:   " + string.Join(", ", depthFirst));
        Console.WriteLine("Stage three, level by level:             " + string.Join(", ", byLevel));
        Console.WriteLine("Stage three, first change (by copying):  "
            + (new StageThreeChangeReport().FirstDifference(before, Callback("Zeynep")) ?? "none"));
    }

    private static CallbackDepartment Callback(string salesHead) =>
        new CallbackDepartment("Head office")
            .Add(new Employee("Ayse", "CEO"))
            .Add(new CallbackDepartment("Sales")
                .Add(new Employee(salesHead, "head of sales"))
                .Add(new Employee("Ali", "sales"))
                .Add(new CallbackDepartment("Export").Add(new Employee("Can", "export"))))
            .Add(new CallbackDepartment("Operations")
                .Add(new Employee("Mert", "head of operations"))
                .Add(new CallbackDepartment("Support").Add(new Employee("Elif", "support"))));

    // ------------------------------------------------------------ the org chart, with yield

    private static void OrgChartYield()
    {
        Department company = OrgChartMain.Company("Deniz");
        var withYield = DepthFirstWithYield.Walk(company).ToList();
        Console.WriteLine("With yield return: " + string.Join(", ", withYield.Select(e => e.Name)));
        Console.WriteLine("Same order as DepthFirstIterator: "
            + (withYield.SequenceEqual(company) ? "yes" : "no"));
    }

    // ------------------------------------------------------------ homework

    /// <summary>A bicycle: a frame, two wheels each with a rim and 36 spokes, and an assembly service.</summary>
    private static void Bom()
    {
        var wheel = new Assembly("WHEEL-700C", "wheel")
            .Add(new Part("RIM-700C", "rim", Money.Of(24.50m), 450))
            .Add(new Part("SPOKE-SS", "spoke", Money.Of(0.40m), 7), 36);
        var bicycle = new Assembly("BIKE-01", "bicycle")
            .Add(new Part("FRAME-AL", "frame", Money.Of(180.00m), 1600))
            .Add(wheel, 2)
            .Add(new Service("SVC-ASSY", "assembly", Money.Of(35.00m)));

        Console.WriteLine("Parts: " + string.Join(", ", PartIterator.PartsOf(bicycle)));
    }

    /// <summary>The business days of 2026-10-01 to 2026-10-09, with a holiday on 2026-10-06.</summary>
    private static void Calendar()
    {
        var days = new BusinessDays(
            new DateOnly(2026, 10, 1), new DateOnly(2026, 10, 9), [new DateOnly(2026, 10, 6)]);

        Console.WriteLine("Business days: "
            + string.Join(", ", days.Select(d => d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))));
    }

    /// <summary>Three pages of three orders, then an empty page. Stops at the first match.</summary>
    private static void Paging()
    {
        PageSource<string> orders = number =>
            number < 3
                ? [.. Enumerable.Range(number * 3 + 1, 3).Select(n => "order-" + n)]
                : [];

        using var search = new PagedIterator<string>(orders);
        string? found = null;
        while (search.MoveNext())
        {
            if (search.Current == "order-5")
            {
                found = search.Current;
                break;
            }
        }

        Console.WriteLine($"Found {found} after fetching {search.PagesFetched} page(s)");

        using var all = new PagedIterator<string>(orders);
        int count = 0;
        while (all.MoveNext())
        {
            count++;
        }

        Console.WriteLine($"Walked all {count} orders after fetching {all.PagesFetched} page(s), the last one empty");
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
