using System.Reflection;
using Xunit;

namespace dev.kaldiroglu.TemplateMethod.Tests.Gof;

using Application = global::dev.kaldiroglu.TemplateMethod.Gof.Solution.Application;
using Document = global::dev.kaldiroglu.TemplateMethod.Gof.Solution.Document;
using DrawApplication = global::dev.kaldiroglu.TemplateMethod.Gof.Solution.DrawApplication;
using SpreadsheetApplication = global::dev.kaldiroglu.TemplateMethod.Gof.Solution.SpreadsheetApplication;
using ProblemDraw = global::dev.kaldiroglu.TemplateMethod.Gof.Problem.DrawApplication;
using ProblemSpreadsheet = global::dev.kaldiroglu.TemplateMethod.Gof.Problem.SpreadsheetApplication;
using GofMain = global::dev.kaldiroglu.TemplateMethod.Gof.Main;

/// <summary>GoF's application framework, before and after the pattern.</summary>
public class ApplicationTests
{
    private const BindingFlags AnyInstance =
        BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    private static readonly List<string> DrawEvents = ["open house.draw", "read shapes from house.draw"];
    private static readonly List<string> SpreadsheetEvents =
        ["remember budget.sheet as the last file", "open budget.sheet", "read cells from budget.sheet"];

    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";

    /// <summary>Gof.Main opens one file of each kind, and ignores the file the drawing program cannot open.</summary>
    [Fact]
    public void MainPrintsBothApplications()
    {
        List<string> lines = Printed.By(GofMain.Run);

        Assert.Equal([
            "Draw:        " + Show(DrawEvents),
            "Spreadsheet: " + Show(SpreadsheetEvents)], lines);
    }

    /// <summary>Before the pattern: each application writes the whole algorithm and gets the same steps.</summary>
    [Fact]
    public void TheProblemApplicationsProduceTheSameSteps()
    {
        var draw = new ProblemDraw();
        draw.OpenDocument("house.draw");
        draw.OpenDocument("budget.sheet");
        var sheet = new ProblemSpreadsheet();
        sheet.OpenDocument("budget.sheet");

        Assert.Equal(DrawEvents, draw.Events);
        Assert.Equal(SpreadsheetEvents, sheet.Events);
    }

    /// <summary>With the pattern: the same steps, in the order the template method fixes.</summary>
    [Fact]
    public void TheSolutionApplicationsProduceTheSameSteps()
    {
        Application draw = new DrawApplication();
        draw.OpenDocument("house.draw");
        Application sheet = new SpreadsheetApplication();
        sheet.OpenDocument("budget.sheet");

        Assert.Equal(DrawEvents, draw.Events);
        Assert.Equal(SpreadsheetEvents, sheet.Events);
        Assert.True(draw.Documents[0].IsOpen);
        Assert.True(sheet.Documents[0].IsOpen);
    }

    /// <summary>A file the application cannot open adds no document and no event.</summary>
    [Fact]
    public void AFileThatCannotBeOpenedIsIgnored()
    {
        Application draw = new DrawApplication();

        draw.OpenDocument("budget.sheet");

        Assert.Empty(draw.Documents);
        Assert.Empty(draw.Events);
    }

    /// <summary>
    /// OpenDocument is final; CanOpenDocument and DoCreateDocument are abstract; the hook is
    /// not. Java's <c>final</c> is "not <c>virtual</c>" in C#.
    /// </summary>
    [Fact]
    public void TheKindsOfMethod()
    {
        var openDocument = typeof(Application).GetMethod("OpenDocument", AnyInstance, [typeof(string)])!;
        var canOpen = typeof(Application).GetMethod("CanOpenDocument", AnyInstance, [typeof(string)])!;
        var create = typeof(Application).GetMethod("DoCreateDocument", AnyInstance, [typeof(string)])!;
        var hook = typeof(Application).GetMethod("AboutToOpenDocument", AnyInstance, [typeof(Document)])!;

        Assert.False(openDocument.IsVirtual);
        Assert.True(canOpen.IsAbstract);
        Assert.True(create.IsAbstract);
        Assert.False(hook.IsAbstract);
        Assert.True(hook.IsFamily);
    }

    /// <summary>Only the spreadsheet overrides the hook; the drawing program leaves it alone.</summary>
    [Fact]
    public void OnlyTheSpreadsheetUsesTheHook()
    {
        Assert.Null(typeof(DrawApplication).GetMethod("AboutToOpenDocument", AnyInstance, [typeof(Document)]));
        Assert.NotNull(typeof(SpreadsheetApplication).GetMethod("AboutToOpenDocument", AnyInstance, [typeof(Document)]));
    }

    /// <summary>When the factory method creates no document, nothing is added and nothing is read.</summary>
    [Fact]
    public void ANullDocumentStopsTheAlgorithm()
    {
        Application refuses = new RefusingApplication();

        refuses.OpenDocument("anything");

        Assert.Empty(refuses.Documents);
        Assert.Empty(refuses.Events);
    }

    /// <summary>The Java test writes this as an anonymous subclass.</summary>
    private sealed class RefusingApplication : Application
    {
        protected override bool CanOpenDocument(string name) => true;

        protected override Document? DoCreateDocument(string name) => null;
    }
}
