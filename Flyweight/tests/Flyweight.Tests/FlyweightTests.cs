using dev.kaldiroglu.Flyweight;
using Xunit;

namespace dev.kaldiroglu.Flyweight.Tests;

public class FlyweightTests
{
    [Fact(DisplayName = "Factory returns the SAME shared instance for the same letter")]
    public void FactorySharesCharacterFlyweights()
    {
        var factory = new GlyphFactory();

        CharacterGlyph a1 = factory.CreateCharacter('a');
        CharacterGlyph a2 = factory.CreateCharacter('a');

        Assert.Same(a1, a2);
        Assert.Equal(1, factory.CreatedCharacterCount);
    }

    [Fact(DisplayName = "Distinct letters get distinct flyweights")]
    public void DistinctLettersGetDistinctFlyweights()
    {
        var factory = new GlyphFactory();

        factory.CreateCharacter('a');
        factory.CreateCharacter('b');
        factory.CreateCharacter('a'); // pool hit

        Assert.Equal(2, factory.CreatedCharacterCount);
    }

    [Fact(DisplayName = "Rows and Columns are unshared (a fresh one every time)")]
    public void RowsAndColumnsAreNeverShared()
    {
        var factory = new GlyphFactory();

        Assert.NotSame(factory.CreateRow(), factory.CreateRow());
        Assert.NotSame(factory.CreateColumn(), factory.CreateColumn());
    }

    [Fact(DisplayName = "One flyweight renders in different fonts via the context (extrinsic state)")]
    public void SameFlyweightRendersInDifferentFonts()
    {
        var factory = new GlyphFactory();
        CharacterGlyph t = factory.CreateCharacter('t');

        var context = new GlyphContext(new Font("default"));
        var times = new Font("Times");
        var courier = new Font("Courier");
        context.SetFont(times, 1);
        context.Next();
        context.SetFont(courier, 1);

        var window = new Window();
        context.Reset();
        t.Draw(window, context); // position 0 -> Times
        t.Draw(window, context); // position 1 -> Courier

        Assert.Equal(times, window.Rendered[0].Font);
        Assert.Equal(courier, window.Rendered[1].Font);
        Assert.Equal(1, factory.CreatedCharacterCount); // still only ONE flyweight
    }

    [Fact(DisplayName = "A document draws its glyphs in order")]
    public void DocumentDrawsInOrder()
    {
        var factory = new GlyphFactory();

        Column column = factory.CreateColumn();
        Row row = factory.CreateRow();
        foreach (char c in "hi")
        {
            row.Insert(factory.CreateCharacter(c));
        }
        column.Insert(row);

        var window = new Window();
        column.Draw(window, new GlyphContext(new Font("default")));

        Assert.Equal("hi", window.Text());
    }

    [Fact(DisplayName = "Sharing reduces the object count below the number of occurrences")]
    public void SharingReducesObjectCount()
    {
        var factory = new GlyphFactory();

        const string text = "banana";
        int occurrences = 0;
        foreach (char c in text)
        {
            factory.CreateCharacter(c);
            occurrences++;
        }

        Assert.Equal(6, occurrences);
        Assert.Equal(3, factory.CreatedCharacterCount); // b, a, n
    }
}
