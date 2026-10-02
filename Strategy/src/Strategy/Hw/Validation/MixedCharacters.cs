namespace dev.kaldiroglu.Strategy.Hw.Validation;

/// <summary>Letters, digits and something else — the rule most markets settled on.</summary>
public sealed class MixedCharacters : IPassphraseRule
{
    public string Name => "MIXED";

    public IReadOnlyList<string> Complaints(string passphrase)
    {
        var complaints = new List<string>();
        if (!passphrase.Any(char.IsLetter))
        {
            complaints.Add("no letters");
        }
        if (!passphrase.Any(char.IsDigit))
        {
            complaints.Add("no digits");
        }
        if (passphrase.All(char.IsLetterOrDigit))
        {
            complaints.Add("no punctuation");
        }
        return [.. complaints];
    }
}
