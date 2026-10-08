using System.Text;

namespace dev.kaldiroglu.TemplateMethod.Export.Problem;

// Inside the namespace, so that the record Export wins over the namespace Export.
using dev.kaldiroglu.TemplateMethod.Export.Domain;

/// <summary>
/// Stage one: <b>each format has its own copy of the whole algorithm.</b>
/// <para>
/// Check the user, build the header, build a line per sale, name the file, write the audit
/// record. <see cref="StandaloneHtmlExport"/> does the same five things in the same order,
/// with different text in two of them.
/// </para>
/// <para>
/// It works. What it costs: four of the five steps are the same in every copy, so a fix to
/// the permission check or the audit line must be made in every class, and a missed copy
/// is a silent difference.
/// </para>
/// </summary>
public sealed class StandaloneCsvExport(AuditLog audit)
{
    public Export Export(User user, Report report)
    {
        if (!user.MayExport)
        {
            throw new ExportNotAllowedException(user);
        }
        var content = new StringBuilder("customer,amount\n");
        foreach (Sale sale in report.Sales)
        {
            content.Append(sale.Customer).Append(',').Append(sale.Amount).Append('\n');
        }
        var export = new Export(report.Title + ".csv", content.ToString());
        audit.Record(user, export, report.Sales.Count);
        return export;
    }
}
