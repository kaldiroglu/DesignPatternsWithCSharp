using System.Reflection;
using dev.kaldiroglu.Flyweight;
using Xunit;

namespace dev.kaldiroglu.Flyweight.Tests;

/// <summary>
/// GoF's own example, and the one that shows the solution's least obvious half: the same
/// shared object rendered in two different fonts, because the font was never inside it.
/// <para>
/// The two lines below are the ones the Java original types, so the figures here are the
/// figures that repository asserts and the deck quotes.
/// </para>
/// </summary>
public class FlyweightTests
{
    private const string LineOne = "flyweight is a nice solution";
    private const string LineTwo = "lightweight is also a nice solution";

    private sealed record Built(Column Document, GlyphFactory Factory, int Occurrences);

    private static Built Document()
    {
        var factory = new GlyphFactory();
        Column document = factory.CreateColumn();
        var occurrences = 0;
        foreach (var line in new[] { LineOne, LineTwo })
        {
            Row row = factory.CreateRow();
            foreach (var c in line)
            {
                row.Insert(factory.CreateCharacter(c));
                occurrences++;
            }

            document.Insert(row);
        }

        return new Built(document, factory, occurrences);
    }

    [Fact(DisplayName = "sixty-three characters of text cost sixteen objects")]
    public void SharingIsMeasured()
    {
        Built built = Document();

        Assert.Equal(63, built.Occurrences);
        Assert.Equal(16, built.Factory.CreatedCharacterCount);
        Assert.Equal(47, built.Occurrences - built.Factory.CreatedCharacterCount);
    }

    [Fact(DisplayName = "the factory returns the identical object for a repeated letter")]
    public void TheFactoryShares()
    {
        var factory = new GlyphFactory();

        CharacterGlyph first = factory.CreateCharacter('e');
        CharacterGlyph second = factory.CreateCharacter('e');

        Assert.Same(first, second);
        Assert.Equal(1, factory.CreatedCharacterCount);
    }

    [Fact(DisplayName = "distinct letters get distinct flyweights")]
    public void DistinctLettersAreDistinctObjects()
    {
        var factory = new GlyphFactory();

        Assert.NotSame(factory.CreateCharacter('a'), factory.CreateCharacter('b'));
        Assert.Equal(2, factory.CreatedCharacterCount);
    }

    [Fact(DisplayName = "rows and columns are unshared — each CreateRow is a new object")]
    public void UnsharedConcreteFlyweightsAreNotPooled()
    {
        var factory = new GlyphFactory();

        Row one = factory.CreateRow();
        Row two = factory.CreateRow();

        Assert.NotSame(one, two);        // a row owns its children, so it cannot be shared
        Assert.IsAssignableFrom<Glyph>(one);  // and it is still a Glyph, which is what lets
    }                                         // it hold shared characters

    [Fact(DisplayName = "only the factory can create a character glyph")]
    public void TheConstructorIsNotPublic()
    {
        ConstructorInfo[] constructors = typeof(CharacterGlyph)
            .GetConstructors(BindingFlags.Instance | BindingFlags.Public
                             | BindingFlags.NonPublic);

        Assert.Single(constructors);
        Assert.False(constructors[0].IsPublic,
            "a public constructor would let a client defeat the sharing");
    }

    [Fact(DisplayName = "the flyweight stores no font — one object renders in two of them")]
    public void TheSameObjectRendersInTwoFonts()
    {
        Built built = Document();

        var context = new GlyphContext(new Font("Helvetica"));
        context.Reset();
        context.SetFont(new Font("Times"), LineOne.Length);
        context.Next(LineOne.Length);
        context.SetFont(new Font("Courier"), LineTwo.Length);

        var window = new Window();
        context.Reset();
        built.Document.Draw(window, context);

        // 'i' appears on both lines, and it is one object.
        Window.RenderedGlyph first = window.Rendered.First(r => r.Charcode == 'i' && r.Y == 0);
        Window.RenderedGlyph second = window.Rendered.First(r => r.Charcode == 'i' && r.Y == 1);

        Assert.Equal(new Font("Times"), first.Font);
        Assert.Equal(new Font("Courier"), second.Font);
        Assert.Equal(63, window.Rendered.Count);
    }

    [Fact(DisplayName = "no field of the flyweight can hold a font")]
    public void TheFlyweightHasNoFontField()
    {
        FieldInfo[] fields = typeof(CharacterGlyph)
            .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        Assert.DoesNotContain(fields, f => f.FieldType == typeof(Font));
        Assert.Single(fields);   // one field: the character code
    }

    [Fact(DisplayName = "the document renders back to the text that was typed")]
    public void TheDocumentIsCorrect()
    {
        Built built = Document();

        var window = new Window();
        var context = new GlyphContext(new Font("Helvetica"));
        built.Document.Draw(window, context);

        Assert.Equal(LineOne + "\n" + LineTwo, window.Text());
    }
}
