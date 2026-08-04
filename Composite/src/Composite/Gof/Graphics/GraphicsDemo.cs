namespace dev.kaldiroglu.Composite.Gof.Graphics;

/// <summary>
/// Client of the Composite pattern — the "graphics editor" of GoF p. 163.
/// </summary>
/// <remarks>
/// The client builds a tree and then issues a single request to its root. It
/// never distinguishes a primitive from a container.
/// </remarks>
public static class GraphicsDemo
{
    public static void Run()
    {
        // A small drawing: two primitives plus a nested picture.
        var drawing = new Picture("drawing");
        drawing.Add(new Line(100));
        drawing.Add(new Text("Composite"));

        var logo = new Picture("logo");
        logo.Add(new Rectangle(40, 20));
        logo.Add(new Line(40));
        drawing.Add(logo); // a Picture inside a Picture — arbitrary depth

        Console.WriteLine("--- Drawing the whole tree with one call ---");
        drawing.Draw(new Point(0, 0));

        Console.WriteLine();
        Console.WriteLine("--- The client treats a leaf exactly the same way ---");
        Graphic anything = new Text("a lone leaf");
        anything.Draw(new Point(5, 5)); // same call, no type test

        Console.WriteLine();
        Console.WriteLine("--- Transparency has a price: leaves reject child operations ---");
        try
        {
            anything.Add(new Line(1));
        }
        catch (NotSupportedException e)
        {
            Console.WriteLine($"Rejected as expected: {e.Message}");
        }
    }
}
