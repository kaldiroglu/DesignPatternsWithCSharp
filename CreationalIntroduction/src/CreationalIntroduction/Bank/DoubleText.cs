using System.Globalization;

namespace dev.kaldiroglu.CreationalIntroduction.Bank;

/// <summary>
/// The text the slides show for a <c>double</c>: the shortest digits that identify the value,
/// and always one digit after the point, so 1120 prints as 1120.0.
/// </summary>
public static class DoubleText
{
    public static string Of(double value)
    {
        var text = value.ToString("R", CultureInfo.InvariantCulture);
        return text.Contains('.') ? text : text + ".0";
    }
}
