using dev.kaldiroglu.Composite.Drawing;
using Xunit;

namespace dev.kaldiroglu.Composite.Tests.Drawing;

/// <summary>
/// The canvas example: the safe variant, where child management lives on
/// <see cref="ICompositeGraphic"/> and a leaf simply has no such method.
/// </summary>
public class CanvasTests
{
    [Fact(DisplayName = "A leaf is one shape")]
    public void ALeafIsOneShape() => Assert.Equal(1, new Circle("c", "Red").ShapeCount());

    [Fact(DisplayName = "A canvas adds up its children, at any depth")]
    public void ShapeCountRollsUp()
    {
        var display = new Canvas("Display", "Light Green");
        display.AddGraphic(new Circle("Red Circle", "Red"));
        display.AddGraphic(new Circle("Blue Circle", "Blue"));
        display.AddGraphic(new Ellipse("Black Ellipse", "Black"));

        var logo = new Canvas("Logo", "White");
        logo.AddGraphic(new Triangle("Triangle", "Yellow"));
        logo.AddGraphic(new Rectangle("Rectangle", "Green"));
        display.AddGraphic(logo);

        Assert.Equal(2, logo.ShapeCount());
        Assert.Equal(5, display.ShapeCount());   // three of its own, two a level down
    }

    [Fact(DisplayName = "A canvas is a graphic, so a canvas can hold a canvas")]
    public void CompositesNest()
    {
        var outer = new Canvas("outer", "White");
        var inner = new Canvas("inner", "White");
        outer.AddGraphic(inner);

        IGraphic asComponent = outer;   // no cast needed: a Canvas is an IGraphic
        Assert.Single(outer.Graphics);
        Assert.Equal(0, asComponent.ShapeCount());
    }

    [Fact(DisplayName = "Removing a child detaches it")]
    public void RemoveDetaches()
    {
        var canvas = new Canvas("c", "White");
        var circle = new Circle("circle", "Red");
        canvas.AddGraphic(circle);
        canvas.RemoveGraphic(circle);

        Assert.Empty(canvas.Graphics);
    }

    [Fact(DisplayName = "The safe variant: a leaf has no AddGraphic to call")]
    public void ALeafCannotBeGivenChildren()
    {
        // new Circle("c", "Red").AddGraphic(..) does not compile — that is the point.
        Assert.False(typeof(Circle).IsAssignableTo(typeof(ICompositeGraphic)));
        Assert.True(typeof(Canvas).IsAssignableTo(typeof(ICompositeGraphic)));
    }
}
