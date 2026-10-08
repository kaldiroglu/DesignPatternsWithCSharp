namespace dev.kaldiroglu.TemplateMethod.Export.Problem;

// Inside the namespace, so that the record Export wins over the namespace Export.
using dev.kaldiroglu.TemplateMethod.Export.Domain;

/// <summary>
/// Stage three: <b>a base class with the shared steps, and each format writes its own
/// <c>Export()</c>.</b>
/// <para>
/// The best of the three. No step is copied: the permission check and the audit line are
/// written once, here. And any team can add a format by writing a subclass; nothing in
/// this file changes.
/// </para>
/// <para>
/// What it does not share is the order. Each subclass writes <c>Export()</c> itself and
/// decides which helpers to call and when. <see cref="MarkdownExport"/> was written later,
/// by someone new, and it never calls <see cref="RecordAudit"/>. The auditors do not see its
/// exports. The steps are shared; the algorithm is not.
/// </para>
/// </summary>
public abstract class ExportSupport
{
    private readonly AuditLog audit;

    protected ExportSupport(AuditLog audit)
    {
        this.audit = audit;
    }

    public abstract Export Export(User user, Report report);

    protected void CheckPermission(User user)
    {
        if (!user.MayExport)
        {
            throw new ExportNotAllowedException(user);
        }
    }

    protected void RecordAudit(User user, Export export, Report report)
    {
        audit.Record(user, export, report.Sales.Count);
    }
}
