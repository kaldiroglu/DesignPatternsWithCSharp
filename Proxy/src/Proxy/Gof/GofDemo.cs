namespace dev.kaldiroglu.Proxy.Gof;

/// <summary>
/// Runs the GoF page-207 scenario end to end so the laziness of the virtual
/// proxy is visible on the console.
/// </summary>
public static class GofDemo
{
    public static void Run()
    {
        Console.WriteLine("== Building the document (insert proxies — no image is loaded) ==");
        var doc = new TextDocument();
        doc.Insert(new ImageProxy("diagram.png", new Extent(800, 600)));
        doc.Insert(new ImageProxy("photo.png", new Extent(1024, 768)));

        Console.WriteLine("\n== Laying out the page (uses GetExtent() — still no image loaded) ==");
        Console.WriteLine($"Total page extent: {doc.LayoutExtent()}");

        Console.WriteLine("\n== Drawing the page (first draw forces the real images to load) ==");
        doc.Draw(new Point(0, 0));

        Console.WriteLine("\n== Drawing again (images already loaded — no reload) ==");
        doc.Draw(new Point(0, 0));
    }
}
