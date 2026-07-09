namespace DevKaldiroglu.DP.Structural.Proxy.Problem;

public class RealImage
{
    private readonly string _path;

    public RealImage(string path)
    {
        _path = path;
        LoadFromDisk();
    }

    private void LoadFromDisk() => Console.WriteLine($"Loading from disk: {_path}");

    public void Display() => Console.WriteLine($"Displaying: {_path}");
}

public static class ProblemDemo
{
    public static void Run()
    {
        var gallery = new[]
        {
            new RealImage("photo1.jpg"),
            new RealImage("photo2.jpg"),
            new RealImage("photo3.jpg"),
            new RealImage("photo4.jpg"),
            new RealImage("photo5.jpg")
        };
        gallery[2].Display();
    }
}
