using dev.kaldiroglu.Composite.Gof.Graphics;
using Xunit;

namespace dev.kaldiroglu.Composite.Tests.Gof;

/// <summary>
/// Unit tests for the graphics example of GoF p. 163.
/// </summary>
/// <remarks>
/// They pin down the three properties that make this Composite and not just a
/// list: leaves and composites share one type, a composite may contain other
/// composites to any depth, and the transparent interface makes a leaf reject
/// child operations at run time.
/// </remarks>
public class GraphicsCompositeTests
{
    [Fact(DisplayName = "A leaf and a composite are both usable as a Graphic")]
    public void LeafAndCompositeShareTheComponentType()
    {
        var mixed = new Graphic[] { new Line(10), new Picture("empty"), new Text("hi") };

        // The client declares only Graphic; there is no type test anywhere.
        foreach (var graphic in mixed)
        {
            graphic.Draw(new Point(0, 0));
        }

        Assert.Equal(3, mixed.Length);
    }

    [Fact(DisplayName = "A Picture can nest other Pictures to arbitrary depth")]
    public void CompositesNest()
    {
        var innermost = new Line(5);

        var level3 = new Picture("level3");
        level3.Add(innermost);
        var level2 = new Picture("level2");
        level2.Add(level3);
        var level1 = new Picture("level1");
        level1.Add(level2);

        Assert.Same(innermost, level1.GetChild(0).GetChild(0).GetChild(0));
    }

    [Fact(DisplayName = "A composite reports its children; a leaf reports none")]
    public void ChildrenAreVisibleThroughTheComponentInterface()
    {
        var picture = new Picture("drawing");
        var rectangle = new Rectangle(3, 4);
        picture.Add(rectangle);

        Assert.Equal(new Graphic[] { rectangle }, picture.Children);
        Assert.True(picture.IsComposite);

        Graphic leaf = new Text("leaf");
        Assert.Empty(leaf.Children);
        Assert.False(leaf.IsComposite);
    }

    [Fact(DisplayName = "Remove() detaches a child from its composite")]
    public void RemoveDetachesAChild()
    {
        var picture = new Picture("drawing");
        var line = new Line(1);
        picture.Add(line);
        picture.Remove(line);

        Assert.Empty(picture.Children);
    }

    [Fact(DisplayName = "The price of transparency: a leaf rejects child operations")]
    public void LeavesRejectChildOperations()
    {
        Graphic leaf = new Line(1);

        Assert.Throws<NotSupportedException>(() => leaf.Add(new Line(2)));
        Assert.Throws<NotSupportedException>(() => leaf.Remove(new Line(2)));
        Assert.Throws<NotSupportedException>(() => leaf.GetChild(0));
    }

    [Fact(DisplayName = "A recursive walk over the tree needs no knowledge of concrete types")]
    public void AUniformWalkCountsEveryNode()
    {
        var drawing = new Picture("drawing");
        drawing.Add(new Line(100));
        drawing.Add(new Text("Composite"));
        var logo = new Picture("logo");
        logo.Add(new Rectangle(40, 20));
        logo.Add(new Line(40));
        drawing.Add(logo);

        Assert.Equal(6, CountNodes(drawing)); // 2 pictures + 4 primitives
    }

    private static int CountNodes(Graphic graphic)
    {
        var count = 1;
        foreach (var child in graphic.Children)
        {
            count += CountNodes(child);
        }

        return count;
    }
}
