namespace dev.kaldiroglu.TemplateMethod.Export.Solution;

using dev.kaldiroglu.TemplateMethod.Export.Domain;

/// <summary>A <b>ConcreteClass</b> that also uses the hook: an HTML table must be closed.</summary>
public sealed class HtmlExporter(AuditLog audit) : ReportExporter(audit)
{
    protected override string Header()
    {
        return "<table>\n<tr><th>customer</th><th>amount</th></tr>\n";
    }

    protected override string Row(Sale sale)
    {
        return "<tr><td>" + sale.Customer + "</td><td>" + sale.Amount + "</td></tr>\n";
    }

    protected override string Extension()
    {
        return ".html";
    }

    protected override string Footer()
    {
        return "</table>\n";
    }
}
