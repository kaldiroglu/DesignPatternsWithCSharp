namespace dev.kaldiroglu.TemplateMethod.Export.Problem;

// Inside the namespace, so that the record Export wins over the namespace Export.
using dev.kaldiroglu.TemplateMethod.Export.Domain;

/// <summary>
/// The report that every run in this namespace exports, and two users: Deniz may export it,
/// Mert may not.
/// </summary>
internal static class SampleReport
{
    public static readonly Report Report =
        new("sales-october", [new Sale("Ayse", 1200), new Sale("Deniz", 800)]);

    public static readonly User Deniz = new("Deniz", true);
    public static readonly User Mert = new("Mert", false);

    /// <summary>Runs an export that should be refused, and returns the reason it was refused.</summary>
    public static string Refusal(Action export)
    {
        try
        {
            export();
            return "exported";
        }
        catch (ExportNotAllowedException e)
        {
            return e.Message;
        }
    }
}
