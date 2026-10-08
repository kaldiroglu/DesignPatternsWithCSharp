using System.Text;

namespace dev.kaldiroglu.TemplateMethod.Export.Problem;

// Inside the namespace, so that the record Export wins over the namespace Export.
using dev.kaldiroglu.TemplateMethod.Export.Domain;

/// <summary>Stage three: CSV, with the steps called in the right order.</summary>
public sealed class CsvExport(AuditLog audit) : ExportSupport(audit)
{
    public override Export Export(User user, Report report)
    {
        CheckPermission(user);
        var content = new StringBuilder("customer,amount\n");
        foreach (Sale sale in report.Sales)
        {
            content.Append(sale.Customer).Append(',').Append(sale.Amount).Append('\n');
        }
        var export = new Export(report.Title + ".csv", content.ToString());
        RecordAudit(user, export, report);
        return export;
    }
}
