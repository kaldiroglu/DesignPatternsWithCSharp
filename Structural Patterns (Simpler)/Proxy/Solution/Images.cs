namespace DevKaldiroglu.DP.Structural.Proxy.Solution;

public interface IImage
{
    void Display();
}

public class RealImage : IImage
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

public class ImageProxy : IImage
{
    private readonly string _path;
    private RealImage? _real;

    public ImageProxy(string path) => _path = path;

    public void Display()
    {
        _real ??= new RealImage(_path);
        _real.Display();
    }
}

public static class SolutionDemo
{
    public static void Run()
    {
        IImage[] gallery =
        {
            new ImageProxy("photo1.jpg"),
            new ImageProxy("photo2.jpg"),
            new ImageProxy("photo3.jpg"),
            new ImageProxy("photo4.jpg"),
            new ImageProxy("photo5.jpg")
        };

        Console.WriteLine("Gallery built (no disk I/O yet).");
        gallery[2].Display();
        gallery[2].Display(); // cached
    }
}
