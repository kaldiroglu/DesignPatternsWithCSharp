namespace dev.kaldiroglu.TemplateMethod.Export.Problem;

// Inside the namespace, so that the record Export wins over the namespace Export.
using dev.kaldiroglu.TemplateMethod.Export.Domain;

/// <summary>The report and the user that every run in this namespace exports with.</summary>
internal static class SampleReport
{
    public static readonly Report Report =
        new("sales-october", [new Sale("Ayse", 1200), new Sale("Deniz", 800)]);

    public static readonly User Deniz = new("Deniz", true);
}
