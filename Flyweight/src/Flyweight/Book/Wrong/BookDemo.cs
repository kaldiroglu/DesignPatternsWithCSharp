namespace dev.kaldiroglu.Flyweight.Book.Wrong;

/// <summary>Asks for a capital T, and prints "this book" in lower case.</summary>
public static class BookDemo
{
    public static void Run()
    {
        IFactory factory = new BookFactory();
        // Page 1
        Line line1 = factory.CreateLine(10);
        Character c1 = factory.CreateCharacter('t', true);
        line1.Add(c1);
        Character c2 = factory.CreateCharacter('h', false);
        line1.Add(c2);
        Character c3 = factory.CreateCharacter('i', false);
        line1.Add(c3);
        Character c4 = factory.CreateCharacter('s', false);
        line1.Add(c4);
        Character c5 = factory.CreateCharacter(' ', false);
        line1.Add(c5);
        Character c6 = factory.CreateCharacter('b', false);
        line1.Add(c6);
        Character c7 = factory.CreateCharacter('o', false);
        line1.Add(c7);
        Character c8 = factory.CreateCharacter('o', false);
        line1.Add(c8);
        Character c9 = factory.CreateCharacter('k', false);
        line1.Add(c9);
        line1.AddEndOfLine();

        Page page1 = factory.CreatePage(1, 20);
        page1.Add(line1);

        Book book = factory.CreateBook("Thinking Design Patterns", 349);
        book.Add(page1);
        Console.WriteLine(book);
    }
}
