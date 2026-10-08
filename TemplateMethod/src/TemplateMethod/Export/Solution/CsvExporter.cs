namespace dev.kaldiroglu.TemplateMethod.Export.Solution;

using dev.kaldiroglu.TemplateMethod.Export.Domain;

/// <summary>A <b>ConcreteClass</b>: only the text of a CSV file. It has no <c>Export()</c> of its own.</summary>
public sealed class CsvExporter(AuditLog audit) : ReportExporter(audit)
{
    protected override string Header()
    {
        return "customer,amount\n";
    }

    protected override string Row(Sale sale)
    {
        return sale.Customer + "," + sale.Amount + "\n";
    }

    protected override string Extension()
    {
        return ".csv";
    }
}
