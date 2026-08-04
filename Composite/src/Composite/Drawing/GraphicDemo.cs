namespace dev.kaldiroglu.Composite.Drawing;

/// <summary>
/// A display with shapes on it, and a group nested inside.
/// </summary>
/// <remarks>
/// Watch what the client has to know. Building the tree needs an
/// <see cref="ICompositeGraphic"/>; drawing it needs only an <see cref="IGraphic"/>. That
/// split is the cost of keeping <c>AddGraphic</c> off the Component, and the compile error at
/// the bottom is what it buys.
/// </remarks>
public static class GraphicDemo
{
    public static void Run()
    {
        var display = new Canvas("Display", "Light Green");
        display.AddGraphic(new Circle("Red Circle", "Red"));
        display.AddGraphic(new Circle("Blue Circle", "Blue"));
        display.AddGraphic(new Ellipse("Black Ellipse", "Black"));

        // A canvas inside a canvas — the whole point of the pattern.
        var logo = new Canvas("Logo", "White");
        logo.AddGraphic(new Triangle("Triangle", "Yellow"));
        logo.AddGraphic(new Rectangle("Rectangle", "Green"));
        display.AddGraphic(logo);

        display.ListGraphic();
        Console.WriteLine("****************");
        display.Draw();
        Console.WriteLine("****************");

        Console.WriteLine();
        Console.WriteLine($"shapes on the display: {display.ShapeCount()}");
        Console.WriteLine($"shapes in the logo   : {logo.ShapeCount()}");
        Console.WriteLine($"a single circle      : {new Circle("c", "Red").ShapeCount()}");
        Console.WriteLine("  The client asked one object. Five shapes answered, two of them");
        Console.WriteLine("  a level down, and nobody wrote a loop.");

        Console.WriteLine();
        IGraphic asComponent = display;   // a Canvas is an IGraphic, so this needs no cast
        asComponent.Draw();

        Console.WriteLine();
        Console.WriteLine("A leaf cannot be given children, and the compiler says so:");
        Console.WriteLine("    new Circle(\"c\", \"Red\").AddGraphic(..)   does not compile");
        Console.WriteLine("That is the safety this namespace chose. Its price is that a client");
        Console.WriteLine("building a tree must hold ICompositeGraphic, not IGraphic.");
    }
}
