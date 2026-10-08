using System.Text;

namespace dev.kaldiroglu.TemplateMethod.Export.Solution;

// Inside the namespace, so that the record Export wins over the namespace Export.
using dev.kaldiroglu.TemplateMethod.Export.Domain;

/// <summary>
/// The <b>AbstractClass</b>: the export algorithm, written once.
/// <para>
/// <see cref="Export"/> is the <b>template method</b>. In Java it is <c>final</c>. In C# a
/// method is not virtual unless it says so, so the template method is simply a public method
/// without <c>virtual</c>: no subclass can change the order of the steps or leave one out.
/// The permission check is always first and the audit line is always last. Compare
/// <c>Problem.ExportSupport</c>, where each subclass wrote <c>Export()</c> itself.
/// </para>
/// <para>
/// The steps that differ by format are <c>protected abstract</c>: <see cref="Header"/>,
/// <see cref="Row"/> and <see cref="Extension"/>. A subclass must write them.
/// <see cref="Footer"/> is a <b>hook</b>: it is <c>protected virtual</c> and does nothing
/// here, and a subclass may override it if it needs to.
/// </para>
/// <para>
/// The subclass never calls the steps; the template method calls them. GoF call this the
/// Hollywood principle: "don't call us, we'll call you".
/// </para>
/// </summary>
public abstract class ReportExporter
{
    private readonly AuditLog audit;

    protected ReportExporter(AuditLog audit)
    {
        this.audit = audit;
    }

    /// <summary>
    /// The template method. Not <c>virtual</c>, so it cannot be overridden: this is what
    /// Java's <c>final</c> says.
    /// </summary>
    public Export Export(User user, Report report)
    {
        if (!user.MayExport)
        {
            throw new ExportNotAllowedException(user);
        }
        var content = new StringBuilder(Header());
        foreach (Sale sale in report.Sales)
        {
            content.Append(Row(sale));
        }
        content.Append(Footer());
        var export = new Export(report.Title + Extension(), content.ToString());
        audit.Record(user, export, report.Sales.Count);
        return export;
    }

    /// <summary>A primitive operation: every format must write its header.</summary>
    protected abstract string Header();

    /// <summary>A primitive operation: one line of the file for one sale.</summary>
    protected abstract string Row(Sale sale);

    /// <summary>A primitive operation: ".csv", ".html", ...</summary>
    protected abstract string Extension();

    /// <summary>A hook: most formats need nothing after the rows.</summary>
    protected virtual string Footer()
    {
        return "";
    }
}
