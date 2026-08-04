using dev.kaldiroglu.Composite.Bom.Problem;
using dev.kaldiroglu.Composite.Bom.Solution;
using dev.kaldiroglu.Composite.FileSystem;
using dev.kaldiroglu.Composite.Gof.Equipment;
using dev.kaldiroglu.Composite.Gof.Graphics;
using dev.kaldiroglu.Composite.Drawing;
using dev.kaldiroglu.Composite.Hw.Expression;
using dev.kaldiroglu.Composite.Hw.OrgChart;
using dev.kaldiroglu.Composite.Hw.SurveyForm;

// Every Composite example in this repository, in the order the course covers them.

static void Section(string title)
{
    Console.WriteLine();
    Console.WriteLine(new string('=', 63));
    Console.WriteLine($" {title}");
    Console.WriteLine(new string('=', 63));
}

Section("GoF Motivation — a graphics editor (p. 163)");
GraphicsDemo.Run();

Section("GoF Sample Code — computer equipment (pp. 170-173)");
EquipmentDemo.Run();

Section("THE PROBLEM — a bill of materials WITHOUT Composite");
ProblemDemo.Run();

Section("THE SOLUTION — the same bill of materials WITH Composite");
SolutionDemo.Run();

Section("Shapes on a canvas — child management on the Composite");
GraphicDemo.Run();

Section("A file system — roll-ups, a cache and an iterator");
FileSystemDemo.Run();

Section("Homework 1 — the org chart, and what sharing costs");
OrgChartDemo.Run();

Section("Homework 2 — the survey form, the transparent variant");
SurveyFormDemo.Run();

Section("Homework 3 — the expression tree");
ExpressionDemo.Run();
