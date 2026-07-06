using Xunit;

using dev.kaldiroglu.Adapter.Gof;

using ObjectTextShape = dev.kaldiroglu.Adapter.Gof.TextShape;
using ClassTextShape = dev.kaldiroglu.Adapter.Gof.ClassAdapter.TextShape;

namespace dev.kaldiroglu.Adapter.Tests;

/// <summary>The GoF drawing-editor example: object adapter, class adapter, and the pluggable shape adapter.</summary>
public class GofTests
{
    [Fact]
    public void ObjectAdapter_TranslatesExtentIntoBoundingBox()
    {
        Shape shape = new ObjectTextShape(new TextView(new Point(10, 10), 100, 20, "Hi"));
        Assert.Equal(new Point(110, 30), shape.BoundingBox().TopRight);
        Assert.False(shape.IsEmpty());
    }

    [Fact]
    public void ObjectAdapter_DelegatesIsEmptyToAdaptee()
    {
        Assert.True(new ObjectTextShape(new TextView(new Point(0, 0), 0, 0, "")).IsEmpty());
    }

    [Fact]
    public void ClassAdapter_ComputesSameBoundingBox()
    {
        Shape shape = new ClassTextShape(new Point(10, 10), 100, 20, "Hi");
        Assert.Equal(new Point(110, 30), shape.BoundingBox().TopRight);
    }

    [Fact]
    public void PluggableShapeAdapter_AdaptsAnObjectViaLambdas()
    {
        var adapter = new dev.kaldiroglu.Adapter.Gof.Pluggable.PluggableShapeAdapter(
            "circle",
            () => new BoundingBox(new Point(20, 20), new Point(80, 80)),
            () => false);

        Assert.Equal(new Point(80, 80), adapter.BoundingBox().TopRight);
        Assert.False(adapter.IsEmpty());
    }
}
