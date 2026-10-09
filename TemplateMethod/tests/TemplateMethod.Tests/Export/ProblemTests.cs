using System.Text.RegularExpressions;
using Xunit;

namespace dev.kaldiroglu.TemplateMethod.Tests.Export;

// Inside the namespace, so that the record Export wins over the namespace Export.
using dev.kaldiroglu.TemplateMethod.Export.Domain;
using dev.kaldiroglu.TemplateMethod.Export.Problem;

/// <summary>The three attempts of Part 1, and the reversal: a Markdown export that skips the audit.</summary>
public class ProblemTests
{
    internal const string Source = "Export";

    internal static readonly Report SalesReport =
        new("sales-october", [new Sale("Ayse", 1200), new Sale("Deniz", 800)]);
    internal static readonly User Deniz = new("Deniz", true);
    internal static readonly User Guest = new("Guest", false);

    internal const string Csv = "customer,amount\nAyse,1200\nDeniz,800\n";
    internal const string Html = "<table>\n<tr><th>customer</th><th>amount</th></tr>\n"
        + "<tr><td>Ayse</td><td>1200</td></tr>\n<tr><td>Deniz</td><td>800</td></tr>\n</table>\n";
    internal const string Markdown = "| customer | amount |\n|---|---|\n| Ayse | 1200 |\n| Deniz | 800 |\n";

    // ------------------------------------------------------------------ stage one

    /// <summary>Stage one works: the CSV file is right, and the audit log has the export.</summary>
    [Fact]
    public void StageOneWorks()
    {
        var audit = new AuditLog();

        Export csv = new StandaloneCsvExport(audit).Export(Deniz, SalesReport);
        Export html = new StandaloneHtmlExport(audit).Export(Deniz, SalesReport);

        Assert.Equal(new Export("sales-october.csv", Csv), csv);
        Assert.Equal(new Export("sales-october.html", Html), html);
        Assert.Equal(["Deniz exported sales-october.csv (2 rows)",
            "Deniz exported sales-october.html (2 rows)"], audit.Lines);
    }

    /// <summary>Stage one: the permission check and the audit line are written again in every copy.</summary>
    [Fact]
    public void StageOneCopiesTheSharedSteps()
    {
        foreach (string file in new[] { "StandaloneCsvExport.cs", "StandaloneHtmlExport.cs" })
        {
            string code = Code.Of(Path.Combine(Source, "Problem", file));
            Assert.True(1 == Code.CountOf(code, "user.MayExport"), file);
            Assert.True(1 == Code.CountOf(code, "audit.Record("), file);
        }
    }

    // ------------------------------------------------------------------ stage two

    /// <summary>Stage two: one class makes both formats, permission first and audit last.</summary>
    [Fact]
    public void StageTwoWorks()
    {
        var audit = new AuditLog();
        var exporter = new SwitchingExporter(audit);

        Assert.Equal(new Export("sales-october.csv", Csv), exporter.Export(Deniz, SalesReport, Format.Csv));
        Assert.Equal(new Export("sales-october.html", Html), exporter.Export(Deniz, SalesReport, Format.Html));
        Assert.Equal(2, audit.Lines.Count);
    }

    /// <summary>
    /// Stage two: every format is a branch in this class, in three switches. The Java counts
    /// <c>switch (format)</c> statements and their <c>case</c> labels; the C# writes switch
    /// expressions, so this counts <c>format switch</c> and the arms that name a format.
    /// </summary>
    [Fact]
    public void StageTwoHasThreeSwitches()
    {
        string code = Code.Of(Path.Combine(Source, "Problem", "SwitchingExporter.cs"));

        Assert.Equal(3, Code.CountOf(code, "format switch"));
        // Each format is a branch in all three switches.
        Assert.Equal(3 * Enum.GetValues<Format>().Length,
            Regex.Matches(code, @"Format\.\w+\s*=>").Count);
    }

    // ------------------------------------------------------------------ stage three

    /// <summary>Stage three: the Markdown export makes a correct file and checks the permission.</summary>
    [Fact]
    public void MarkdownMakesACorrectFile()
    {
        var audit = new AuditLog();

        Assert.Equal(new Export("sales-october.md", Markdown),
            new MarkdownExport(audit).Export(Deniz, SalesReport));
        Assert.Throws<ExportNotAllowedException>(() => new MarkdownExport(audit).Export(Guest, SalesReport));
    }

    /// <summary>The reversal: after a CSV and a Markdown export, the audit log has one line, the CSV one.</summary>
    [Fact]
    public void TheMarkdownExportIsMissingFromTheAudit()
    {
        var audit = new AuditLog();

        new CsvExport(audit).Export(Deniz, SalesReport);
        new MarkdownExport(audit).Export(Deniz, SalesReport);

        Assert.Single(audit.Lines);
        Assert.Equal(["Deniz exported sales-october.csv (2 rows)"], audit.Lines);
    }

    /// <summary>Stage three: MarkdownExport never calls RecordAudit, and CsvExport does.</summary>
    [Fact]
    public void MarkdownExportNeverCallsRecordAudit()
    {
        Assert.Equal(0, Code.CountOf(Code.Of(Path.Combine(Source, "Problem", "MarkdownExport.cs")), "RecordAudit("));
        Assert.Equal(1, Code.CountOf(Code.Of(Path.Combine(Source, "Problem", "CsvExport.cs")), "RecordAudit("));
    }

    /// <summary>A user who may not export gets an exception, and nothing reaches the audit log.</summary>
    [Fact]
    public void ARefusedExportIsNotAudited()
    {
        var audit = new AuditLog();

        Assert.Throws<ExportNotAllowedException>(() => new StandaloneCsvExport(audit).Export(Guest, SalesReport));
        Assert.Throws<ExportNotAllowedException>(() => new StandaloneHtmlExport(audit).Export(Guest, SalesReport));
        Assert.Throws<ExportNotAllowedException>(
            () => new SwitchingExporter(audit).Export(Guest, SalesReport, Format.Csv));
        Assert.Throws<ExportNotAllowedException>(() => new CsvExport(audit).Export(Guest, SalesReport));

        Assert.Empty(audit.Lines);
    }
}
