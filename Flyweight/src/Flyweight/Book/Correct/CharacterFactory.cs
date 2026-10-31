namespace dev.kaldiroglu.Flyweight.Book.Correct;

/// <summary>
/// FlyweightFactory (GoF, p. 198). The key identifies the intrinsic state completely: the
/// letter and its case together. A key without the case would hand back a lower-case 't' to
/// a caller that asked for a capital.
/// </summary>
public class CharacterFactory : IFactory
{
    /// <summary>One entry per distinct piece of intrinsic state, not per occurrence in the book.</summary>
    private readonly Dictionary<string, Character> _characters = new();

    private int _requests;

    public Character CreateCharacter(char value, bool upperCase)
    {
        _requests++;
        string key = value + (upperCase ? "^" : "");
        if (!_characters.TryGetValue(key, out Character? character))
        {
            character = new Character(value, upperCase);
            _characters[key] = character;
        }

        return character;
    }

    public Line CreateLine(int numberOfCharacters) => new(numberOfCharacters);

    public Page CreatePage(int no, int numberOfLines) => new(no, numberOfLines);

    public Book CreateBook(string name, int numberOfPages) => new(name, numberOfPages);

    /// <summary>Distinct flyweights actually allocated: the size of the pool.</summary>
    public int CreatedCount() => _characters.Count;

    /// <summary>How many times a character was asked for, shared or not.</summary>
    public int RequestCount() => _requests;

    /// <summary>Occurrences that cost no object at all, because the pool already held one.</summary>
    public int SavedCount() => _requests - _characters.Count;
}
