using dev.kaldiroglu.Proxy.Gof;
using Xunit;

namespace dev.kaldiroglu.Proxy.Tests;

/// <summary>The GoF virtual proxy: an <see cref="ImageProxy"/> defers loading the real <see cref="Image"/>.</summary>
public class GofProxyTests
{
    [Fact(DisplayName = "A fresh proxy has not loaded the real image")]
    public void FreshProxyHasNotLoadedImage()
    {
        var proxy = new ImageProxy("diagram.png", new Extent(800, 600));

        Assert.False(proxy.IsImageLoaded);
        Assert.Equal(0, proxy.LoadCount);
    }

    [Fact(DisplayName = "GetExtent answers from the cache without loading the real image")]
    public void GetExtentDoesNotLoadImage()
    {
        var proxy = new ImageProxy("diagram.png", new Extent(800, 600));

        Extent extent = proxy.GetExtent();

        Assert.Equal(new Extent(800, 600), extent);
        Assert.False(proxy.IsImageLoaded); // still not loaded
    }

    [Fact(DisplayName = "The first Draw forces the real image to load exactly once")]
    public void DrawLoadsImageExactlyOnce()
    {
        var proxy = new ImageProxy("diagram.png", new Extent(800, 600));

        proxy.Draw(new Point(0, 0));
        proxy.Draw(new Point(0, 0)); // second draw must not reload

        Assert.True(proxy.IsImageLoaded);
        Assert.Equal(1, proxy.LoadCount);
    }

    [Fact(DisplayName = "Laying out a document never loads any real image")]
    public void LayoutNeverLoadsImages()
    {
        var a = new ImageProxy("a.png", new Extent(800, 600));
        var b = new ImageProxy("b.png", new Extent(1024, 768));
        var doc = new TextDocument();
        doc.Insert(a);
        doc.Insert(b);

        Extent total = doc.LayoutExtent();

        Assert.Equal(new Extent(1024, 1368), total); // max width, summed height
        Assert.False(a.IsImageLoaded);
        Assert.False(b.IsImageLoaded);
    }

    [Fact(DisplayName = "The proxy is substitutable for the real image through IGraphic")]
    public void ProxyIsSubstitutableForRealImage()
    {
        IGraphic real = new Image("real.png");
        IGraphic proxy = new ImageProxy("proxy.png", new Extent(800, 600));

        // Both satisfy the Subject contract; the client uses them the same way.
        Assert.Equal(new Extent(800, 600), real.GetExtent());
        Assert.Equal(new Extent(800, 600), proxy.GetExtent());
    }
}
