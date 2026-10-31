namespace dev.kaldiroglu.Flyweight.Book.Correct;

/// <summary>
/// ConcreteFlyweight (GoF, p. 198). Holds the intrinsic state of a character and nothing
/// else: the letter, and whether it is capitalized. The class is immutable, and the
/// constructor is internal, so a client must go through <see cref="CharacterFactory"/>.
/// Position is not here: <see cref="Line"/> holds it as an index.
/// </summary>
public sealed class Character
{
    private readonly char _value;
    private readonly bool _upperCase;

    internal Character(char value, bool upperCase)
    {
        _value = value;
        _upperCase = upperCase;
    }

    /// <summary>The character as it should be rendered.</summary>
    public char GetValue() => _upperCase ? char.ToUpperInvariant(_value) : _value;

    /// <summary>The letter as it was requested, before capitalization is applied.</summary>
    public char RawValue() => _value;

    public bool IsUpperCase() => _upperCase;

    public override string ToString() => GetValue().ToString();
}
