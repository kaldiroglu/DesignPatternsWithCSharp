namespace dev.kaldiroglu.Flyweight.Book.Wrong;

/// <summary>
/// The flyweight that cannot be shared. It stores its line and position, so two occurrences
/// of one object would overwrite each other, and it records <c>upperCase</c> and then ignores
/// it, so a capital renders in lower case.
/// </summary>
public class Character
{
    // Intrinsic properties
    private readonly char _value;
    private readonly bool _upperCase;

    // Extrinsic properties
    private Line? _line;
    private int _position;

    public Character(char value, bool upperCase)
    {
        _value = value;
        _upperCase = upperCase;
    }

    public Character(int value)
    {
        _value = (char)value;
    }

    public char GetValue() => _value;

    public bool IsUpperCase() => _upperCase;

    public Line? GetLine() => _line;

    public void SetLine(Line line) => _line = line;

    public int GetPosition() => _position;

    public void SetPosition(int position) => _position = position;

    public override string ToString() => _value.ToString();
}
