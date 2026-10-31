using dev.kaldiroglu.Flyweight.Book.Correct;
using Xunit;
using WrongFactory = dev.kaldiroglu.Flyweight.Book.Wrong.BookFactory;

namespace dev.kaldiroglu.Flyweight.Tests;

/// <summary>
/// Both versions of the book example. The first four tests pin the defects in
/// <c>Book.Wrong</c>; the rest hold <c>Book.Correct</c> to what the solution promises, as a
/// count.
/// </summary>
public class BookFlyweightTests
{
    [Fact(DisplayName = "The first factory allocates a new object for every character")]
    public void OriginalFactorySharesNothing()
    {
        var factory = new WrongFactory();

        Assert.NotSame(factory.CreateCharacter('e', false), factory.CreateCharacter('e', false));
    }

    [Fact(DisplayName = "The first version stores extrinsic state on the character, so it cannot be shared")]
    public void OriginalStoresPositionInsideTheFlyweight()
    {
        var factory = new WrongFactory();
        var line = factory.CreateLine(10);

        var shared = factory.CreateCharacter('o', false);
        line.Add(shared);          // position 0
        line.Add(shared);          // the same object, added again

        // Both occurrences are the same object, so the second write replaced the first.
        Assert.Equal(1, shared.GetPosition());
        Assert.Equal(2, line.GetChars().Count);
    }

    [Fact(DisplayName = "The first version records upperCase and then renders lower case")]
    public void OriginalIgnoresItsOwnIntrinsicState()
    {
        var capital = new WrongFactory().CreateCharacter('t', true);

        Assert.True(capital.IsUpperCase());
        Assert.Equal('t', capital.GetValue());
    }

    [Fact(DisplayName = "The first line accepts one character more than it was built for")]
    public void OriginalLineIsOffByOne()
    {
        var factory = new WrongFactory();
        var line = factory.CreateLine(3);

        for (int i = 0; i < 4; i++)
        {
            Assert.True(line.Add(factory.CreateCharacter('x', false)));
        }

        Assert.Equal(4, line.GetChars().Count);
    }

    [Fact(DisplayName = "The corrected factory returns one object per distinct character")]
    public void SolutionSharesByIntrinsicState()
    {
        var factory = new CharacterFactory();

        Assert.Same(factory.CreateCharacter('e', false), factory.CreateCharacter('e', false));
        Assert.Equal(2, factory.RequestCount());
        Assert.Equal(1, factory.CreatedCount());
        Assert.Equal(1, factory.SavedCount());
    }

    [Fact(DisplayName = "Case is part of the key, because it changes what is rendered")]
    public void CaseIsIntrinsic()
    {
        var factory = new CharacterFactory();

        var lower = factory.CreateCharacter('t', false);
        var upper = factory.CreateCharacter('t', true);

        Assert.NotSame(lower, upper);
        Assert.Equal('t', lower.GetValue());
        Assert.Equal('T', upper.GetValue());
        Assert.Equal(2, factory.CreatedCount());
    }

    [Fact(DisplayName = "The corrected line refuses the character that would overflow it")]
    public void SolutionLineRespectsItsCapacity()
    {
        var factory = new CharacterFactory();
        Line line = factory.CreateLine(3);

        Assert.True(line.Add(factory.CreateCharacter('a', false)));
        Assert.True(line.Add(factory.CreateCharacter('b', false)));
        Assert.True(line.Add(factory.CreateCharacter('c', false)));
        Assert.False(line.Add(factory.CreateCharacter('d', false)));
        Assert.Equal(3, line.Length());
        Assert.True(line.IsFull());
    }

    [Fact(DisplayName = "A paragraph of 236 characters costs 29 objects")]
    public void AParagraphCostsFarFewerObjectsThanItHasCharacters()
    {
        var factory = new CharacterFactory();

        var book = factory.CreateBook("Design Patterns", 1);
        Page page = factory.CreatePage(1, BookDemo.Text.Length);
        foreach (string content in BookDemo.Text)
        {
            Line line = factory.CreateLine(content.Length);
            foreach (char c in content)
            {
                line.Add(factory.CreateCharacter(char.ToLowerInvariant(c), char.IsUpper(c)));
            }

            page.Add(line);
        }

        book.Add(page);

        Assert.Equal(236, book.CharacterCount());
        Assert.Equal(236, factory.RequestCount());
        Assert.Equal(29, factory.CreatedCount());
        Assert.Equal(207, factory.SavedCount());
    }

    [Fact(DisplayName = "The same object really is in two places at once")]
    public void OneObjectAppearsInManyPlaces()
    {
        var factory = new CharacterFactory();
        Line one = factory.CreateLine(20);
        Line two = factory.CreateLine(20);

        foreach (char c in "hello")
        {
            one.Add(factory.CreateCharacter(c, false));
        }

        foreach (char c in "held")
        {
            two.Add(factory.CreateCharacter(c, false));
        }

        Assert.Same(one.CharacterAt(1), two.CharacterAt(1));
        Assert.Same(one.CharacterAt(0), two.CharacterAt(0));

        var distinct = new HashSet<object>(ReferenceEqualityComparer.Instance);
        distinct.UnionWith(one.GetChars());
        distinct.UnionWith(two.GetChars());

        Assert.Equal(9, one.Length() + two.Length());
        Assert.Equal(5, distinct.Count);
    }
}
