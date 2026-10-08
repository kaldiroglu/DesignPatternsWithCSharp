using System.Text;

namespace dev.kaldiroglu.TemplateMethod.Export.Problem;

// Inside the namespace, so that the record Export wins over the namespace Export.
using dev.kaldiroglu.TemplateMethod.Export.Domain;

/// <summary>
/// Stage one, second copy. Compare with <see cref="StandaloneCsvExport"/>: only the text is
/// different.
/// </summary>
public sealed class StandaloneHtmlExport(AuditLog audit)
{
    public Export Export(User user, Report report)
    {
        if (!user.MayExport)
        {
            throw new ExportNotAllowedException(user);
        }
        var content = new StringBuilder("<table>\n<tr><th>customer</th><th>amount</th></tr>\n");
        foreach (Sale sale in report.Sales)
        {
            content.Append("<tr><td>").Append(sale.Customer).Append("</td><td>")
                .Append(sale.Amount).Append("</td></tr>\n");
        }
        content.Append("</table>\n");
        var export = new Export(report.Title + ".html", content.ToString());
        audit.Record(user, export, report.Sales.Count);
        return export;
    }
}
