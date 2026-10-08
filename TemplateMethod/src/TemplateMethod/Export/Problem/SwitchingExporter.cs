using System.Text;

namespace dev.kaldiroglu.TemplateMethod.Export.Problem;

// Inside the namespace, so that the record Export wins over the namespace Export.
using dev.kaldiroglu.TemplateMethod.Export.Domain;

/// <summary>
/// Stage two: <b>one class, and a switch on the format inside the steps that differ.</b>
/// <para>
/// A real improvement on stage one. The algorithm is written once: the permission check
/// comes first and the audit line comes last, for every format.
/// </para>
/// <para>
/// What it costs: every format lives in this class. A new format is a new constant and a
/// branch in three switches, and a team that owns its own format cannot add it without
/// editing this file.
/// </para>
/// </summary>
public sealed class SwitchingExporter(AuditLog audit)
{
    public Export Export(User user, Report report, Format format)
    {
        if (!user.MayExport)
        {
            throw new ExportNotAllowedException(user);
        }
        var content = new StringBuilder(format switch
        {
            Format.Csv => "customer,amount\n",
            Format.Html => "<table>\n<tr><th>customer</th><th>amount</th></tr>\n",
            _ => throw new ArgumentOutOfRangeException(nameof(format))
        });
        foreach (Sale sale in report.Sales)
        {
            content.Append(format switch
            {
                Format.Csv => sale.Customer + "," + sale.Amount + "\n",
                Format.Html => "<tr><td>" + sale.Customer + "</td><td>" + sale.Amount + "</td></tr>\n",
                _ => throw new ArgumentOutOfRangeException(nameof(format))
            });
        }
        if (format == Format.Html)
        {
            content.Append("</table>\n");
        }
        string extension = format switch
        {
            Format.Csv => ".csv",
            Format.Html => ".html",
            _ => throw new ArgumentOutOfRangeException(nameof(format))
        };
        var export = new Export(report.Title + extension, content.ToString());
        audit.Record(user, export, report.Sales.Count);
        return export;
    }
}
