using Xunit;

namespace dev.kaldiroglu.TemplateMethod.Tests.Export;

// Inside the namespace, so that the record Export wins over the namespace Export.
using dev.kaldiroglu.TemplateMethod.Export.Domain;
using dev.kaldiroglu.TemplateMethod.Export.Solution;
using static dev.kaldiroglu.TemplateMethod.Tests.Export.ProblemTests;

/// <summary>
/// The report exporters with a template method. The audit-log table on the Part 3 slides
/// is asserted here.
/// </summary>
public class SolutionTests
{
    /// <summary>The promise kept: three exports, three lines in the audit log.</summary>
    [Fact]
    public void EveryExportIsAudited()
    {
        var audit = new AuditLog();

        Export csv = new CsvExporter(audit).Export(Deniz, SalesReport);
        Export html = new HtmlExporter(audit).Export(Deniz, SalesReport);
        Export markdown = new MarkdownExporter(audit).Export(Deniz, SalesReport);

        Assert.Equal(new Export("sales-october.csv", Csv), csv);
        Assert.Equal(new Export("sales-october.html", Html), html);
        Assert.Equal(new Export("sales-october.md", Markdown), markdown);
        Assert.Equal(3, audit.Lines.Count);
        Assert.Equal(["Deniz exported sales-october.csv (2 rows)",
            "Deniz exported sales-october.html (2 rows)",
            "Deniz exported sales-october.md (2 rows)"], audit.Lines);
    }

    /// <summary>Main prints stage three's audit log with 1 line and the template method's with 3.</summary>
    [Fact]
    public void MainPrintsBothAuditLogs()
    {
        List<string> lines = Printed.By(global::dev.kaldiroglu.TemplateMethod.Export.Solution.Main.Run);

        Assert.Equal([
            "Stage three, two exports, audit log: [Deniz exported sales-october.csv (2 rows)]",
            "--- sales-october.csv",
            "customer,amount",
            "Ayse,1200",
            "Deniz,800",
            "--- sales-october.html",
            "<table>",
            "<tr><th>customer</th><th>amount</th></tr>",
            "<tr><td>Ayse</td><td>1200</td></tr>",
            "<tr><td>Deniz</td><td>800</td></tr>",
            "</table>",
            "--- sales-october.md",
            "| customer | amount |",
            "|---|---|",
            "| Ayse | 1200 |",
            "| Deniz | 800 |",
            "Template method, three exports, audit log: [Deniz exported sales-october.csv (2 rows), "
                + "Deniz exported sales-october.html (2 rows), Deniz exported sales-october.md (2 rows)]"],
            lines);
    }

    /// <summary>A user who may not export gets an exception from every format, and nothing is audited.</summary>
    [Fact]
    public void PermissionComesFirst()
    {
        var audit = new AuditLog();

        foreach (ReportExporter exporter in new ReportExporter[]
                 {
                     new CsvExporter(audit), new HtmlExporter(audit), new MarkdownExporter(audit)
                 })
        {
            Assert.Throws<ExportNotAllowedException>(() => exporter.Export(Guest, SalesReport));
        }
        Assert.Empty(audit.Lines);
    }

    /// <summary>
    /// The template method Export is final, and no subclass declares one. Java's <c>final</c>
    /// is "not <c>virtual</c>" in C#.
    /// </summary>
    [Fact]
    public void TheTemplateMethodIsFinal()
    {
        var export = typeof(ReportExporter).GetMethod("Export", [typeof(User), typeof(Report)])!;

        Assert.False(export.IsVirtual);
        foreach (Type format in new[] { typeof(CsvExporter), typeof(HtmlExporter), typeof(MarkdownExporter) })
        {
            Assert.False(Code.DeclaredMethodsOf(format).Contains("Export"), format.Name);
        }
    }

    /// <summary>Header, Row and Extension are abstract; Footer is a hook that returns an empty string.</summary>
    [Fact]
    public void AbstractStepsAndOneHook()
    {
        foreach (string step in new[] { "Header", "Row", "Extension" })
        {
            var method = Code.Declared(typeof(ReportExporter), step)!;
            Assert.True(method.IsAbstract, step);
            Assert.True(method.IsFamily, step);
        }
        var footer = Code.Declared(typeof(ReportExporter), "Footer")!;
        Assert.False(footer.IsAbstract);

        // A format that does not override the hook adds nothing after its rows.
        ReportExporter noFooter = new NoFooterExporter();
        Assert.Equal("H\nAyse\nDeniz\n", noFooter.Export(Deniz, SalesReport).Content);
    }

    /// <summary>The Java test writes this as an anonymous subclass.</summary>
    private sealed class NoFooterExporter() : ReportExporter(new AuditLog())
    {
        protected override string Header() => "H\n";

        protected override string Row(Sale sale) => sale.Customer + "\n";

        protected override string Extension() => ".txt";
    }

    /// <summary>Subclasses write only the text: three short methods, and HTML also uses the Footer hook.</summary>
    [Fact]
    public void SubclassesWriteOnlyTheText()
    {
        var threeSteps = new HashSet<string> { "Header", "Row", "Extension" };

        Assert.Equal(threeSteps, Code.DeclaredMethodsOf(typeof(CsvExporter)));
        Assert.Equal(threeSteps, Code.DeclaredMethodsOf(typeof(MarkdownExporter)));
        Assert.Equal(new HashSet<string> { "Header", "Row", "Extension", "Footer" },
            Code.DeclaredMethodsOf(typeof(HtmlExporter)));
    }

    /// <summary>No subclass mentions the permission check or the audit log.</summary>
    [Fact]
    public void SubclassesNeverMentionPermissionOrAudit()
    {
        foreach (string file in new[] { "CsvExporter.cs", "HtmlExporter.cs", "MarkdownExporter.cs" })
        {
            string code = Code.Of(Path.Combine(Source, "Solution", file));
            Assert.True(0 == Code.CountOf(code, "MayExport"), file);
            Assert.True(0 == Code.CountOf(code, "audit.Record"), file);
        }
    }
}
