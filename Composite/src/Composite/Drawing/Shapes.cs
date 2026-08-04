namespace dev.kaldiroglu.Composite.Drawing;

/// <summary>
/// A Leaf: a circle has no children, and says so by not implementing
/// <see cref="ICompositeGraphic"/>.
/// </summary>
public class Circle(string name, string color) : GraphicObject(name, color)
{
    public override void Draw() => Console.WriteLine($"Drawing a circle: {Name}");

    public override void Erase() => Console.WriteLine($"Erasing a circle: {Name}");

    public override void Paint() =>
        Console.WriteLine($"Painting a circle: {Name}, color {Color}");
}

/// <summary>A Leaf.</summary>
public class Ellipse(string name, string color) : GraphicObject(name, color)
{
    public override void Draw() => Console.WriteLine($"Drawing an ellipse: {Name}");

    public override void Erase() => Console.WriteLine($"Erasing an ellipse: {Name}");

    public override void Paint() =>
        Console.WriteLine($"Painting an ellipse: {Name}, color {Color}");
}

/// <summary>A Leaf.</summary>
public class Triangle(string name, string color) : GraphicObject(name, color)
{
    public override void Draw() => Console.WriteLine($"Drawing a triangle: {Name}");

    public override void Erase() => Console.WriteLine($"Erasing a triangle: {Name}");

    public override void Paint() =>
        Console.WriteLine($"Painting a triangle: {Name}, color {Color}");
}

/// <summary>A Leaf.</summary>
public class Rectangle(string name, string color) : GraphicObject(name, color)
{
    public override void Draw() => Console.WriteLine($"Drawing a rectangle: {Name}");

    public override void Erase() => Console.WriteLine($"Erasing a rectangle: {Name}");

    public override void Paint() =>
        Console.WriteLine($"Painting a rectangle: {Name}, color {Color}");
}
