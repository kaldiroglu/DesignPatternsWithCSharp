namespace dev.kaldiroglu.Flyweight.Book.Correct;

/// <summary>
/// Types a real paragraph, because the saving is the gap between how many characters the
/// text has and how many distinct ones it uses, and a nine-letter sample cannot show it.
/// </summary>
public static class BookDemo
{
    public static readonly string[] Text =
    [
        "Flyweight uses sharing to support large numbers of",
        "fine-grained objects efficiently. A flyweight is a",
        "shared object that can be used in multiple contexts",
        "simultaneously, and it cannot make assumptions about",
        "the context in which it operates.",
    ];

    public static void Run()
    {
        var factory = new CharacterFactory();

        Book book = factory.CreateBook("Design Patterns", 1);
        Page page = factory.CreatePage(1, Text.Length);

        foreach (string text in Text)
        {
            Line line = factory.CreateLine(text.Length);
            foreach (char c in text)
            {
                // The case of the letter is intrinsic, so it is part of what gets shared.
                bool upperCase = char.IsUpper(c);
                line.Add(factory.CreateCharacter(char.ToLowerInvariant(c), upperCase));
            }

            page.Add(line);
        }

        book.Add(page);

        Console.WriteLine(book);

        Console.WriteLine($"Character occurrences in the book : {book.CharacterCount()}");
        Console.WriteLine($"Requests made to the factory      : {factory.RequestCount()}");
        Console.WriteLine($"Distinct flyweights allocated     : {factory.CreatedCount()}");
        Console.WriteLine($"Occurrences that cost no object   : {factory.SavedCount()}");

        // The claim the solution makes: one object, in many places at once.
        Line first = page.GetLines()[0];
        Line second = page.GetLines()[1];
        Character eInLineOne = first.CharacterAt(IndexOf(first, 'e'));
        Character eInLineTwo = second.CharacterAt(IndexOf(second, 'e'));

        Console.WriteLine();
        Console.WriteLine("The 'e' on line 1 and the 'e' on line 2 are the same object: "
                          + ReferenceEquals(eInLineOne, eInLineTwo).ToString().ToLowerInvariant());
    }

    /// <summary>Position of the first occurrence of a letter on a line: extrinsic state, read live.</summary>
    private static int IndexOf(Line line, char letter)
    {
        for (int i = 0; i < line.Length(); i++)
        {
            if (line.CharacterAt(i).GetValue() == letter)
            {
                return i;
            }
        }

        throw new ArgumentException($"no '{letter}' on this line");
    }
}
