namespace dev.kaldiroglu.Strategy.Hw.Validation;

/// <summary>
/// A rule that needs data, which is why it is worth a class where the length rule is not.
/// <para>
/// It carries a word list, it is the one that will grow, and it is the one somebody will want
/// to swap for a service call.
/// </para>
/// </summary>
public sealed class NoCommonWords : IPassphraseRule
{
    private readonly IReadOnlySet<string> _banned;

    public NoCommonWords(IReadOnlySet<string> banned)
    {
        _banned = new HashSet<string>(banned);
    }

    public static NoCommonWords TheUsualSuspects() =>
        new(new HashSet<string> { "password", "123456", "qwerty", "admin", "letmein" });

    public string Name => "NO_COMMON_WORDS";

    public IReadOnlyList<string> Complaints(string passphrase)
    {
        // Invariant, not the current culture: on a Turkish locale "ADMIN" would lower-case
        // to "admın", with a dotless i, and slip past the list.
        var lower = passphrase.ToLowerInvariant();
        return _banned.Any(word => lower.Contains(word, StringComparison.Ordinal))
            ? ["contains a word from the banned list"]
            : [];
    }
}
