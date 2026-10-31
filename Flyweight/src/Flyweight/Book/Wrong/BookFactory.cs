namespace dev.kaldiroglu.Flyweight.Book.Wrong;

/// <summary>
/// The first defect of this example: the factory shares nothing. Every request allocates,
/// and the <c>_as</c> field, meant to be the pool, is never read.
/// </summary>
public class BookFactory : IFactory
{
#pragma warning disable CS0414 // The unused pool is the lesson.
    private List<Character>? _as = [];
#pragma warning restore CS0414

    public Character CreateCharacter(char c, bool upperCase) => new(c, upperCase);

    public Line CreateLine(int numberOfCharacters) => new(numberOfCharacters);

    public Page CreatePage(int no, int numberOfLines) => new(no, numberOfLines);

    public Book CreateBook(string name, int numberOfPages) => new(name, numberOfPages);
}
