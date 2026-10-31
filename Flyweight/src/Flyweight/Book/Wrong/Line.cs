using System.Text;

namespace dev.kaldiroglu.Flyweight.Book.Wrong;

/// <summary>
/// Declares itself full at one more than its capacity, so a line built for ten accepts
/// eleven, and <see cref="AddEndOfLine"/> goes around the check altogether.
/// </summary>
public class Line
{
    private static readonly Character EndOfLine = new('\n');

    private readonly List<Character> _chars;
    private readonly int _numberOfCharacters;
    private int _emptyPosition;
    private bool _full;

    public Line(int numberOfCharacters)
    {
        _numberOfCharacters = numberOfCharacters;
        _chars = new List<Character>(numberOfCharacters);
    }

    public bool Add(Character character)
    {
        if (_full)
        {
            return false;
        }

        _chars.Add(character);
        character.SetLine(this);
        character.SetPosition(_emptyPosition);
        _emptyPosition++;
        if (_emptyPosition == _numberOfCharacters + 1)
        {
            _full = true;
        }

        return true;
    }

    public void AddEndOfLine() => _chars.Add(EndOfLine);

    public List<Character> GetChars() => _chars;

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (Character character in _chars)
        {
            sb.Append(character.GetValue());
        }

        return sb.ToString();
    }
}
