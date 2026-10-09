using System.Reflection;
using dev.kaldiroglu.Strategy.Gof;
using dev.kaldiroglu.Strategy.Gof.Solution;
using Xunit;
using NaiveComposition = dev.kaldiroglu.Strategy.Gof.Problem.Composition;

namespace dev.kaldiroglu.Strategy.Tests.Gof;

/// <summary>
/// GoF's own example (Design Patterns, pp. 315-316): a document that has to break text into
/// lines. The paragraph is set in a 26-column measure, a width at which the two text
/// algorithms disagree. Ported from the Java <c>gof.CompositorTest</c>.
/// </summary>
public class CompositorTests
{
    private const string Text =
        "A document editor breaks a stream of text into lines "
        + "and there are many algorithms for it";
    private const int Width = 26;

    private static IReadOnlyList<Component> Paragraph() =>
        [.. Text.Split(' ').Select(Component.Word)];

    private static Composition DocumentWith(ICompositor compositor)
    {
        var document = new Composition(Width, compositor);
        foreach (var component in Paragraph())
        {
            document.Insert(component);
        }
        return document;
    }

    [Fact(DisplayName = "the same paragraph, two algorithms, two different sets of lines")]
    public void TwoAlgorithmsOneParagraph()
    {
        var simple = DocumentWith(new SimpleCompositor()).Repair();
        var tex = DocumentWith(new TeXCompositor()).Repair();

        // The same number of lines. The difference is where they fall.
        Assert.Equal(4, simple.LineCount);
        Assert.Equal(4, tex.LineCount);
        Assert.NotEqual(simple.Render(), tex.Render());

        Assert.Equal(["A document editor breaks a", "stream of text into lines",
            "and there are many", "algorithms for it"], simple.Render());
        Assert.Equal(["A document editor breaks", "a stream of text into",
            "lines and there are many", "algorithms for it"], tex.Render());
    }

    [Fact(DisplayName = "TeX reads the whole paragraph, so it leaves a smaller worst gap")]
    public void TexEvensTheLinesOut()
    {
        var simple = DocumentWith(new SimpleCompositor()).Repair();
        var tex = DocumentWith(new TeXCompositor()).Repair();

        Assert.Equal(8, simple.WorstSlack);
        Assert.Equal(5, tex.WorstSlack);
        Assert.True(tex.WorstSlack < simple.WorstSlack);

        foreach (var layout in new[] { simple, tex })
        {
            for (var i = 0; i < layout.LineCount; i++)
            {
                Assert.True(layout.WidthOf(i) <= Width, "line " + i + " overflowed the measure");
            }
        }
    }

    [Fact(DisplayName = "the algorithm can be replaced on a document that already exists")]
    public void TheCompositorIsAField()
    {
        var document = DocumentWith(new SimpleCompositor());
        Assert.Equal("SimpleCompositor", document.CompositorName);
        var before = document.Repair().Render();

        document.SetCompositor(new TeXCompositor());     // the same document

        Assert.Equal("TeXCompositor", document.CompositorName);
        Assert.NotEqual(before, document.Repair().Render());
    }

    [Fact(DisplayName = "ArrayCompositor ignores the measure entirely, and is still a Compositor")]
    public void TheThirdAlgorithmIsNothingLikeTheOthers()
    {
        var rows = DocumentWith(new ArrayCompositor(6)).Repair();

        // It counts components, not columns: six to a row, and the first row is 33 wide in
        // a 26-column measure.
        Assert.Equal(6, rows.Lines[0].Count);
        Assert.Equal(33, rows.WidthOf(0));
        Assert.True(rows.WidthOf(0) > Width);
        Assert.True(typeof(ICompositor).IsAssignableFrom(typeof(ArrayCompositor)));
    }

    [Fact(DisplayName = "the context forwards one call; the flag that chose the algorithm is gone")]
    public void TheContextOnlyForwards()
    {
        // The naive class took a bool to pick between two hard-wired algorithms.
        Assert.Contains(typeof(NaiveComposition).GetConstructors()[0].GetParameters(),
            p => p.ParameterType == typeof(bool));

        // The context takes an ICompositor instead, and holds no bool at all.
        Assert.Contains(typeof(Composition).GetConstructors()[0].GetParameters(),
            p => p.ParameterType == typeof(ICompositor));
        Assert.DoesNotContain(
            typeof(Composition).GetFields(BindingFlags.Instance | BindingFlags.Static
                                          | BindingFlags.Public | BindingFlags.NonPublic
                                          | BindingFlags.DeclaredOnly),
            f => f.FieldType == typeof(bool));
    }

    [Fact(DisplayName = "both designs lay the same paragraph out the same way")]
    public void TheDesignsAgree()
    {
        var fast = new NaiveComposition(Paragraph(), Width, false).Repair();
        var quality = new NaiveComposition(Paragraph(), Width, true).Repair();

        Assert.Equal(fast.Render(), DocumentWith(new SimpleCompositor()).Repair().Render());
        Assert.Equal(quality.Render(), DocumentWith(new TeXCompositor()).Repair().Render());
    }

    [Fact(DisplayName = "a row of no components is a mistake, and says so")]
    public void ArrayCompositorRejectsZero()
    {
        Assert.Throws<ArgumentException>(() => new ArrayCompositor(0));
    }
}
