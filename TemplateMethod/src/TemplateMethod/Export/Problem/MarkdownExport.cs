using System.Text;

namespace dev.kaldiroglu.TemplateMethod.Export.Problem;

// Inside the namespace, so that the record Export wins over the namespace Export.
using dev.kaldiroglu.TemplateMethod.Export.Domain;

/// <summary>
/// Stage three: Markdown, added later by a new team member.
/// <para>
/// It compiles, it makes a correct file, and it checks the permission. It never calls
/// <c>RecordAudit</c>, so its exports are missing from the audit log. Nothing in the
/// design could stop this: <c>Export()</c> is the subclass's to write.
/// </para>
/// </summary>
public sealed class MarkdownExport(AuditLog audit) : ExportSupport(audit)
{
    public override Export Export(User user, Report report)
    {
        CheckPermission(user);
        var content = new StringBuilder("| customer | amount |\n|---|---|\n");
        foreach (Sale sale in report.Sales)
        {
            content.Append("| ").Append(sale.Customer).Append(" | ")
                .Append(sale.Amount).Append(" |\n");
        }
        return new Export(report.Title + ".md", content.ToString());
    }
}
