using System.Text;

namespace dev.kaldiroglu.Flyweight.Book.Correct;

/// <summary>
/// Client-side container, and the place the extrinsic state lives. A character's position is
/// not stored anywhere: it is the index in this list.
/// </summary>
public class Line
{
    private readonly List<Character> _chars;
    private readonly int _capacity;

    internal Line(int capacity)
    {
        _capacity = capacity;
        _chars = new List<Character>(capacity);
    }

    /// <summary>Appends a character if there is room.</summary>
    /// <returns>True if the character was added, false if the line is full.</returns>
    public bool Add(Character character)
    {
        if (IsFull())
        {
            return false;
        }

        _chars.Add(character);
        return true;
    }

    public bool IsFull() => _chars.Count >= _capacity;

    public int Capacity() => _capacity;

    public int Length() => _chars.Count;

    /// <summary>The character at a position: the extrinsic state, read rather than stored.</summary>
    public Character CharacterAt(int position) => _chars[position];

    public IReadOnlyList<Character> GetChars() => _chars.ToList();

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
