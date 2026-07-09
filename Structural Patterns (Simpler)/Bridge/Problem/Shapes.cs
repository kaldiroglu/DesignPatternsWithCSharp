namespace DevKaldiroglu.DP.Structural.Bridge.Problem;

public abstract class Shape
{
    public abstract void Draw();
}

public class RedCircle : Shape   { public override void Draw() => Console.WriteLine("Drawing a Red Circle"); }
public class BlueCircle : Shape  { public override void Draw() => Console.WriteLine("Drawing a Blue Circle"); }
public class RedSquare : Shape   { public override void Draw() => Console.WriteLine("Drawing a Red Square"); }
public class BlueSquare : Shape  { public override void Draw() => Console.WriteLine("Drawing a Blue Square"); }

public static class ProblemDemo
{
    public static void Run()
    {
        Shape[] shapes = { new RedCircle(), new BlueCircle(), new RedSquare(), new BlueSquare() };
        foreach (var s in shapes) s.Draw();
    }
}
