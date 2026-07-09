namespace DevKaldiroglu.DP.Structural.Bridge.Solution;

public interface IColor
{
    string ApplyColor();
}

public class RedColor  : IColor { public string ApplyColor() => "Red"; }
public class BlueColor : IColor { public string ApplyColor() => "Blue"; }

public abstract class Shape
{
    protected readonly IColor Color;
    protected Shape(IColor color) => Color = color;
    public abstract void Draw();
}

public class Circle : Shape
{
    public Circle(IColor color) : base(color) { }
    public override void Draw() => Console.WriteLine($"Drawing a {Color.ApplyColor()} Circle");
}

public class Square : Shape
{
    public Square(IColor color) : base(color) { }
    public override void Draw() => Console.WriteLine($"Drawing a {Color.ApplyColor()} Square");
}

public static class SolutionDemo
{
    public static void Run()
    {
        Shape[] shapes = {
            new Circle(new RedColor()),
            new Circle(new BlueColor()),
            new Square(new RedColor()),
            new Square(new BlueColor())
        };
        foreach (var s in shapes) s.Draw();
    }
}
