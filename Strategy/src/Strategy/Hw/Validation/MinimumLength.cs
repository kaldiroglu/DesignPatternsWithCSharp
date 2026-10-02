namespace dev.kaldiroglu.Strategy.Hw.Validation;

/// <summary>One condition, and the reason the exercise asks whether this deserves a class.</summary>
public sealed class MinimumLength : IPassphraseRule
{
    private readonly int _minimum;

    public MinimumLength(int minimum)
    {
        _minimum = minimum;
    }

    public string Name => "MIN_LENGTH_" + _minimum;

    public IReadOnlyList<string> Complaints(string passphrase) =>
        passphrase.Length >= _minimum
            ? []
            : ["shorter than " + _minimum + " characters"];
}
