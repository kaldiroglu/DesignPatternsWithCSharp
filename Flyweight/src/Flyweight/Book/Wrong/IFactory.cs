namespace dev.kaldiroglu.Flyweight.Book.Wrong;

public interface IFactory
{
    Character CreateCharacter(char c, bool upperCase);

    Line CreateLine(int numberOfCharacters);

    Page CreatePage(int no, int numberOfLines);

    Book CreateBook(string name, int numberOfPages);
}
