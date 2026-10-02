namespace dev.kaldiroglu.Strategy.Hw.Validation;

/// <summary>
/// The Strategy: what a market requires of a passphrase.
/// <para>
/// Homework 3, and the one with a trap in it. Two of these rules are a single condition each,
/// and the exercise asks whether a class per rule is worth it — the answer is not always yes.
/// </para>
/// </summary>
public interface IPassphraseRule
{
    string Name { get; }

    /// <summary>Every reason this passphrase is unacceptable. Empty means it is fine.</summary>
    IReadOnlyList<string> Complaints(string passphrase);
}
