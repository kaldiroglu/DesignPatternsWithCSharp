namespace dev.kaldiroglu.Strategy.Hw.Validation;

/// <summary>
/// The Context: runs whichever rules this market requires.
/// <para>
/// Note that it holds a <em>list</em> of strategies rather than one. That is a legitimate
/// shape — GoF's Strategy says nothing about how many a context may hold — and it is what
/// makes "the market decides the rules" a line of configuration.
/// </para>
/// </summary>
public sealed class SignUpForm
{
    private readonly List<IPassphraseRule> _rules = [];

    public SignUpForm(params IPassphraseRule[] rules)
    {
        _rules.AddRange(rules);
    }

    public int RuleCount => _rules.Count;

    public IReadOnlyList<string> Complaints(string passphrase)
    {
        var all = new List<string>();
        foreach (var rule in _rules)
        {
            all.AddRange(rule.Complaints(passphrase));
        }
        return [.. all];
    }

    public bool Accepts(string passphrase) => Complaints(passphrase).Count == 0;
}
