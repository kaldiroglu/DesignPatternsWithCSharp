namespace dev.kaldiroglu.TemplateMethod.Export.Domain;

/// <summary>A report: a title and its rows.</summary>
public sealed record Report(string Title, IReadOnlyList<Sale> Sales)
{
    /// <summary>A copy of the rows, so a change to the caller's list does not change the report.</summary>
    public IReadOnlyList<Sale> Sales { get; } = Sales.ToList().AsReadOnly();
}
