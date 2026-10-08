namespace dev.kaldiroglu.TemplateMethod.Export.Solution;

using dev.kaldiroglu.TemplateMethod.Export.Domain;

/// <summary>
/// A <b>ConcreteClass</b> added later, by the same new team member as in stage three.
/// <para>
/// This time there is nothing to forget. The audit line is written by the template method,
/// and a subclass cannot override it, because <c>Export()</c> is not <c>virtual</c> (in
/// Java, it is <c>final</c>).
/// </para>
/// </summary>
public sealed class MarkdownExporter(AuditLog audit) : ReportExporter(audit)
{
    protected override string Header()
    {
        return "| customer | amount |\n|---|---|\n";
    }

    protected override string Row(Sale sale)
    {
        return "| " + sale.Customer + " | " + sale.Amount + " |\n";
    }

    protected override string Extension()
    {
        return ".md";
    }
}
