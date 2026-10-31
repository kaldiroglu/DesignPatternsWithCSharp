using dev.kaldiroglu.Composite.Bom.Problem;
using dev.kaldiroglu.Composite.Bom.Solution;
using dev.kaldiroglu.Composite.FileSystem;
using dev.kaldiroglu.Composite.Gof.Equipment;
using dev.kaldiroglu.Composite.Gof.Graphics;
using dev.kaldiroglu.Composite.Drawing;
using dev.kaldiroglu.Composite.Hw.Expression;
using dev.kaldiroglu.Composite.Hw.OrgChart;
using dev.kaldiroglu.Composite.Hw.SurveyForm;

// Every Composite example in this repository, in the order the course covers them. One
// example runs on its own — `dotnet run -- bom-roll-ups` — and an entry named for a topic
// prints one slide's output.

static void Section(string title)
{
    Console.WriteLine();
    Console.WriteLine(new string('=', 63));
    Console.WriteLine($" {title}");
    Console.WriteLine(new string('=', 63));
}

var examples = new (string Name, string Group, Action Run)[]
{
    ("gof-graphics", "GoF Motivation — a graphics editor (p. 163)", GraphicsDemo.Run),
    ("gof-equipment", "GoF Sample Code — computer equipment (pp. 170-173)", EquipmentDemo.Run),
    ("bom-problem-right-answers", "THE PROBLEM — a bill of materials WITHOUT Composite", ProblemDemo.RightAnswers),
    ("bom-problem-repeated-walks", "THE PROBLEM — a bill of materials WITHOUT Composite", ProblemDemo.RepeatedWalks),
    ("bom-problem-list-entries", "THE PROBLEM — a bill of materials WITHOUT Composite", ProblemDemo.ListEntries),
    ("bom-problem-wheel-drift", "THE PROBLEM — a bill of materials WITHOUT Composite", ProblemDemo.WheelDrift),
    ("bom-problem-new-item", "THE PROBLEM — a bill of materials WITHOUT Composite", ProblemDemo.NewItem),
    ("bom-product-structure", "THE SOLUTION — the same bill of materials WITH Composite", SolutionDemo.ProductStructure),
    ("bom-roll-ups", "THE SOLUTION — the same bill of materials WITH Composite", SolutionDemo.RollUps),
    ("bom-sharing", "THE SOLUTION — the same bill of materials WITH Composite", SolutionDemo.Sharing),
    ("bom-engineering-change", "THE SOLUTION — the same bill of materials WITH Composite", SolutionDemo.EngineeringChange),
    ("bom-shipping-estimate", "THE SOLUTION — the same bill of materials WITH Composite", SolutionDemo.ShippingEstimate),
    ("bom-new-item", "THE SOLUTION — the same bill of materials WITH Composite", SolutionDemo.NewItem),
    ("bom-acyclic", "THE SOLUTION — the same bill of materials WITH Composite", SolutionDemo.Acyclic),
    ("graphic-draw", "Shapes on a canvas — child management on the Composite", GraphicDemo.Draw),
    ("graphic-shape-count", "Shapes on a canvas — child management on the Composite", GraphicDemo.ShapeCount),
    ("graphic-safety", "Shapes on a canvas — child management on the Composite", GraphicDemo.Safety),
    ("file-system-six-questions", "A file system — roll-ups, a cache and an iterator", FileSystemDemo.SixQuestions),
    ("file-system-disk-report", "A file system — roll-ups, a cache and an iterator", FileSystemDemo.DiskReportForOneFile),
    ("file-system-cache", "A file system — roll-ups, a cache and an iterator", FileSystemDemo.Cache),
    ("file-system-move", "A file system — roll-ups, a cache and an iterator", FileSystemDemo.Move),
    ("file-system-copy", "A file system — roll-ups, a cache and an iterator", FileSystemDemo.Copy),
    ("file-system-walk", "A file system — roll-ups, a cache and an iterator", FileSystemDemo.Walk),
    ("hw-org-chart", "Homework 1 — the org chart, and what sharing costs", OrgChartDemo.Run),
    ("hw-survey-form", "Homework 2 — the survey form, the transparent variant", SurveyFormDemo.Run),
    ("hw-expression", "Homework 3 — the expression tree", ExpressionDemo.Run),
};

if (args.Length > 0)
{
    var name = args[0].ToLowerInvariant();
    var example = examples.FirstOrDefault(e => e.Name == name);
    if (example.Run is null)
    {
        Console.WriteLine($"unknown example '{name}'. One of: "
                          + string.Join(", ", examples.Select(e => e.Name)));
        return;
    }

    example.Run();
    return;
}

string? lastGroup = null;
foreach (var (_, group, run) in examples)
{
    if (group != lastGroup)
    {
        Section(group);
        lastGroup = group;
    }
    else
    {
        Console.WriteLine();
    }

    run();
}
