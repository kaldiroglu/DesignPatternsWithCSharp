namespace dev.kaldiroglu.Flyweight.Book.Correct;

/// <summary>
/// The factory interface the client programs against. Keeping it behind an interface means
/// a client cannot reach for a constructor, which keeps the sharing guarantee true.
/// </summary>
public interface IFactory
{
    /// <summary>Returns the shared flyweight for this letter, creating it only on first request.</summary>
    Character CreateCharacter(char value, bool upperCase);

    Line CreateLine(int numberOfCharacters);

    Page CreatePage(int no, int numberOfLines);

    Book CreateBook(string name, int numberOfPages);
}
